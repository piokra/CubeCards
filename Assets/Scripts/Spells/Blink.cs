using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : SpellBase {

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(BlinkRoutine());
	}

	IEnumerator BlinkRoutine()
	{
		var intel = Caster.GetStat(CubeObject.Stat.Intelligence);
		
		var particles = Instantiate(EffectLibrary.Instance.SimpleParticleSystem);
		var system = particles.GetComponent<ParticleSystem>();
		
		system.startColor = Color.blue;
		var p = CastPoint;
		p.z = -0.5f;
		particles.transform.position = p;
		Destroy(particles, 1);
		yield return new WaitForSeconds(0.66f/(1+intel));

		Caster.transform.position = CastPoint;
	}
}
