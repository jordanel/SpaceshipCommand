using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine.UI;

// TODO: Add error handling (+ for Dictionaries), adjust dictionary logic

public enum MType { Init, Pilot, Nav };      // message types

public class ServerManager : MonoBehaviour {

	int maxConnections = 10;
	int socketPort = 8888;

	// Set during socket setup in Start()
	int socketId;             // hostId
	int channelId;

	IDictionary<int, HashSet<MType>> userList;
	IDictionary<MType, HashSet<int>> revUserList;

	void Start() {

		Application.runInBackground = true;

		// Initialization to open socket on port specified by socketId.
		NetworkTransport.Init();                                          // init transport using default values
		ConnectionConfig config = new ConnectionConfig();                 // create connection config + channel
		channelId = config.AddChannel(QosType.Reliable);
		HostTopology topology = new HostTopology(config, maxConnections); // create topology based on connection config
		socketId = NetworkTransport.AddHost(topology, socketPort);        // create host based on topology, listening on port 8888
		Debug.Log("Socket Open. SocketId is: " + socketId);               // print socket ID to console

		userList = new Dictionary<int, HashSet<MType>>();
		revUserList = new Dictionary<MType, HashSet<int>>();

		try {
			foreach (MType value in MType.GetValues(typeof(MType))) {         // initialize each message type list
				revUserList.Add(value, new HashSet<int>());
			}
		} catch(ArgumentException) {
			Debug.Log("Tried to add duplicate message type to revUserList!");
		}
	}

	public Text messageDisplay;          // FOR DEBUGGING PURPOSES ONLY
	// Checks for received messages and displays their contents in a text display.
	void Update() { //make do while loop - while received != null?
		string received = null;

		do {
			received = ReceiveMessage();

			if (received != null) {

				CSMessage message = JsonUtility.FromJson<CSMessage>(received);

				if (!revUserList[message.type].Contains(message.player)) {              // player not authorized to send message
					Debug.Log("Illegal " + message.type.ToString() + " message received from player " + message.player);
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
						Debug.Log("Invalid message type!");
						break;
				}
			}
		} while (received != null);
	}

	//	public void Connect(string ip, int port)
	//	{
	//		byte error;
	//		connectionId = NetworkTransport.Connect(socketId, ip, port, 0, out error);
	//		Debug.Log("Connected to client. ConnectionId: " + connectionId);
	//	}

	// Serializes message into JSON and sends to correct client based on message type.
	public void SendSocketMessage(SCMessage message) {
		byte error;
		byte[] buffer = new byte[1024];

		Stream stream = new MemoryStream(buffer);
		BinaryFormatter formatter = new BinaryFormatter();

		string str = JsonUtility.ToJson(message);

		foreach (int sendId in revUserList[message.type]) {    // send message to all receiving clients
			formatter.Serialize(stream, str);
			NetworkTransport.Send(socketId, sendId, channelId, buffer, buffer.Length, out error);
			if((NetworkError)error != NetworkError.Ok) { Debug.Log("Error sending message: " + ((NetworkError)error).ToString()); }
		}

	}

	// Serializes message and sends to client based on connection ID
	public void SendSocketMessage(SCMessage message, int userId) {
		byte error;
		byte[] buffer = new byte[1024];

		Stream stream = new MemoryStream(buffer);
		BinaryFormatter formatter = new BinaryFormatter();

		string str = JsonUtility.ToJson(message);

		formatter.Serialize(stream, str);
		NetworkTransport.Send(socketId, userId, channelId, buffer, buffer.Length, out error);
		if ((NetworkError)error != NetworkError.Ok) { Debug.Log("Error sending message: " + ((NetworkError)error).ToString()); }
	}

	// Receives and returns messages from clients in JSON form.
	string ReceiveMessage() {
		int hostId;
		int connectionId;
		int channelId;
		byte[] buffer = new byte[1024];
		int dataSize;
		byte error;
		NetworkEventType evt = NetworkTransport.Receive(out hostId, out connectionId, out channelId, buffer, buffer.Length, out dataSize, out error);
		if((NetworkError)error != NetworkError.Ok) { Debug.Log("Error receiving message: " + ((NetworkError)error).ToString()); }

		switch (evt) {
			case NetworkEventType.Nothing:
				return null;
			case NetworkEventType.ConnectEvent:
				onConnect(hostId, connectionId, channelId, (NetworkError) error);
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

	// Handles new connection by inserting connectionId into list to be referenced for messaging.
	void onConnect(int hostId, int connectionId, int channelId, NetworkError error) {
		Debug.Log("Incoming connection event received: " + connectionId);

		try {
			userList.Add(connectionId, new HashSet<MType>());
		} catch(ArgumentException) {
			Debug.Log("Tried to add duplicate connectionId to userList!");
		}

		List<MType> list = new List<MType>();
		registerUser(connectionId, MType.Init);
		switch(connectionId) {
			case 1:
				registerUser(connectionId, MType.Pilot);
				list.Add(MType.Pilot);
				break;
			case 2:
				registerUser(connectionId, MType.Nav);
				list.Add(MType.Nav);
				break;
			default:
				break;
		}

		//send init message with module stuff
		SCInit init = new SCInit(SCInit.SubType.Init, connectionId, null, null);    // send initialization message with client's connectionId - used for (weak) authentication
		SendSocketMessage(init, connectionId);
		//SCInit init2 = new SCInit(SCInit.SubType.RegisterTypes, connectionId, list, null);
		SendSocketMessage(new SCInit(SCInit.SubType.RegisterTypes, connectionId, list, null), connectionId);

	}

	// Handles disconnect (currently no logic).
	void onDisconnect(int hostId, int connectionId, int channelId, NetworkError error) {
		Debug.Log("Remote client disconnected");
	}

	// Deserializes received message into JSON string.
	string onReceive(int hostId, int connectionId, int channelId, byte[] buffer, NetworkError error) {

		Stream stream = new MemoryStream(buffer);
		BinaryFormatter formatter = new BinaryFormatter();
		string message = formatter.Deserialize(stream) as string;

		Debug.Log("Incoming message connectionId: " + connectionId);
		return message;
	}

	void handleInit(string message) {
		CSInit m = JsonUtility.FromJson<CSInit>(message);
		messageDisplay.text = "Type: " + m.type + ", Other: " + m.other;
	}

	void handlePilot(string message) {
		CSPilot m = JsonUtility.FromJson<CSPilot>(message);
		messageDisplay.text = "Type: " + m.type + ", Direction: " + m.direction + ", Other: " + m.other;
	}

	void handleNav(string message) {
		CSNav m = JsonUtility.FromJson<CSNav>(message);
		messageDisplay.text = "Type: " + m.type + ", Loc: " + m.loc + ", Other: " + m.other;
	}

	public void registerUser(int playerNo, MType type) {           // consider having this method send registration messages as well
		try {
			userList[playerNo].Add(type);
			revUserList[type].Add(playerNo);
		} catch(KeyNotFoundException) {
			Debug.Log("Invalid dictionary access during player registration! playerNo: " + playerNo + " type: " + type.ToString());
		}
	}
}
