using UnityEngine;
using System.Collections;

public class CameraSwitchScript : MonoBehaviour {

    Camera cam;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
            cam.enabled = !cam.enabled;
	}
}
