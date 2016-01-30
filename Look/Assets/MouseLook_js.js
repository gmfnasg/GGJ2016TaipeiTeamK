/* MouseLook_js.js */

public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
public var axes = RotationAxes.MouseXAndY;
public var sensitivityX : float = 8F;
public var sensitivityY : float = 8F;

public var minimumX : float = -360F;
public var maximumX : float = 360F;

public var minimumY : float = -60F;
public var maximumY : float = 60F;

private var rotationY : float = 0F;

function Update ()
{
	if (axes == RotationAxes.MouseXAndY)
	{
		var rotationX : float = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
			
		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
		transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
	}
	else if (axes == RotationAxes.MouseX)
	{
		transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
	}
	else
	{
		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
		transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
	}
}
	
function Start ()
{
	// Make the rigid body not change rotation
	if (GetComponent.<Rigidbody>())
		GetComponent.<Rigidbody>().freezeRotation = true;
}
