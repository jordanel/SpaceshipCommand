[System.Serializable]
public class SCNav : SCMessage {
	public int loc;

	public SCNav(int loc, string other) : base(MType.Nav, other) {
		this.loc = loc;
	}
}