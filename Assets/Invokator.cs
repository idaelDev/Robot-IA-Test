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
	}
	
	public IEnumerator Activate()
	{
		anim.SetTrigger("Pressed");
//		WaitForSeconds(0.5f);
		particle.Play();
//		WaitForSeconds(0.2f);
		Instantiate(bot, transform.position, transform.rotation);
		yield return 0;
	}
}
