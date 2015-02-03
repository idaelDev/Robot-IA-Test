using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Architect : MonoBehaviour {

	public float timeToDestroy = 5f;
	public float timeToNextRule = 1f;
	public Rules[] rules;
	public float speed;
	public Color color;
	public float rotateSpeed;
	public float distanceDelta = 0.1f;
	public float rotationDelta = 0.02f;
	public GameObject cubeObject ;
	public GameObject longObject;
	public GameObject tallObject;
	public float performance= 0;

	public int nbAction = 100;
	public int nbBlock = 100;
	private List<GameObject> blocks;
	private int actionCount;
	private int blockCount;
	private float timer = 0;
	private Vector3 movement;	
	private Vector3 positionToReach;
	private Quaternion rotationToReach;
	private GameObject objectToDrop;
	private int ruleCount = 0;
	private bool actionDone = true;
	
	// Use this for initialization
	void Awake () {
		rules = new Rules[0];
		actionCount = nbAction;
		blockCount = nbBlock;
		positionToReach = transform.position;
		rotationToReach = transform.rotation;
		gameObject.renderer.material.color = color;
		blocks = new List<GameObject>();

	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if(!actionDone)
		{
//			StartCoroutine(TranslateOvertime(transform.position, positionToReach, 0.5f));
//			moving = true;
			transform.position = Vector3.Lerp(transform.position, positionToReach, speed*Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotationToReach, rotateSpeed * Time.deltaTime);

//			if(drop)
//			{
//				GameObject o = Instantiate(objectToDrop, transform.position + transform.forward, Quaternion.identity) as GameObject;
//				o.renderer.material.color = color;
//				drop = false;
//			}
			if(Vector3.Distance(transform.position, positionToReach) <= distanceDelta && Quaternion.Angle(transform.rotation, rotationToReach) <= rotationDelta)
			{
				transform.position = positionToReach;
				transform.rotation = rotationToReach;
				IncrementRules();
				actionDone = true;
			}
		}
		else if(timer > timeToNextRule)
		{
			if(blockCount > 0)
			{
				Interpretor(rules[ruleCount]);
				actionDone = false;
				timer = 0;
			}
			if(timer >= timeToDestroy)
			{
				Destructor();
			}
		}
	}

	void Destructor()
	{
		float maxHeight = 0f;
		timer = 0;
		positionToReach = new Vector3(0f,0.5f,0f);
		rotationToReach = Quaternion.identity;
		for(int i=0; i<blocks.Count; i++)
		{
			if(blocks[i].transform.position.y > maxHeight)
				maxHeight = blocks[i].transform.position.y;
		}
		actionCount = nbAction;
		Controller.cubePool.DestroyAll();
		Controller.longPool.DestroyAll();
		Controller.tallPool.DestroyAll();
		Controller.FireEvent(maxHeight);
		blocks.Clear();
		gameObject.SetActive(false);
	}

	void Interpretor(Rules rule)
	{
		switch(rule)
		{
		case Rules.FORWARD :
			if(!Collide(transform.forward))
				positionToReach = transform.position + transform.forward;
			break;
		case Rules.BACKWARD :
			if(!Collide( -transform.forward))
				positionToReach = transform.position - transform.forward;
			break;
		case Rules.LEFT :
			if(!Collide(-transform.right))
			positionToReach = transform.position - transform.right;
			break;
		case Rules.RIGHT :
			if(!Collide( transform.right))
				positionToReach = transform.position + transform.right;
			break;
		case Rules.UP :
			if(!Collide(transform.up))
				positionToReach = transform.position + transform.up;
			break;
		case Rules.DOWN :
			if(!Collide(-transform.up))
				positionToReach = transform.position - transform.up;
			break;
		case Rules.TURNLEFT :
			rotationToReach = Quaternion.Euler(transform.rotation.eulerAngles +  transform.up*-90);
			break;
		case Rules.TURNRIGHT :
			rotationToReach = Quaternion.Euler(transform.rotation.eulerAngles + transform.up*90);
			break;
		case Rules.DROPCUBE :
			if(!Collide(transform.forward))
			{
				GameObject obj = Controller.cubePool.GetPooledObject();
				obj.transform.position = transform.position + transform.forward;
				obj.transform.rotation = Quaternion.identity;
				obj.renderer.material.color = color;
				obj.SetActive(true);
				blocks.Add(obj);
				blockCount--;
			}
			break;
		case Rules.DROPLONGRIGHT :
			if(!Collide(transform.forward,transform.right) && !Collide(transform.forward))
			{
				GameObject obj = Controller.longPool.GetPooledObject();
				obj.transform.position = transform.position + transform.forward + transform.right/2;
				obj.transform.rotation =transform.rotation;
				obj.renderer.material.color = color;
				obj.SetActive(true);
				blocks.Add(obj);
				blockCount--;
			}
			break;
		case Rules.DROPLONGLEFT :
			if(!Collide(transform.forward, - transform.right) && !Collide(transform.forward))
			{
				GameObject obj = Controller.longPool.GetPooledObject();
				obj.transform.position = transform.position + transform.forward - transform.right/2;
				obj.transform.rotation =transform.rotation;
				obj.renderer.material.color = color;
				obj.SetActive(true);
				blocks.Add(obj);
				blockCount--;
			}
			break;
		case Rules.DROPTALLUP :
			if(!Collide(transform.forward , transform.up) && !Collide(transform.forward))
			{
				GameObject obj = Controller.tallPool.GetPooledObject();
				obj.transform.position = transform.position + transform.forward + transform.up/2;
				obj.transform.rotation =transform.rotation;
				obj.renderer.material.color = color;
				obj.SetActive(true);
				blocks.Add(obj);
				blockCount--;
			}
			break;
		case Rules.DROPTALLDOWN : 
		if(!Collide(transform.forward,- transform.up)&& !Collide(transform.forward))
		{
				GameObject obj = Controller.tallPool.GetPooledObject();
				obj.transform.position = transform.position + transform.forward - transform.up/2;
				obj.transform.rotation =transform.rotation;
				obj.renderer.material.color = color;
				obj.SetActive(true);
				blocks.Add(obj);
				blockCount--;
		}
		break;
		}
		actionCount--;
	}


	bool Collide(Vector3 direction)
	{
		RaycastHit hit;
//		Debug.DrawRay(transform.position, direction,Color.red,1f);
		if(Physics.SphereCast(transform.position,0.4f, direction,out hit, 1))
		{
			return true;
		}
		return false;
	}

	bool Collide(Vector3 from, Vector3 direction)
	{
		RaycastHit hit;
		if(Physics.SphereCast(transform.position+from,0.4f ,direction,out hit, 1))
		{

			return true;
		}
		return false;
	}
	
	void IncrementRules()
	{
		ruleCount++;
		if(ruleCount == rules.Length){
			ruleCount = 0;
			if(actionCount >= nbAction)
				blockCount = 0;
			else
				actionCount = nbAction;
		}
	}

	public void RandomGeneration(int maximumGene)
	{
//		int nbGenes = Random.Range(1,maximumGene);
		rules = new Rules[maximumGene];
		for(int i=0; i<maximumGene; i++)
		{
			rules[i] = RandomRule();
		}
		color = new Color(Random.value, Random.value, Random.value);
		gameObject.renderer.material.color = color;
	}
	
	Rules RandomRule()
	{
		int rand = (int)(Random.Range(0, 12-0.01f));
		return (Rules)(rand);
	}
	
	public void CrossOver(Architect other)
	{
		Rules[] otherRules = other.rules;
		int size = rules.Length;
		int otherSize = otherRules.Length;
		int r = 0;
		Rules[] buff = new Rules[otherSize];
		Rules[] otherBuff = new Rules[size];
		int big = 0;
		if(size <= otherSize)
		{
			big = otherSize;
		}
		else
		{
			big = size;
		}
		r = Random.Range(0, big-1);
		for(int i =0; i<r; i++)
		{
			rules[i] = rules[i];
			otherRules[i] = otherRules[i];
		}
		for(int i=r; i< big; i++)
		{
			if(i<size)
			{
				otherRules[i] = rules[i];
			}
			if(i<otherSize)
			{
				rules[i] = otherRules[i];
			}
		}
		gameObject.renderer.material.color = Color.Lerp(color, other.color, 0.5f);
	}
	
	public void Mutate()
	{
		int index = Random.Range(0, rules.Length);
		Debug.Log("Mutation "+index);
		rules[index] = RandomRule();
	}
}
