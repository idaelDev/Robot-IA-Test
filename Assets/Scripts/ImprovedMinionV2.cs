using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ImprovedMinionV2 : MonoBehaviour
{

    public float actionDist = 1.5f;
    public float delay = 2f;
    public State defaultState = State.BUILD;
    public State currentState;
    private NavMeshAgent nav;
    Pattern targetPattern = null;
    Pattern oldPattern = null;
    Resource resourceSpot;
    public ResourceType currentMining = ResourceType.ROCK;
    public int currentNeed;
    MinionInventory inventory;
    public MinionInventory chest;
    private Vector3 chestPosition;
    private float timer = 0f;
    private List<Pattern> patterns;
    public delegate void OnBlockBuilt();
    public static event OnBlockBuilt onBlockBuiltEvent;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        inventory = gameObject.GetComponent<MinionInventory>();
        chest = GameObject.FindGameObjectWithTag("Chest").GetComponent<MinionInventory>();
        chestPosition = GameObject.FindGameObjectWithTag("Chest").transform.position;
        MouseTarget.onPoseEndEvent += UpdatePatterns;
        onBlockBuiltEvent += UpdatePatterns;
        patterns = new List<Pattern>();
    }

    void Update()
    {
        if (targetPattern != null)
            Debug.DrawLine(transform.position, targetPattern.transform.position);
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
                    break;

            }
            timer = 0;
        }
    }

    void Build()
    {
        if(targetPattern == null)
        {
            Debug.Log("BUILD : Select target");
            SelectTargetPattern();
        }
        else
        {
            if(canConstruct(targetPattern))
            {
                Debug.Log("BUILD : Can Construct");
                nav.destination = targetPattern.transform.position;
                if(Vector3.Distance(transform.position, nav.destination) <= actionDist)
                {
                    Debug.Log("BUILD : Construction");
                    targetPattern.DoAction(gameObject);
                    oldPattern = targetPattern;
                    targetPattern = null;
                    onBlockBuiltEvent();
                    Debug.Log("BUILD : Construction End");
                }
            }
            else
            {
                Debug.Log("BUILD : Not enought material");
                currentMining = targetPattern.type;
                currentNeed = targetPattern.quantity;
                currentState = State.TAKE;
                Debug.Log("BUILD -> TAKE");
            }
        }
    }

    void Take()
    {
        nav.destination = chestPosition;
        if (Vector3.Distance(transform.position, nav.destination) <= actionDist)
        {
            Debug.Log("TAKE : Look in the chest");
            int c = chest.DropResource(currentMining, inventory.maxCapacity);
            chest.AddResource(currentMining, inventory.AddResource(currentMining, c));
            if (inventory.GetInventoryValue(currentMining) >= currentNeed)
            {
                Debug.Log("TAKE : Not enought material in the chest");
                targetPattern.choosen = false;
                oldPattern = targetPattern;
                currentState = defaultState;
                Debug.Log("TAKE -> "+ defaultState.ToString());
            }
            else
            {
                Debug.Log("TAKE : Full inventory");
                currentState = State.POSE;
                Debug.Log("TAKE -> POSE");
            }
        }
    }

    void Pose()
    {
        nav.destination = chest.gameObject.transform.position;
        if (Vector3.Distance(transform.position, nav.destination) <= actionDist)
        {
            Debug.Log("POSE : drop in the chest");
            chest.AddResource(ResourceType.MEAT, inventory.DropResource(ResourceType.MEAT,inventory.GetInventoryValue(ResourceType.MEAT)));
            chest.AddResource(ResourceType.ROCK, inventory.DropResource(ResourceType.ROCK, inventory.GetInventoryValue(ResourceType.ROCK)));
            chest.AddResource(ResourceType.WATER, inventory.DropResource(ResourceType.WATER, inventory.GetInventoryValue(ResourceType.WATER)));
            chest.AddResource(ResourceType.WOOD, inventory.DropResource(ResourceType.WOOD, inventory.GetInventoryValue(ResourceType.WOOD)));
            currentState = defaultState;
            Debug.Log("POSE -> "+ defaultState.ToString());
        }
    }

    void Mine()
    {
        if (resourceSpot == null || resourceSpot.type != currentMining)
        {
            Debug.Log("MINE : Looking for spot");
            resourceSpot = SelectRessourceSpot();
        }
        else
        {
            nav.destination = resourceSpot.transform.position;
            if (Vector3.Distance(transform.position, nav.destination) <= actionDist)
            {
                if (inventory.IsFull)
                {
                    Debug.Log("MINE : full inventory");
                    currentState = State.POSE;
                    Debug.Log("Minage -> Poser");
                }
                else
                {
                    Debug.Log("MINE : in progress...");
                    resourceSpot.GetComponent<Actor>().DoAction(this.gameObject);
                }
            }
        }
    }

    private Resource SelectRessourceSpot()
    {
        Resource r = null;
        GameObject[] targets = GameObject.FindGameObjectsWithTag(currentMining.ToString());
        float minDistance = 10000f;
        if (targets.Length > 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                float d = Vector3.Distance(targets[i].transform.position, transform.position);
                if (d < minDistance)
                {
                    minDistance = d;
                    r = targets[i].GetComponent<Resource>();
                }
            }
        }
        return r;
    }

    bool canConstruct(Pattern p)
    {
        return inventory.GetInventoryValue(p.type) >= p.quantity;
    }
    
    void SelectTargetPattern()
    {
        float minD = 10000f;
        Pattern constructible = null;
        Pattern near = null;
        for(int i=0; i<patterns.Count; i++)
        {
            if(!patterns[i].choosen && patterns[i] != oldPattern)
            {
                float d = Vector3.Distance(patterns[i].transform.position, transform.position);
                //if(canConstruct(patterns[i]))
                //{
                //    if(d < minD || constructible == null)
                //    {
                //        patterns[i].choosen = true;
                //        constructible = patterns[i];
                //        minD = Mathf.Min(minD, d);
                //    }
                //}
                //else
                //{
                    if(d < minD)
                    {
                        if (near != null)
                            near.choosen = false;
                        patterns[i].choosen = true;
                        near = patterns[i];
                        minD = d;
                    }
                //}
            }
        }
        //if(constructible != null)
        //{
        //    near.choosen = false;
        //    targetPattern = constructible;
        //}
        //else
        //{
            targetPattern = near;
            Debug.Log(targetPattern);
    }

    void UpdatePatterns()
    {
        Debug.Log("Update Pattern");
        GameObject[] buf = GameObject.FindGameObjectsWithTag("Pattern");
        patterns.Clear();
        for(int i=0; i<buf.Length; i++)
        {
            patterns.Add(buf[i].GetComponent<Pattern>());
        }
        Debug.Log("Update End : " + patterns.Count);
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
