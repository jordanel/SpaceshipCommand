[System.Serializable]
public class SCMessage {
	public MType type;               //specifies the type of message
	public string other;            //holds any extra data not in template

	public SCMessage(MType type, string other) {
		this.type = type;
		this.other = other;
	}
}