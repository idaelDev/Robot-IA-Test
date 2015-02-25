using UnityEngine;
using System.Collections;

public class Resource : MonoBehaviour
{

    public ResourceType type = ResourceType.ROCK;
    public int initialQuantity = 100;
    public float timeToMine = 1f;

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
}

public enum ResourceType
{
    ROCK = 0,
    WATER = 1,
    WOOD = 2,
    MEAT = 3
}
