using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImprovedMinion : MonoBehaviour {

    public float actionDist = 1.5f;
    public float delay = 2f;
    public State defaultState = State.WAIT;
    public State currentState;
    private NavMeshAgent nav;
    GameObject target;
    GameObject resourceSpot;
    ResourceType currentMining = ResourceType.ROCK;
    public int currentNeed;
    MinionInventory inventory;
    public MinionInventory chest;
    private Vector3 chestPosition;
    private float timer = 0f;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        inventory = gameObject.GetComponent<MinionInventory>();
        chest = GameObject.FindGameObjectWithTag("Chest").GetComponent<MinionInventory>();
        chestPosition = GameObject.FindGameObjectWithTag("Chest").transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > delay)
        {
            switch (currentState)
            {
                case State.BUILD:
                    Build();
                    break;
                case State.MINE:
                    Mine();
                    break;
                case State.POSE:
                    Pose();
                    break;
                case State.TAKE:
                    Take();
                    break;
                case State.WAIT:
                    Wait();
                    break;
            
            }
            timer = 0;
        }
    }

    void Wait()
    {
        target = FindNearest("Pattern");
        if (target != null)
        {
            Debug.Log("Attente : Objectif definit");
            currentState = State.BUILD;
            Debug.Log("Attente -> Construction");
        }
    }

    void Build()
    {
        Pattern p = target.GetComponent<Pattern>();
        p.choosen = true;
        if (inventory.GetInventoryValue(p.type) < p.quantity)
        {
            Debug.Log("Construction : Pas assez de ressource");
            currentMining = p.type;
            currentNeed = p.quantity;
            currentState = State.TAKE;
            Debug.Log("Construction -> Prendre");
        }
        else
        {
            Debug.Log("Construction : Deplacement");
            nav.destination = target.transform.position;
            if (Vector3.Distance(transform.position, nav.destination) <= actionDist)
            {
                Debug.Log("Construction : Construction");
                p.DoAction(this.gameObject);
                currentState = defaultState;
                Debug.Log("Construction -> Attente");
            }
        }
    }

    void Take()
    {
        nav.destination = chestPosition;
        if(Vector3.Distance(transform.position, nav.destination) <= actionDist)
        {
            Debug.Log("Prendre : Recherche dans le coffre");
            int c = chest.DropResource(currentMining, currentNeed);
            inventory.AddResource(currentMining, c);
            if(inventory.GetInventoryValue(currentMining) < currentNeed)
            {
                Debug.Log("Prendre : Pas assez de ressources dans le coffre");
                currentState = State.MINE;
                Debug.Log("Prendre -> Minage");
            }
            if(inventory.IsFull)
            {
                Debug.Log("Prendre : Inventaire plein");
                currentState = State.POSE;
                Debug.Log("Prendre -> Poser");
            }
        }   
    }

    void Pose()
    {
        nav.destination = chest.gameObject.transform.position;
        if (Vector3.Distance(transform.position, nav.destination) <= actionDist)
        {
            Debug.Log("Pose : Depot dans le coffre");
            chest.AddResource(ResourceType.MEAT, inventory.GetInventoryValue(ResourceType.MEAT));
            chest.AddResource(ResourceType.ROCK, inventory.GetInventoryValue(ResourceType.ROCK));
            chest.AddResource(ResourceType.WATER, inventory.GetInventoryValue(ResourceType.WATER));
            chest.AddResource(ResourceType.WOOD, inventory.GetInventoryValue(ResourceType.WOOD));
            currentState = defaultState;
            Debug.Log("Poser -> Attente");
        } 
    }

    void Mine()
    {
        if (resourceSpot == null)
        {
            Debug.Log("Minage : recherche de ressources");
            resourceSpot = FindNearest(currentMining.ToString());
        }
        else 
        {
            resourceSpot.GetComponent<Actor>().choosen = false;
            nav.destination = resourceSpot.transform.position;
            if (Vector3.Distance(transform.position, nav.destination) <= actionDist)
            {
                if (inventory.GetInventoryValue(currentMining) < currentNeed)
                {
                    Debug.Log("Minage : En cours...");
                    resourceSpot.GetComponent<Actor>().DoAction(this.gameObject);
                }
                else
                {
                    Debug.Log("Minage : Objectif atteint, reprise de construction");
                    resourceSpot = null;
                    currentState = State.BUILD;
                    Debug.Log("Minage -> Construction");
                }
                if(inventory.IsFull)
                {
                    Debug.Log("Minage : inventaire plein");
                    resourceSpot = null;
                    currentState = State.POSE;
                    Debug.Log("Minage -> Poser");
                }
            }
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
                    if (!targets[i].GetComponent<Actor>().choosen)
                    {
                        targets[i].GetComponent<Actor>().choosen = true;
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
