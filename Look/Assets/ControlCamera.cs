using UnityEngine;
using System.Collections;

public class ControlCamera : MonoBehaviour {
    public string SystemName = "ControlCamera";

    public float jumpHeight = 10;
	public float jumpSpeed = 1f;
	public float jumpValue;
	public float fallSpeed = 1f;
	public bool onJump = false;
	public float groundHeight;
	public Transform headTransform;
    public float moveSpeed = 0.1f;

	void Start(){
		groundHeight = transform.position.y;
		Input.gyro.enabled = true;
	}

	void Update()  
	{
		
	}

    void FixedUpdate()
    {
        Move();

        if ((Input.GetKeyDown(KeyCode.Joystick1Button1) && Input.GetKey(KeyCode.Joystick1Button7)) || Input.GetKeyDown(KeyCode.Space))
        {
            onJump = true;
            DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
               DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
               SystemName,
               "跳"));
        }
        Jump();
        Fall();

        if ((Input.GetKeyDown(KeyCode.Joystick1Button8) && Input.GetKey(KeyCode.Joystick1Button7)) || Input.GetKeyDown(KeyCode.R))
        {
            DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
               DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
               SystemName,
               "重設視野中新點"));
            Cardboard.SDK.Recenter();
        }
    }

	void Move(){
		if (Input.GetAxis ("Horizontal") == 0 && Input.GetAxis ("Vertical") == 0)
			return;
		Vector3 newPos =  Vector3.zero;
		newPos = newPos + (headTransform.forward*Input.GetAxis("Vertical")* moveSpeed);
		newPos = newPos + (headTransform.right*Input.GetAxis("Horizontal")* moveSpeed);
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
