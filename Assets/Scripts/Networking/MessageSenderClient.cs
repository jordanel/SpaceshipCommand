using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageSenderClient : MonoBehaviour {

	public ClientManager net;
	public Button button;
	public Dropdown type;
	public InputField direction;
	public InputField loc;
	public InputField other;

	// Use this for initialization
	void Start() {
		Button btn = button.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick() {
		switch (type.value) {
			case 0: {          //Pilot
					CSPilot m = new CSPilot(direction.text, other.text);
					net.SendSocketMessage(m);
					break;
				}
			case 1: {        //Nav
					CSNav m = new CSNav(int.Parse(loc.text), other.text);
					net.SendSocketMessage(m);
					break;
				}
			default:
				break;
		}
	}

}
