[System.Serializable]
public class CSPilot : CSMessage {
	public string direction;

	public CSPilot(string direction, string other) : base(MType.Pilot, other) {
		this.direction = direction;
	}
}