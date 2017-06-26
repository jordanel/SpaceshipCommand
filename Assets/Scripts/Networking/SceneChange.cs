using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void changeSceneClient() {
		SceneManager.LoadScene("ClientNetTest");
	}

	public void changeSceneServer() {
		SceneManager.LoadScene("ServerNetTest");
	}
}
