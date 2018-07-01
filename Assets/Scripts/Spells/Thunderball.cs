using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class Thunderball : SpellBase {

	// Use this for initialization
	void Start ()
	{
		var rigibody = gameObject.AddComponent<Rigidbody>();
		var boxCollider = gameObject.AddComponent<BoxCollider>();
		boxCollider.size = new Vector3(0.6f, 0.6f, 0.6f);
		var meshFilter = gameObject.AddComponent<MeshFilter>();
		meshFilter.mesh = MeshUtil.CubeMesh(0.6f);
		var meshRenderer = gameObject.AddComponent<MeshRenderer>();
		
		rigibody.velocity = Direction;
		var p = CastPoint;
		p.z = -1;
		gameObject.transform.position = p;

		StartCoroutine(ZapRoutine());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter(Collision other)
	{
		Destroy(gameObject);
	}

	private IEnumerator ZapRoutine()
	{
		var intel = Caster.GetStat(CubeObject.Stat.Intelligence);
		while (true)
		{
			yield return new WaitForSeconds(2/(3+intel));
			Zap();
		}
	}

	private void OnDestroy()
	{
		Zap();
		Zap();
		Zap();
		StopAllCoroutines();
	}

	private void Zap()
	{
		var str = Caster.GetStat(CubeObject.Stat.Strength);
		var boxCollider = gameObject.GetComponent<BoxCollider>();
		boxCollider.enabled = false;
		var colliders = Physics.OverlapSphere(transform.position, 4);
		var sortedColliders = colliders.OrderBy(collider1 => Random.Range(0, 1)).ToList();
		boxCollider.enabled = true;
		foreach (var sortedCollider in sortedColliders)
		{
			var parent = sortedCollider.gameObject;
			var cubeObject = parent.GetComponent<CubeObject>();
			if (cubeObject)
			{
				var line = Instantiate(EffectLibrary.Instance.SimpleLineEffect);
				var lineRenderer = line.GetComponent<LineRenderer>();
				lineRenderer.SetPositions(new Vector3[] {cubeObject.transform.position, transform.position});
				lineRenderer.startColor = Color.cyan;
				Destroy(line, 1);
				cubeObject.Damage(5*(1+str));
				
				return;
			}
		}

	}
}
