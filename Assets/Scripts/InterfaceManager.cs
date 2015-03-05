using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour {

    public Canvas menu;
    public MouseLook mouseX;
    public MouseLook mouseY;
    public CharacterMotor control;
    private GameObject minion;
	// Use this for initialization
	void Start () {
        PlayerEventManager.OnMinionTargetEvent += ActiveMenu;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ActiveMenu(GameObject target)
    {
        Debug.Log("Active Menu");
        minion = target;
        mouseX.enabled = false;
        mouseY.enabled = false;
        control.enabled = false;
        //menu.gameObject.GetComponent<AudioSource>().Play();
        menu.enabled = true;
    }

    void DeActivateMenu()
    {
        mouseX.enabled = true;
        mouseY.enabled = true;
        control.enabled = true;
        menu.enabled = false;
    }

    public void MakeBuilder()
    {
        minion.GetComponent<ImprovedMinionV2>().SetToBuilder();
        DeActivateMenu();
    }

    public void MakeMiner(int type)
    {
        minion.GetComponent<ImprovedMinionV2>().SetToMiner((ResourceType)type);
        DeActivateMenu();
    }
}
