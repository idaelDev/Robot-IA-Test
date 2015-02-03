using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour {

	public float crossOverprob = 1f;
	public float mutationProb = 0.1f;
	public int individualNumber = 10;
	public int numberToKill = 3;
	public int nbRules;
	public int randomSeed = 0;
	public float timeScale = 1f;
	public static ObjectPool cubePool;
	public static ObjectPool longPool;
	public static ObjectPool tallPool;
	public GameObject[] generation;
	public GameObject architect;
	public GameObject cubeObject ;
	public GameObject longObject;
	public GameObject tallObject;
	public int generationCount = 0;
	public List<float> maxPerformance;
	public float max= 0;

	public delegate void ArchitectEnd(float performance);
	public static event ArchitectEnd architectEnd;
	private int individualCount = 0;

	private Architect[] individuals;
	// Use this for initialization
	void Start () {
		maxPerformance = new List<float>();
		Random.seed = randomSeed;
		cubePool = new ObjectPool(cubeObject, 250, true);
	 	longPool = new ObjectPool(longObject, 250, true);
		tallPool = new ObjectPool(tallObject, 250, true);
		generation = new GameObject[individualNumber];
		individuals = new Architect[individualNumber];
		for(int i=0; i<individualNumber; i++)
		{
			generation[i] = Instantiate(architect) as GameObject;
			generation[i].SetActive(false);
			individuals[i] = generation[i].GetComponent<Architect>();
			individuals[i].RandomGeneration(nbRules);
		}
		architectEnd += NotePerformance;
		TestArchitect();
	}

	public static void FireEvent(float maxHeight)
	{
		architectEnd(maxHeight);
	}

	void TestArchitect()
	{
		if(individualCount < individualNumber)
		{
			generation[individualCount].transform.position = new Vector3(0f,0.5f,0f);
			generation[individualCount].transform.rotation = Quaternion.identity;
			generation[individualCount].SetActive(true);
		}
		else
		{
			maxPerformance.Add(max);
			max = 0;
			Reproduction();
		}
	}

	void NotePerformance(float  performance)
	{
		individuals[individualCount].performance = performance;
		if(performance > max)
			max = performance;
		individualCount++;
		TestArchitect();

	}

	void Reproduction()
	{
		int[] macingPool = new int[individualCount-numberToKill];
		for(int i=0; i<macingPool.Length; i++)
		{
			float max = 0;
			int ind = 0;
			for(int j=0; j<individuals.Length; j++)
			{
				if(individuals[j].performance > max)
				{
					max = individuals[j].performance;
					ind = j;
				}
			}
			macingPool[i] = ind;
			individuals[ind].performance = 0;
		}

		for(int j=0; j<individuals.Length; j++)
		{
			if(individuals[j].performance > 0)
			{
				Debug.Log ("Random Generated : " + j);
				individuals[j].performance = 0;
				individuals[j].RandomGeneration(nbRules);
			}
		}
		
		for(int i=0; i<macingPool.Length-1; i+= 2)
		{
//			Debug.Log(i + " se reproduit avec "+ (i+1));
			if(Random.value <= crossOverprob)
			{
				individuals[macingPool[i]].CrossOver(individuals[macingPool[i+1]]);
			}
			if(Random.value <= mutationProb)
			{
				individuals[macingPool[i]].Mutate();
			}
			if(Random.value <= mutationProb)
			{
				individuals[macingPool[i+1]].Mutate();
			}
		}

		for(int i=0; i<individuals.Length; i++)
		{
			individuals[i].performance = 0;
		}

		generationCount ++;
		individualCount = 0;
		TestArchitect();
	}


}
