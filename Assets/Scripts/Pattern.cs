using UnityEngine;
using System.Collections;

public class Pattern : Actor
{
    
    public GameObject obj;
	public bool choosen = false;
    public ResourceType type;
    public int quantity;

    public override void DoAction(GameObject requester)
    {
        Instantiate(obj, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
