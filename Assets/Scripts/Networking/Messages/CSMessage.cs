[System.Serializable]
public class CSMessage {
	public MType type;               // specifies the type of message
	public int player;              // indicates player # according to server
	public string other;            // holds any extra data not in template

	public CSMessage(MType type, string other) {
		this.type = type;
		player = 0;
		this.other = other;
	}

	public void setPlayer(int playerNo) {     // set player number (done by ClientManager)
		player = playerNo;
	}

}