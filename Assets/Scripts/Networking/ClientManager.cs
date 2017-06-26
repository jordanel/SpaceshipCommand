using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine.UI;

public class ClientManager : MonoBehaviour {

	int maxConnections = 1;
	int socketPort = 8889;

	// Set during socket setup in Start()
	int channelId;
	int socketId;
	int connectionId;

	int player;          // player number provided by server
	HashSet<MType> modules; // list of modules

	void Start() {

		Application.runInBackground = true;

		// Initialization to open socket on port specified by socketId.
		NetworkTransport.Init();                                          // init transport using default values
		ConnectionConfig config = new ConnectionConfig();                 // create connection config + channel
		channelId = config.AddChannel(QosType.Reliable);
		HostTopology topology = new HostTopology(config, maxConnections); // create topology based on connection config
		socketId = NetworkTransport.AddHost(topology, socketPort);        // create host based on topology, listening on port 8888
		Debug.Log("Socket Open. SocketId is: " + socketId);               // print socket ID to console

		modules = new HashSet<MType>();
		modules.Add(MType.Init);
	}

	public Text messageDisplay;          // FOR DEBUGGING PURPOSES ONLY
	// Checks for received messages and displays their contents in a text display.
	void Update() {
		string received = null;

		do {
			received = ReceiveMessage();

			if (received != null) {

				SCMessage message = JsonUtility.FromJson<SCMessage>(received);

				if (!modules.Contains(message.type)) {              // player not authorized to receive message
					Debug.Log("Illegal " + message.type.ToString() + " message received from server");
					return;
				}

				switch (message.type) {
					case MType.Init:
						handleInit(received);
						break;
					case MType.Pilot:
						handlePilot(received);
						break;
					case MType.Nav:
						handleNav(received);
						break;
					default:
						break;
				}
			}
		} while (received != null);
	}

	// Connect to a given ip and port.
	public void Connect(string ip, int port) {
		byte error;
		connectionId = NetworkTransport.Connect(socketId, ip, port, 0, out error);
		if ((NetworkError)error != NetworkError.Ok) {
			Debug.Log("Error connecting to server: " + ((NetworkError)error).ToString());
		} else {
			Debug.Log("Connected to server. ConnectionId: " + connectionId + " ip: " + ip);
		}
	}

	// Receives and returns messages from server in JSON form.
	public void SendSocketMessage(CSMessage message) {
		byte error;
		byte[] buffer = new byte[1024];

		Stream stream = new MemoryStream(buffer);
		BinaryFormatter formatter = new BinaryFormatter();

		message.setPlayer(player);                                 // set player value to client's player number for validation
		string str = JsonUtility.ToJson(message);

		formatter.Serialize(stream, str);
		NetworkTransport.Send(socketId, connectionId, channelId, buffer, buffer.Length, out error);
		if ((NetworkError)error != NetworkError.Ok) { Debug.Log("Error sending message: " + ((NetworkError)error).ToString()); }
	}

	// Checks for received message through socket - if no message, ignores.
	string ReceiveMessage() {
		int hostId;
		//int connectionId;
		int channelId;
		byte[] buffer = new byte[1024];
		int dataSize;
		byte error;
		NetworkEventType evt = NetworkTransport.Receive(out hostId, out connectionId, out channelId, buffer, buffer.Length, out dataSize, out error);
		if ((NetworkError)error != NetworkError.Ok) { Debug.Log("Error receiving message: " + ((NetworkError)error).ToString()); }

		switch (evt) {
			case NetworkEventType.Nothing:
				return null;
			case NetworkEventType.ConnectEvent:
				onConnect(hostId, connectionId, channelId, (NetworkError)error);
				return null;
			// Received JSON - parse and use data
			case NetworkEventType.DataEvent:
				string message = onReceive(hostId, connectionId, channelId, buffer, (NetworkError)error);
				return message;
			case NetworkEventType.DisconnectEvent:
				onDisconnect(hostId, connectionId, channelId, (NetworkError)error);
				return null;
			default:
				Debug.LogError("Unknown network message type received: " + evt);
				return null;
		}
	}

	// Handles new connection.
	void onConnect(int hostId, int connectionId, int channelId, NetworkError error) {
		Debug.Log("Incoming connection event received");

		this.connectionId = connectionId;
	}

	// Handles disconnect (currently no logic).
	void onDisconnect(int hostId, int connectionId, int channelId, NetworkError error) {
		Debug.Log("remote client event disconnected");
	}

	// Deserializes received message into JSON string.
	string onReceive(int hostId, int connectionId, int channelId, byte[] buffer, NetworkError error) {

		Stream stream = new MemoryStream(buffer);
		BinaryFormatter formatter = new BinaryFormatter();
		string message = formatter.Deserialize(stream) as string;

		Debug.Log("incoming message connectionId: " + connectionId);
		return message;
	}

	void handleInit(string message) {
		SCInit m = JsonUtility.FromJson<SCInit>(message);

		switch(m.sub) {
			case SCInit.SubType.Init:
				player = m.player;                                               // Set player number to be sent to server in all subsequent messages
				break;
			case SCInit.SubType.RegisterTypes:
				foreach (MType type in m.moduleList) {
					modules.Add(type);
				}
				break;
		}
		messageDisplay.text = "Type: " + m.type + ", Subtype: " + m.sub.ToString() + ", Player: " + m.player + ", Other: " + m.other;
	}

	void handlePilot(string message) {
		SCPilot m = JsonUtility.FromJson<SCPilot>(message);
		messageDisplay.text = "Type: " + m.type + ", Direction: " + m.direction + ", Other: " + m.other;
	}

	void handleNav(string message) {
		SCNav m = JsonUtility.FromJson<SCNav>(message);
		messageDisplay.text = "Type: " + m.type + ", Loc: " + m.loc + ", Other: " + m.other;
	}

}
