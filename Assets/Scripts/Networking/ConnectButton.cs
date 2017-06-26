using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour {

	public Button button;
	public InputField input;
	public ClientManager net;

	// Use this for initialization
	void Start() {
		Button btn = button.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}
	
	void TaskOnClick() {
		string ip = input.text;
		net.Connect(ip, 8888);
	}
}
