[System.Serializable]
public class SCPilot : SCMessage {
	public string direction;

	public SCPilot(string direction, string other) : base(MType.Pilot, other) {
		this.direction = direction;
	}
}