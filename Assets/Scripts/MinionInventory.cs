using UnityEngine;
using System.Collections;

public class MinionInventory : MonoBehaviour
{
    public int maxCapacity = 100;

    private ResourceType currentType = ResourceType.ROCK;
    private int charge = 0;
    private bool isFull = false;

    public int AddResource(ResourceType type, int quantity)
    {
        if (isFull)
            return 0;
        if (type != currentType)
        {
            if (charge == 0)
            {
                currentType = type;
            }
            else
            {
                return 0;
                
            }
        }
        if (charge + quantity > maxCapacity)
        {
            charge = maxCapacity;
            return (maxCapacity - charge);
        }
        else
        {
            charge += quantity;
            return quantity;
        }
    }

    //Changer de type uniquement si quantity == 0;
    //Ne pas porter + de maxCapacity;
    //Prendre le maximum de charge possible
}
