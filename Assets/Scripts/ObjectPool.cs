using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool{

	public static ObjectPool current;
	public GameObject pooledObject;
	public int pooledAmount = 20;
	public bool willGrow = true;

	public List<GameObject> pooledObjects;

	// Use this for initialization
	public ObjectPool (GameObject pooledObject, int pooledAmount, bool willGrow) 
	{
		this.pooledObject = pooledObject;
		this.pooledAmount = pooledAmount;
		this.willGrow = willGrow;
		pooledObjects = new List<GameObject>();
		for(int i=0; i<pooledAmount; i++)
		{
			GameObject obj = MonoBehaviour.Instantiate(pooledObject) as GameObject;
			obj.SetActive(false);
			pooledObjects.Add(obj);
		}
	}

	public GameObject GetPooledObject()
	{
		for(int i=0; i<pooledAmount; i++)
		{
			if(!pooledObjects[i].activeInHierarchy)
			{
				return pooledObjects[i];
			}
		}

		if(willGrow)
		{
			GameObject obj = MonoBehaviour.Instantiate(pooledObject) as GameObject;
			obj.SetActive(false);
			pooledObjects.Add(obj);
			pooledAmount++;
			return obj;
		}

		return null;
	}

	public void DestroyAll()
	{
		for(int i=0; i<pooledAmount; i++)
		{
			pooledObjects[i].SetActive(false);
		}
	}

}
