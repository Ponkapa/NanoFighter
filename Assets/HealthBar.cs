using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {
	public Player player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float currentpercent = player.Health / 100f;
		if (currentpercent < 0) {
			currentpercent = 0;
		}
		transform.localScale = new Vector3 (currentpercent, 1f, 1f);
	}
}
