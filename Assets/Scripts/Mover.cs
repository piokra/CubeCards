using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class Mover : MonoBehaviour {
	public float Scale = 1;
	private Rigidbody _rigidbody;
	private bool _ready;


	void Start ()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_ready = false;

		if (_rigidbody)
			_rigidbody.freezeRotation = true;
	}

	// Update is called once per frame
	void Update () {
		if (!_ready)
		{
			CustomEventManager.Instance().SubscribeMove(HandleMovement);
			CustomEventManager.Instance().SubscribeMouse(GetComponent<BoxCollider>(), HandleClick, HandleDrag, HandleDrop);
		}

		_ready = true;
	}

	void HandleMovement(Vector3 movement)
	{
		if (!_rigidbody) return;
		
		_rigidbody.AddForce(Scale*movement);

		float cos = Vector3.Dot(_rigidbody.velocity.normalized, movement);
		float arg = -(cos+1) * 10;
		float exp = Mathf.Exp(arg);
		
		_rigidbody.AddForce(Scale*10*exp*movement);


	}

	void HandleClick(RaycastHit ray)
	{
		Debug.Log("Ahh..");
		CardCreator.Instance.CreateCard("Kupka");
	}

	void HandleDrag(Vector3 worldPosition)
	{
		worldPosition.z = 0;
		transform.position = worldPosition;
	}

	void HandleDrop(Vector3 worldPosition)
	{
		Debug.Log("Uff..");
	}
}
