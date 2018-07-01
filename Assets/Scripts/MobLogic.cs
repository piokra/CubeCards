using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobLogic : CubeObject
{
	private void Start()
	{
		SetStat(Stat.Health, 20);
		StartCoroutine(Fire());
	}

	private IEnumerator Fire()
	{
		while (true)
		{
			var fireballObject = new GameObject();
			var fireball = fireballObject.AddComponent<Fireball>();
			fireball.InitSpell(this, transform.position + (PlayerLogic.Instance.transform.position - transform.position).normalized);
			yield return new WaitForSeconds(5);
		}
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
		
	}

	private bool Cooldown = false;
	
	private void OnCollisionEnter(Collision other)
	{
		var player = other.gameObject.GetComponent<PlayerLogic>();
		Debug.Log(other.gameObject);
		if (player && !Cooldown)
		{
			player.Damage(50);
			Cooldown = true;
			StartCoroutine(Cooloff());
			other.gameObject.GetComponent<Rigidbody>().AddForce(50*GetComponent<Rigidbody>().velocity.normalized);

		}
	}

	private IEnumerator Cooloff()
	{
		yield return new WaitForSeconds(0.5f);
		Cooldown = false;
	}
}
