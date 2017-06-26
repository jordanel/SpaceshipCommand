[System.Serializable]
public class CSInit : CSMessage {

	public CSInit(int loc, string other) : base(MType.Init, other) {
		
	}
}