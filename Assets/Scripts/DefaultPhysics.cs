using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPhysics : MonoBehaviour
{

	private Rigidbody _rigidbody;
	
	// Use this for initialization
	void Start ()
	{
		_rigidbody = GetComponent<Rigidbody>();
		if (_rigidbody)
		{
			_rigidbody.freezeRotation = true;
		}
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (_rigidbody)
		{
			_rigidbody.AddForce(0,0,1);
			_rigidbody.AddForce(-0.5f*_rigidbody.velocity);
		}

	}
}
