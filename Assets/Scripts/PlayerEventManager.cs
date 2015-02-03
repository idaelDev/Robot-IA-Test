using UnityEngine;
using System.Collections;

public class PlayerEventManager : MonoBehaviour {

	public delegate void OnFire();
	public static event OnFire onFireEvent;
	public delegate void OnPose();
	public static event OnPose onPoseEvent;
	public delegate void OnTarget();
	public static event OnPose onTargetEvent;
	public delegate void OnTargetExit();
	public static event OnPose onTargetExitEvent;



	void Update()
	{
		if(Input.GetButtonDown("Fire1") && Input.GetKey(KeyCode.LeftShift))
		{
			onPoseEvent();
		}
		else if(Input.GetKey(KeyCode.LeftShift))
		{
			onTargetEvent();
		}
		else if(Input.GetButtonDown("Fire1"))
		{
			onFireEvent();
		}
		else if(Input.GetKeyUp(KeyCode.LeftShift))
		{
			onTargetExitEvent();
		}
	}


	
}