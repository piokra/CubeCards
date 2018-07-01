using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : SpellBase {

	// Use this for initialization
	void Start ()
	{
		CastPoint.z = -0.5f;
		transform.position = CastPoint;
		var boxCollider = gameObject.AddComponent<BoxCollider>();
		boxCollider.size = new Vector3(0.33f, 0.33f, 0.33f);
		

		var meshFilter = gameObject.AddComponent<MeshFilter>();
		meshFilter.mesh = MeshUtil.CubeMesh(0.33f);

		var meshRender = gameObject.AddComponent<MeshRenderer>();

		var rigidbody = gameObject.AddComponent<Rigidbody>();
		rigidbody.velocity = 5f * Direction;

		StartCoroutine(EmitTrail());
	}

	private IEnumerator EmitTrail()
	{
		while (true)
		{
			var particles = Instantiate(EffectLibrary.Instance.SimpleParticleSystem);
			var system = particles.GetComponent<ParticleSystem>();
			system.startColor = Color.yellow;
			var p = transform.position;
			Destroy(particles, 1.5f);
			Debug.Log(gameObject.GetComponent<Rigidbody>().velocity);
			p.z = -0.5f;

			particles.transform.position = p;
			yield return new WaitForSeconds(0.5f);
		}
	}
	
	private void OnDestroy()
	{
		StopAllCoroutines();
	}

	public float DamageDealt()
	{
		return 10 * (1 + Caster.GetStat(CubeObject.Stat.Strength));
	}
	
	private void OnCollisionEnter(Collision other)
	{
		Debug.Log("Collision!");
		var cubeObject = other.gameObject.GetComponent<CubeObject>();

		if (cubeObject)
		{
			cubeObject.Damage(DamageDealt());
		}
		Destroy(gameObject);
	}
}
