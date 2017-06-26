using System.Collections.Generic;

[System.Serializable]

public class SCInit : SCMessage {

	public enum SubType { Init, RegisterTypes }

	public SubType sub;
	public int player;
	public List<MType> moduleList;

	public SCInit(SubType sub, int player, List<MType> moduleList, string other) : base(MType.Init, other) {
		this.sub = sub;
		this.player = player;
		this.moduleList = moduleList;
	}
}