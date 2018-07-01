using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class Heal : SpellBase {

	// Use this for initialization
	void Start ()
	{
		Debug.Log("Heal logic");
		StartCoroutine(HealUp());
	}

	IEnumerator HealUp()
	{
		Debug.Log("Started to heal up");
		var intel = Caster.GetStat(CubeObject.Stat.Intelligence);
		var strength = Caster.GetStat(CubeObject.Stat.Strength);
		yield return new WaitForSeconds(1/(1+intel));
		Debug.Log(Caster);
		Caster.Heal(20+10*strength);
		Destroy(gameObject);
	}
}
