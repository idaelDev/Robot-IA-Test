using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour {

    public Button button;
    public Canvas menu;

    private GameObject target;
	// Use this for initialization
	void Start () {
        PlayerEventManager.OnMinionTargetEvent += ActiveMenu;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ActiveMenu(GameObject target)
    {
        menu.enabled = true;
    }
}
