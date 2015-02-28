using UnityEngine;
using System.Collections;

public class Resource : Actor
{

    public ResourceType type = ResourceType.ROCK;
    public int initialQuantity = 100;
    public int give = 10;

    private int quantity = 0;
    private bool isMined = false;

    public int Quantity
    {
        get
        {
            return quantity;
        }
    }

    public bool IsMined
    {
        get
        {
            return isMined;
        }
        set
        {
            isMined = value;
        }
    }

    public override void DoAction(GameObject requester)
    {
        requester.GetComponent<MinionInventory>().AddResource(type, give);
    }
}

public enum ResourceType
{
    ROCK = 0,
    WATER = 1,
    WOOD = 2,
    MEAT = 3
}
