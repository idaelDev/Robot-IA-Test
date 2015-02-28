using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinionInventory : MonoBehaviour
{
    public int maxCapacity = 100;

    private int charge = 0;
    private bool isFull = false;
    private Dictionary<string, int> inventory;

    void Awake()
    {
        inventory = new Dictionary<string, int>();
    }

    public int GetInventoryValue(ResourceType type)
    {
        int buf = 0;
        inventory.TryGetValue(type.ToString(), out buf);
        return buf;
    }

    public int AddResource(ResourceType type, int nb)
    {
        if (isFull)
            return 0;

        int taken = nb;
        
        if(charge + nb > maxCapacity)
        {
            taken = maxCapacity - charge;
            isFull = true;
        }
        InventoryOperation(type.ToString(), taken);
        return taken;
    }

    public int DropResource(ResourceType type, int nb)
    {
        int buf = nb;
        if (charge - nb < 0)
            buf = charge;
        InventoryOperation(type.ToString(), -nb);
        return buf;
    }

    private void UpdateCapacity()
    {
        charge = 0;
        foreach(int k in inventory.Values)
        {
            charge += k;
        }
        isFull = (charge >= maxCapacity);
    }

    private void InventoryOperation(string key, int toAdd)
    {
        int buf = 0;
        inventory.TryGetValue(key,out buf);
        int sum = Mathf.Max(buf + toAdd, 0);
        inventory.Remove(key);
        inventory.Add(key, sum);
        UpdateCapacity();
    }

    public bool IsFull
    {
        get
        {
            UpdateCapacity();
            return (charge >= maxCapacity);
        }
    }
}
