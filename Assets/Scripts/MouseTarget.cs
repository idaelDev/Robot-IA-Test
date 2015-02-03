using UnityEngine;
using System.Collections;

public class MouseTarget : MonoBehaviour {


	public GameObject obj;
	public GameObject tile;
	private GameObject pTile;
	float camRayLength = 5;  
	int floorMask; 
	// Use this for initialization
	void Start () {
		floorMask = LayerMask.GetMask ("Floor");
		pTile = Instantiate(tile) as GameObject;
		pTile.transform.Rotate(new Vector3(90,0,0));
		pTile.SetActive(false);
		PlayerEventManager.onTargetEvent += Target;
		PlayerEventManager.onPoseEvent += InstantiatePattern;
		PlayerEventManager.onTargetExitEvent += TargetExit;
	}

	void TargetExit()
	{
		pTile.SetActive(false);
	}

	void Target()
	{
		Ray ray = new Ray(transform.position, transform.forward);
		// Create a RaycastHit variable to store information about what was hit by the ray.
		RaycastHit hit;
		// Perform the raycast and if it hits something on the floor layer...
		if(Physics.Raycast (ray, out hit, camRayLength))
		{
			if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Floor"))
			{
				// Create a vector from the player to the point on the floor the raycast from the mouse hit.
				Vector3 position = new Vector3((int)(hit.point.x)+0.5f,(int) (hit.point.y)+0.5f,(int)(hit.point.z)-0.5f);
				pTile.transform.position = position;
	//			pTile.transform.position = floorHit.point;
				pTile.SetActive(true);
			}
		}
		else
		{
			pTile.SetActive(false);
		}
	}

	void InstantiatePattern()
	{
		Ray ray = new Ray(transform.position, transform.forward);
		// Create a RaycastHit variable to store information about what was hit by the ray.
		RaycastHit hit;
		// Perform the raycast and if it hits something on the floor layer...
		if(Physics.Raycast (ray, out hit, camRayLength))
		{
			if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Floor"))
			{
				Vector3 position = pTile.transform.position;
				Instantiate(obj, position, Quaternion.identity);

			}
		}
	}
	

}
