using UnityEngine;
using System.Collections;

public class Hitbox : MonoBehaviour {
	public int Damage;
	public int Direction;
	public int Duration;
	public int Hitstun;
	public GameObject parent;
	public GameObject Bone;
	private BoxCollider2D collider;
	private int i;
	// Use this for initialization
	void Start () {
		transform.parent = parent.transform;
		collider = GetComponent<BoxCollider2D>();
		this.transform.position = Bone.transform.position;
		i = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (i > Duration) {
			Destroy (gameObject);
		}
		i++;
		this.transform.position = Bone.transform.position;
	}
}
