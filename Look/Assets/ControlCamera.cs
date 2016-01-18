using UnityEngine;
using System.Collections;

public class ControlCamera : MonoBehaviour {
	public float jumpHeight = 10;
	public float jumpSpeed = 1f;
	public float jumpValue;
	public float fallSpeed = 1f;
	public bool onJump = false;
	public float groundHeight;
	public Transform headTransform;

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
		if (Input.GetAxis ("Horizontal") == 0 && Input.GetAxis ("Vertical") == 0)
			return;
		Vector3 newPos =  Vector3.zero;
		newPos = newPos + (headTransform.forward*Input.GetAxis("Vertical"));
		newPos = newPos + (headTransform.right*Input.GetAxis("Horizontal"));
		transform.Translate (newPos, Space.World);
	}

	void Jump(){
		//跳躍
		if (onJump) {
			if(jumpHeight > jumpValue){
				//往上
				jumpValue += jumpSpeed;
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
