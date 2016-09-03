using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour {
	static bool isMade = false;
	// Use this for initialization
	void Awake(){
		DontDestroyOnLoad (transform.gameObject);
		if (Application.loadedLevelName == "Stuff" && isMade == true) {
			Destroy (gameObject);
		}
		isMade = true;


	}
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
