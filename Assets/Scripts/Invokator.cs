using UnityEngine;
using System.Collections;

public class Invokator : MonoBehaviour 
{
    public int rockNeed = 10;
    public int meatNeed = 10;
    public int woodNeed = 10;
    public int waterNeed = 10;

	public GameObject bot;
	private Animator anim;
	private ParticleSystem particle;
	delegate void OnClick();
    MinionInventory chest;

	void Awake()
	{
		anim = GetComponentInChildren<Animator>();
		particle = GetComponentInChildren<ParticleSystem>();
		PlayerEventManager.onButtonEvent += Button;
        chest = GameObject.FindGameObjectWithTag("Chest").GetComponent<MinionInventory>();
	}

	void Button()
	{
		StartCoroutine(Activate());
	}
	
	public IEnumerator Activate()
	{
		anim.SetTrigger("Pressed");
		yield return new WaitForSeconds(0.5f);
        if (canConstruct())
        {
            particle.Play();
            yield return new WaitForSeconds(0.2f);
            builMinion();
            yield return 0;
        }
	}

    void builMinion()
    {
        chest.DropResource(ResourceType.MEAT, meatNeed);
        chest.DropResource(ResourceType.ROCK, rockNeed);
        chest.DropResource(ResourceType.WOOD, woodNeed);
        chest.DropResource(ResourceType.WATER, waterNeed);
        Instantiate(bot, particle.transform.position, transform.rotation);
    }

    bool canConstruct()
    {
        if (chest.GetInventoryValue(ResourceType.MEAT) < meatNeed)
            return false;
        if (chest.GetInventoryValue(ResourceType.ROCK) < rockNeed)
            return false;
        if (chest.GetInventoryValue(ResourceType.WOOD) < woodNeed)
            return false;
        if (chest.GetInventoryValue(ResourceType.WATER) < waterNeed)
            return false;
        return true;
    }

}
