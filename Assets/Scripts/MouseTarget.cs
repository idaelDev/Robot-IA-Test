using UnityEngine;
using System.Collections;

public class MouseTarget : MonoBehaviour {


	public GameObject[] obj;
	public GameObject[] tile;
	private GameObject pTile;
    private int index;
    public delegate void OnPoseEnd();
    public static event OnPoseEnd onPoseEndEvent;

	// Use this for initialization
	void Start () {
        index = 0;
        InstantiatePTile();
		PlayerEventManager.onTargetEvent += Target;
		PlayerEventManager.onPoseEvent += InstantiatePattern;
		PlayerEventManager.onTargetExitEvent += TargetExit;
        PlayerEventManager.OnChangeBlockTypeEvent += IncrementIndex;
	}

    void IncrementIndex()
    {
        index++;
        if (index >= obj.Length)
            index = 0;
        Destroy(pTile);
        InstantiatePTile();
    }

    void DecrementIndex()
    {
        index--;
        if (index < 0)
            index = obj.Length-1;
    }

    void InstantiatePTile()
    {
        pTile = Instantiate(tile[index]) as GameObject;
        pTile.transform.Rotate(new Vector3(90, 0, 0));
        pTile.SetActive(false);
    }

	void TargetExit()
	{
		pTile.SetActive(false);
	}

	void Target(Vector3 position)
	{
		// Create a vector from the player to the point on the floor the raycast from the mouse hit.
//		Vector3 position = new Vector3((int)(hit.point.x)+0.5f,(int) (hit.point.y)+0.5f,(int)(hit.point.z)-0.5f);
        //position = position + Vector3.up/2;
        position = new Vector3((position.x), (position.y) + 0.5f, (position.z));
		pTile.transform.position = position;
//			pTile.transform.position = floorHit.point;
		pTile.SetActive(true);
	}
	
	void InstantiatePattern()
	{
		Vector3 position = pTile.transform.position;
		Instantiate(obj[index], position, Quaternion.identity);
        onPoseEndEvent();
	}
}
