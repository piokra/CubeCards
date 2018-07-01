using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

	public GameObject What;

	public float SpringStrength = 2f;
	public float AirResistance = 0.5f;
	public float ConstantFriction = 0.5f;
	public float ConstantForce = 0;
	
	private Rigidbody _rigidbody;

	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (_rigidbody && What)
		{
			Vector3 force = What.transform.position - transform.position;
			force.z = 0;
			_rigidbody.AddForce(SpringStrength*force+ConstantForce*force.normalized);
			if (_rigidbody.velocity.sqrMagnitude > 0.01f)
				_rigidbody.AddForce(-AirResistance*_rigidbody.velocity - ConstantFriction*_rigidbody.velocity.normalized);
		}
	}
}
