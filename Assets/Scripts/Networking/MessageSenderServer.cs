using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageSenderServer: MonoBehaviour {

	public ServerManager net;
	public Button button;
	public Dropdown type;
	public InputField direction;
	public InputField loc;
	public InputField other;

	// Use this for initialization
	void Start () {
		Button btn = button.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick() {
		switch(type.value) {
			case 0: {          //Pilot
				SCPilot m = new SCPilot(direction.text, other.text);
				net.SendSocketMessage(m);
				break;
			} case 1: {        //Nav
				SCNav m = new SCNav(int.Parse(loc.text), other.text);
				net.SendSocketMessage(m);
				break;
			} default:
				break;
		}
	}

}
