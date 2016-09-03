using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private enum Interruptable{
		Yes,
		No
	}
	private enum Flippable{
		Yes,
		No
	}
	private enum State
	{
		Grounded,
		Air
	}
	private enum Moveable{
		Yes,
		No
	}
	private enum Facing{
		Right,
		Left
	}
	private bool hit = false;
	private bool dash = false;
	State state;
	Flippable flippable;
	Interruptable interruptable;
	Facing facing = Facing.Right;
	Moveable moveable;
	public int hitstun;
	public int delay;
	public Hitbox hitbox;
	private Animator animator;
	private Rigidbody2D rigidbody;
	private BoxCollider2D collider;
	public int Health;
	public string A;
	public string B;
	public string X;
	public string Y;
	public string Horizontal;
	public string Vertical;
	public string Trigger;
	public string playerNum;
	public string Taunt;
	private bool crouching;
	private AudioSource audioSource;
	private int loadlevel = 0;
	// Use this for initialization
	void Start () {
		collider = GetComponent<BoxCollider2D> ();
		rigidbody = GetComponent<Rigidbody2D> ();
		animator = GetComponentInChildren<Animator> ();
		audioSource = GetComponent<AudioSource> ();
		hitbox.parent = gameObject;
		Health = 100;
	}

	void GroundP() {
		hitbox.Damage = 18;
		hitbox.Duration = 20;
		hitbox.Direction = 60;
		NormalizeHitbox ();
		hitbox.Hitstun = 24;
		hitbox.Bone =  transform.Find("Fighter1/bubbleguy_F_Hand").gameObject;
		var box = Instantiate (hitbox, transform.position, Quaternion.identity);
		var colliderSize = hitbox.GetComponent<BoxCollider2D> ();
		colliderSize.size = new Vector2 (2, 1);

	}
	void AirP(){
		hitbox.Damage = 15;
		hitbox.Duration = 20;
		hitbox.Direction = 45;
		NormalizeHitbox ();
		hitbox.Hitstun = 15;
		hitbox.Bone =  transform.Find("Fighter1/bubbleguy_F_Hand").gameObject;
		var box = Instantiate (hitbox, transform.position, Quaternion.identity);
		var colliderSize = hitbox.GetComponent<BoxCollider2D> ();
		colliderSize.size = new Vector2 (1, 1);
	}
	void CrouchP(){
		hitbox.Damage = 7;
		hitbox.Duration = 20;
		hitbox.Direction = 65;
		NormalizeHitbox ();
		hitbox.Hitstun = 12;
		hitbox.Bone = transform.Find("Fighter1/bubbleguy_F_Foot").gameObject;
		var box = Instantiate(hitbox, transform.position, Quaternion.identity);
		var colliderSize = hitbox.GetComponent<BoxCollider2D>();
		colliderSize.size = new Vector2(1, 1);

	}
	void GroundS(){
		hitbox.Damage = 5;
		hitbox.Duration = 12;
		hitbox.Direction = 30;
		NormalizeHitbox ();
		hitbox.Hitstun = 5;
		hitbox.Bone = transform.Find("Fighter1/bubbleguy_F_Hand").gameObject;
		var box = Instantiate(hitbox, transform.position, Quaternion.identity);
		var colliderSize = hitbox.GetComponent<BoxCollider2D>();
		colliderSize.size = new Vector2(1, 1);
	}
	void AirS(){
		hitbox.Damage = 10;
		hitbox.Duration = 40;
		hitbox.Direction = 10;
		NormalizeHitbox ();
		hitbox.Hitstun = 8;
		hitbox.Bone = transform.Find("Fighter1/bubbleguy_F_Foot").gameObject;
		var box = Instantiate(hitbox, transform.position, Quaternion.identity);
		var colliderSize = hitbox.GetComponent<BoxCollider2D>();
		colliderSize.size = new Vector2(1, 1);

	}
	void CrouchS(){
		hitbox.Damage = 5;
		hitbox.Duration = 10;
		hitbox.Direction = 10;
		NormalizeHitbox ();
		hitbox.Hitstun = 3;
		hitbox.Bone = transform.Find("Fighter1/bubbleguy_F_Foot").gameObject;
		var box = Instantiate(hitbox, transform.position, Quaternion.identity);
		var colliderSize = hitbox.GetComponent<BoxCollider2D>();
		colliderSize.size = new Vector2(1, 1);

	}
	void SB(){
		hitbox.Damage = 25;
		hitbox.Duration = 12;
		hitbox.Direction = 45;
		NormalizeHitbox ();
		hitbox.Hitstun = 60;
		hitbox.Bone = transform.Find("Fighter1/bubbleguy_Chest").gameObject;
		var box = Instantiate(hitbox, transform.position, Quaternion.identity);
		var colliderSize = hitbox.GetComponent<BoxCollider2D>();
		colliderSize.size = new Vector2(2, 2);

	}

	void getState()
	{
		AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
		if (currentState.IsName ("idle") || currentState.IsName ("walk")) {
			interruptable = Interruptable.Yes;
			flippable = Flippable.Yes;
			moveable = Moveable.Yes;
		} else if (currentState.IsName ("crouch")) {
			interruptable = Interruptable.Yes;
			flippable = Flippable.No;
			moveable = Moveable.No;
		}
		else if (currentState.IsName ("air") || currentState.IsName("jumptoair") || currentState.IsName("air_P2")) {
			interruptable = Interruptable.Yes;
			flippable = Flippable.No;
			moveable = Moveable.Yes;
		}
		else {
			interruptable = Interruptable.No;
			flippable = Flippable.No;
		}
		if (currentState.IsName("Land")){
			moveable = Moveable.No;
		}

	}

	public void Jump(){
		if (Input.GetButton (Y)) {
			rigidbody.velocity += new Vector2 (0, 50);
		} else {
			rigidbody.velocity += new Vector2 (0, 25);
		}
	}
	// Update is called once per frame
	void Update () {
		if (!animator.GetBool ("Dead")) {
			if (!hit) {
				getState ();
				delay = 0;
				Vector2 XMovement;
				if (moveable == Moveable.Yes) {
					XMovement = new Vector2 (Input.GetAxis (Horizontal) * 5, 0f);
				} else {
					XMovement = new Vector2 (0f, 0f); 
				}
				if (XMovement.x > 0 && flippable == Flippable.Yes && facing == Facing.Left) {
					transform.localScale = new Vector3 (1, 1, 1);
					facing = Facing.Right;
				}
				if (XMovement.x < 0 && flippable == Flippable.Yes && facing == Facing.Right) {
					transform.localScale = new Vector3 (-1, 1, 1);
					facing = Facing.Left;
				}
				if (Input.GetAxis (Vertical) < -0.5f) {
					animator.SetTrigger ("crouch");
					animator.ResetTrigger ("Stand");
					animator.ResetTrigger ("Walk");
					crouching = true;
				} else if (XMovement.x == 0) {
					animator.ResetTrigger ("crouch");
					animator.SetTrigger ("Stand");
					animator.ResetTrigger ("Walk");
					crouching = false;
				} else {
					animator.ResetTrigger ("crouch");
					animator.ResetTrigger ("Stand");
					animator.SetTrigger ("Walk");
					crouching = false;
				}
				if (Input.GetButtonDown (X) && interruptable == Interruptable.Yes) {
					animator.SetTrigger ("p");
					if (state == State.Grounded) {
						moveable = Moveable.No;
						if (crouching) {
							Invoke ("CrouchP", 0f);
						}	else{
							Invoke ("GroundP", 0.5f);
						}
					} else if (state == State.Air) {
						Invoke ("AirP", 0.2f);
					}
				}
				if (Input.GetButtonDown (B) && interruptable == Interruptable.Yes) {
					if (state == State.Grounded) {
						moveable = Moveable.No;
						animator.SetTrigger ("SB");
						Invoke ("SB", 0.2f);
					} else if (state == State.Air && !dash) {
						if (facing == Facing.Right) {
							if (XMovement.x < 0) {
								facing = Facing.Left;
								transform.localScale = new Vector3 (-1, 1, 1);
								rigidbody.velocity = new Vector2 (-50, 0);
							} else {
								rigidbody.velocity = new Vector2 (50, 0);
							}
							animator.SetTrigger ("dash");
						} else {
							if (XMovement.x > 0) {
								facing = Facing.Right;
								transform.localScale = new Vector3 (1, 1, 1);
								rigidbody.velocity = new Vector2 (50, 0);
							} else {
								rigidbody.velocity = new Vector2 (-50, 0);
							}
							animator.SetTrigger ("dash");
						}
						dash = true;
					}
				}
				if (Input.GetButtonDown (A) && interruptable == Interruptable.Yes) {
					animator.SetTrigger ("s");
					if (state == State.Grounded) {
						moveable = Moveable.No;
						if (crouching) {
							Invoke ("CrouchS", 0f);
						}	else{
							Invoke ("GroundS", 0f);
						}
					} else if (state == State.Air) {
						Invoke ("AirS", 0f);
					}
				}
				if (Input.GetButtonDown (Taunt) && state == State.Grounded && interruptable == Interruptable.Yes) {
					animator.SetTrigger ("TAUNTBUTTON");
				}
				if (Input.GetButtonDown (Y) && state == State.Grounded && interruptable == Interruptable.Yes) {
					state = State.Air;
					animator.SetTrigger ("Jump");
					Invoke ("Jump", 0.08333f);
				}
				if (Input.GetButtonDown (Trigger)) {
					if (state == State.Grounded) {
						animator.SetTrigger ("block");
					}
				}
				if (moveable == Moveable.Yes && Mathf.Abs (rigidbody.velocity.x) < 10) {
					rigidbody.velocity += XMovement;
				}
			} else {
				if (delay > hitstun) {
					hit = false;
					animator.ResetTrigger ("hit");
					animator.SetTrigger ("Recover");
				}
				delay++;
				animator.ResetTrigger ("Fall");
			}
		}
		loadlevel--;
		if (loadlevel < 0) {
			loadlevel = 0;
		} else if (loadlevel == 1) {
			Application.LoadLevel (Application.loadedLevelName);
		}
	}

	void OnCollisionStay2D (Collision2D col){
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("air") && col.gameObject.CompareTag("Floor")) {

			state = State.Grounded;
			animator.SetTrigger ("Land");
			animator.ResetTrigger ("Fall");
			hitstun = 0;
			dash = false;
		}

	}
	void OnCollisionEnter2D (Collision2D col){
		if (col.gameObject.CompareTag ("Floor") && !animator.GetBool("Land")) {
			state = State.Grounded;
			animator.SetTrigger("Land");
			animator.ResetTrigger ("Fall");
			dash = false;
			TerminateCollisions ();
		}
	}

	void OnCollisionExit2D (Collision2D col ){
		if (col.gameObject.CompareTag("Floor")) {
			Debug.Log ("Falling");
			animator.ResetTrigger ("Land");
			animator.ResetTrigger ("Jump");
			animator.SetTrigger ("Fall");
			state = State.Air;
			TerminateCollisions ();
		}

	}
	void OnTriggerEnter2D (Collider2D col){
		if (!animator.GetBool ("Dead")) {
			Hitbox hitbyhitbox = col.GetComponent<Hitbox> ();
			Vector2 HitVel;
			float knockback = hitbyhitbox.Damage * 4;
			float knockbackx = knockback * Mathf.Cos (hitbyhitbox.Direction * Mathf.PI / 180);
			float knockbacky = knockback * Mathf.Sin (hitbyhitbox.Direction * Mathf.PI / 180);
			Health -= hitbyhitbox.Damage;
			if (Health <= 0) {
				animator.SetTrigger ("DEAD!");
				animator.SetBool ("Dead", true);
				loadlevel = 180;
			}
			HitVel = new Vector2 (knockbackx, knockbacky);
			rigidbody.velocity += HitVel;
			hitstun = hitbyhitbox.Hitstun;
			hit = true;
			animator.SetTrigger ("hit");
			animator.ResetTrigger ("Stand");
			animator.ResetTrigger ("Fall");
			Destroy (col.gameObject);
			TerminateCollisions ();
			audioSource.Play ();
		}
	}

	void TerminateCollisions(){
		CancelInvoke ("GroundP");
		CancelInvoke ("AirP");
		CancelInvoke ("CrouchP");
		CancelInvoke ("GroundS");
		CancelInvoke ("AirS");
		CancelInvoke ("CrouchS");
		CancelInvoke ("SB");
		Hitbox[] allHitboxes = FindObjectsOfType<Hitbox> ();
		foreach (Hitbox hitbox in allHitboxes) {
			if (hitbox.transform.IsChildOf (transform)) {
				Destroy (hitbox.gameObject);
			}
		}
	}

	void NormalizeHitbox(){
		if (facing == Facing.Left && hitbox.Direction < 180) {
			if (hitbox.Direction < 180) {
				hitbox.Direction = 180 - hitbox.Direction;
			} else {
				hitbox.Direction = 540 - hitbox.Direction;
			}

		} 
	}

}
