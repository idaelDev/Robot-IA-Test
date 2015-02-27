using UnityEngine;
using System.Collections;

public class ImprovedMinion : MonoBehaviour {

    public float actionDist = 1.5f;
    public State defaultState = State.WAIT;
    public State currentState;
    private NavMeshAgent nav;
    GameObject target;
    ResourceType currentMining = ResourceType.ROCK;
    int currentNeed;
    MinionInventory inventory;
    MinionInventory chest;

    void Start()
    {
        inventory = gameObject.GetComponent<MinionInventory>();
        chest = GameObject.FindGameObjectWithTag("Chest").GetComponent<MinionInventory>();
    }

    void Wait()
    {
        target = FindNearest("Pattern");
        if(target != null)
        {
            currentState = State.BUILD;
        }
    }

    void Build()
    {
        Pattern p = target.GetComponent<Pattern>();
        if (inventory.GetInventoryValue(p.type.ToString()) < p.quantity)
        {
            currentMining = p.type;
            currentState = State.TAKE;
        }
        else
        {
            p.choosen = true;
            nav.destination = target.transform.position;
            if (nav.remainingDistance <= actionDist)
            {
                p.DoAction(this.gameObject);
                currentState = defaultState;
            }
        }
    }

    void Take()
    {
        nav.destination = chest.gameObject.transform.position;
        if(nav.remainingDistance <= actionDist)
        {
            inventory.AddResource(currentMining, currentNeed);
        }
    }

    private GameObject FindNearest(string tag)
    {
        GameObject r = null;
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
        float minDistance = 10000f;
        if (targets.Length > 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                float d = Vector3.Distance(targets[i].transform.position, transform.position);
                if (d < minDistance)
                {
                    if (!targets[i].GetComponent<Pattern>().choosen)
                    {
                        minDistance = d;
                        r = targets[i];
                    }
                }
            }
        }
        return r;
    }
}

public enum State
{
    BUILD,
    MINE,
    POSE,
    TAKE,
    WAIT
}
