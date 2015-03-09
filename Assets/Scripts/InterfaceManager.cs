using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour {

    public Canvas menu;
    public MouseLook mouseX;
    public MouseLook mouseY;
    public CharacterMotor control;
    private GameObject minion;
    private MinionInventory chest;
    public PlayerEventManager manager;

    public Text textChestRock;
    public Text textChestWood;
    public Text textChestMeat;
    public Text textChestWater;

    public Text textMinionRock;
    public Text textMinionWood;
    public Text textMinionMeat;
    public Text textMinionWater; 

    public Texture[] logos;


	// Use this for initialization
	void Start () {
        PlayerEventManager.OnMinionTargetEvent += ActiveMenu;
        chest = GameObject.FindGameObjectWithTag("Chest").GetComponent<MinionInventory>();
	}
	
	// Update is called once per frame
	void Update () {
        textChestMeat.text ="" + chest.GetInventoryValue(ResourceType.MEAT);
        textChestRock.text = "" + chest.GetInventoryValue(ResourceType.ROCK);
        textChestWater.text = "" + chest.GetInventoryValue(ResourceType.WATER);
        textChestWood.text = "" + chest.GetInventoryValue(ResourceType.WOOD);
	}

    void ActiveMenu(GameObject target)
    {
        MinionInventory inv = target.GetComponent<MinionInventory>();
    
        textMinionWood.text = "" + inv.GetInventoryValue(ResourceType.WOOD);
        textMinionMeat.text = "" + inv.GetInventoryValue(ResourceType.MEAT);
        textMinionWater.text = "" + inv.GetInventoryValue(ResourceType.WATER);
        minion = target;
        mouseX.enabled = false;
        mouseY.enabled = false;
        control.enabled = false;
        manager.enabled = false;
        //menu.gameObject.GetComponent<AudioSource>().Play();
        menu.enabled = true;
    }

    void DeActivateMenu()
    {
        mouseX.enabled = true;
        mouseY.enabled = true;
        control.enabled = true;
        manager.enabled = true;
        menu.enabled = false;
    }

    public void MakeBuilder()
    {
        minion.GetComponent<ImprovedMinionV2>().canvas.GetComponentInChildren<RawImage>().texture = logos[4];
        minion.GetComponent<ImprovedMinionV2>().SetToBuilder();
        DeActivateMenu();
    }

    public void MakeMiner(int type)
    {
        minion.GetComponent<ImprovedMinionV2>().canvas.GetComponentInChildren<RawImage>().texture = logos[type];
        minion.GetComponent<ImprovedMinionV2>().SetToMiner((ResourceType)type);
        DeActivateMenu();
    }
}
