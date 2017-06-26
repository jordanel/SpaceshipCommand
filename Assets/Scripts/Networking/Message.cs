[System.Serializable]
public class Message {
	public string type;
	public int x;
	public int y;

	public Message(string type, int x, int y) {
		this.type = type;
		this.x = x;
		this.y = y;
	}
}
