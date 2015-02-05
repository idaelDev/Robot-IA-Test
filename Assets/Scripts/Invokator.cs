using UnityEngine;
using System.Collections;

public class Invokator : MonoBehaviour 
{
	public GameObject bot;
	private Animator anim;
	private ParticleSystem particle;
	delegate void OnClick();

	void Awake()
	{
		anim = GetComponentInChildren<Animator>();
		particle = GetComponentInChildren<ParticleSystem>();
		PlayerEventManager.onButtonEvent += Button;
	}

	void Button()
	{
		StartCoroutine(Activate());
	}
	
	public IEnumerator Activate()
	{
		anim.SetTrigger("Pressed");
		yield return new WaitForSeconds(0.5f);
		particle.Play();
		yield return new WaitForSeconds(0.2f);
		Instantiate(bot, particle.transform.position, transform.rotation);
		yield return 0;
	}
}
