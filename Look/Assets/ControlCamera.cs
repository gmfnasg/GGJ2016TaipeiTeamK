using UnityEngine;
using System.Collections;

public class ControlCamera : MonoBehaviour {
	public float jumpHeight = 10;
	public float jumpSpeed = 1f;
	public float jumpValue;
	public float fallSpeed = 1f;
	public bool onJump = false;
	public float groundHeight;

	void Start(){
		groundHeight = transform.position.y;
		Input.gyro.enabled = true;
	}

	void Update()  
	{
		Move ();

		if (Input.GetKeyDown (KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Space)) {
			onJump = true;
		}
		Jump ();
		Fall ();
	}  

	void Move(){
		Vector3 newPos = new Vector3 (Input.GetAxis("Horizontal")*0.1f, 0,Input.GetAxis("Vertical")*0.1f);
		transform.Translate (newPos);
	}

	void Jump(){
		//跳躍
		if (onJump) {
			float value = 0;
			if(jumpHeight > jumpValue){
				//往上
				jumpValue += jumpSpeed;
				value = 1;
			}else{
				onJump = false;
				jumpValue = 0;
				return;
			}
			transform.position += Vector3.up * jumpSpeed;
		}
	}

	void Fall(){
		if (onJump)
			return;

		if (OnGround())
			return;

		transform.position += Vector3.up * -fallSpeed;
	}

	bool OnGround(){
		if (transform.position.y < groundHeight) {
			transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
			return true;
		}
		if (transform.position.y == groundHeight)
			return true;

		return false;
	}
}
