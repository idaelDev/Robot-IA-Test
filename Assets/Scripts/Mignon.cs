using UnityEngine;
using System.Collections;

public class Mignon : MonoBehaviour {

	public GameObject obj;
	private NavMeshAgent nav;
	private bool occupied;
	private GameObject target;

	void Awake()
	{
		occupied = false;
		nav = gameObject.GetComponent<NavMeshAgent>();
		FindNextTarget();
	}

	void Update()
	{
		if(!occupied)
		{
			FindNextTarget();
		}
		else if(Vector3.Distance(transform.position, target.transform.position) <= 1.5)
		{

			Instantiate(obj, target.transform.position, target.transform.rotation);
			Destroy(target);
			occupied = false;
		}
	}

	void FindNextTarget()
	{
		GameObject[] targets = GameObject.FindGameObjectsWithTag("Pattern");
		float minDistance = 10000f;
		if(targets.Length > 0)
		{
			for(int i=0; i<targets.Length; i++)
			{
				float d = Vector3.Distance(targets[i].transform.position, transform.position);
				if(d < minDistance)
				{
					minDistance = d;
					target = targets[i];
				}
			}
			nav.destination = target.transform.position;
			occupied = true;
		}
	}

}
