using UnityEngine;
using System.Collections;

public class TimeControl : MonoBehaviour {

	public float time = 1f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Time.timeScale = time;
	}
}
