using UnityEngine;
using System.Collections;

public class Pattern : Actor
{
    
    public GameObject obj;
    public ResourceType type;
    public int quantity;

    public override void DoAction(GameObject requester)
    {
        Instantiate(obj, transform.position, transform.rotation);
        requester.GetComponent<MinionInventory>().DropResource(type, quantity);
        Destroy(this.gameObject);
    }
}
