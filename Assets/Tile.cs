using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour 
{
	public Color col; 
	private bool posable = true;

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.layer != LayerMask.NameToLayer("Floor"))
		{
			posable = false;
			gameObject.renderer.material.color = col ;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.layer != LayerMask.NameToLayer("Floor"))
		{	posable = true;
			gameObject.renderer.material.color = Color.white ;
		}
	}

	public bool Posable
	{
		get
		{
			return posable;
		}
	}
}
