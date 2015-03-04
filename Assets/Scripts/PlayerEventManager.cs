using UnityEngine;
using System.Collections;

public class PlayerEventManager : MonoBehaviour {

	public delegate void OnButton();
	public static event OnButton onButtonEvent;
	public delegate void OnTarget(Vector3 position);
	public static event OnTarget onTargetEvent;
	public delegate void OnPose();
	public static event OnPose onPoseEvent;
	public delegate void OnTargetExit();
	public static event OnTargetExit onTargetExitEvent;
    public delegate void OnMinionTarget(GameObject minion);
    public static event OnMinionTarget OnMinionTargetEvent;
	public float camRayLength = 10;  


	void Update()
	{
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * camRayLength, Color.red);
		// Create a RaycastHit variable to store information about what was hit by the ray.
		RaycastHit hit;
		if(Physics.Raycast (ray, out hit, camRayLength))
		{
			if(Input.GetKey(KeyCode.LeftShift) && Input.GetButtonDown("Fire1"))
			{
				onPoseEvent();
			}
			else if(Input.GetButtonDown("Fire1"))
			{
				FireCondition(hit);
			}
			else if(Input.GetKey(KeyCode.LeftShift))
			{
				TargetCondition(hit);
			}
			else if(Input.GetKeyUp(KeyCode.LeftShift))
			{
				onTargetExitEvent();
			}
		}
		else
		{
			onTargetExitEvent();
		}
	}

	void FireCondition(RaycastHit hit)
	{
		if(hit.collider.gameObject.tag == "Button")
			onButtonEvent();
        if (hit.collider.gameObject.tag == "Minion")
            OnMinionTargetEvent(hit.collider.gameObject);
	}

	void TargetCondition(RaycastHit hit)
	{
        if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Floor"))
        //if(true)
        {

			onTargetEvent(hit.point);
		}
		else
		{
            //Debug.Log("TARGET OFF");
			onTargetExitEvent();
		}
	}
}