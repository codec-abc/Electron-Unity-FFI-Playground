using UnityEngine;
using System.Collections;

#pragma warning disable 414

public class ModelViewControls : MonoBehaviour
{
	
	private int yMinLimit = 0;
	private int yMaxLimit = 80;

	private float yDeg = 0;
	private float xDeg= 0.0f;

 	float sensitivity = 1.25f;

	private float currentDistance;

	private Quaternion currentRotation;
	private Quaternion desiredRotation;
	private Quaternion rotation;
	[SerializeField]
	private float desiredDistance = 3.0f;
	[SerializeField]
	private float maxDistance = 6.0f;
	[SerializeField]
	private float minDistance = 9.0f;
	private Vector3 position;
	public GameObject targetObject;
	public GameObject camObject;


	void Start () 
	{
		currentDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
	}
	
	// Update is called once per frame
	void Update () 
	{
		CameraControlUpdate ();
	}

	void CameraControlUpdate()
	{			
		
//		yDeg += Input.GetAxis("Vertical")*sensitivity;
//		xDeg -= Input.GetAxis("Horizontal")*sensitivity;
//		yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);		
//		desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);		
//		rotation = Quaternion.Lerp(targetObject.transform.rotation, desiredRotation, 0.05f  );
//		targetObject.transform.rotation = desiredRotation;
//		desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
//		currentDistance = Mathf.Lerp(currentDistance, desiredDistance, 0.05f  );
//		position = targetObject.transform.position - (rotation * Vector3.forward * currentDistance );
//		Vector3 lerpedPos=Vector3.Lerp(camObject.transform.position,position,0.05f);
//		camObject.transform.position = lerpedPos;

	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360) 
		{
			angle += 360;
		}
		if (angle > 360) 
		{
			angle -= 360;
		}
		return Mathf.Clamp(angle, min, max);
	}
}
