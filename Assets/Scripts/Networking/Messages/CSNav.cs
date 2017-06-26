[System.Serializable]
public class CSNav : CSMessage {
	public int loc;

	public CSNav(int loc, string other) : base(MType.Nav, other) {
		this.loc = loc;
	}
}