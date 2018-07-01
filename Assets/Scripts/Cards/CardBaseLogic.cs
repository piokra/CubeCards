using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CardBaseLogic : MonoBehaviour
{

	public Deck Deck;
	public Type Spell;
	public string Name;
	private bool _ready;
	
	
	
	
	// Use this for initialization
	void Start ()
	{
		_ready = false;

	}

	private void Update()
	{
		if (!_ready)
		{
			CustomEventManager.Instance().SubscribeMouse(GetComponent<BoxCollider>(), HandleClick, HandleDrag, HandleDrop);
		}

		_ready = true;
	}

	void HandleClick(RaycastHit raycastHit)
	{
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<BoxCollider>().enabled = false;
		var cardArt = transform.Find("Card Art");
		var canvas = transform.Find("Canvas");

		if (Deck)
		{
			Deck.PlayCard(gameObject);
		}
		
		if (canvas)
		{
			Destroy(canvas.gameObject);
		}

		if (cardArt)
		{
			Destroy(cardArt.gameObject);
		}
	}

	void HandleDrag(Vector3 worldVector)
	{
		var pointHighlight = Instantiate(EffectLibrary.Instance.SimpleUIParticleSystem);
		
		pointHighlight.transform.position = worldVector;
		pointHighlight.transform.position += Vector3.back;
		
		Destroy(pointHighlight, 0.66f);
		
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		var pos  = ray.origin + 3*ray.direction;
		pos.z = -7;

		var plane = new Plane(Vector3.back, new Vector3(0,0,-7));

		float enter;
		plane.Raycast(ray, out enter);

		transform.position = ray.origin + enter * ray.direction;
	}

	void HandleDrop(Vector3 worldVector)
	{
		Debug.Log("Dropping...");
		Debug.Log(Spell);
		if (Spell.IsSubclassOf(typeof(SpellBase)) || Spell == typeof(SpellBase))
		{
			Debug.Log("Creating Spell...");
			var spell = new GameObject();
			var spellBase = spell.AddComponent(Spell) as SpellBase;
			spellBase.InitSpell(PlayerLogic.Instance, worldVector);
			PlayerLogic.Instance.TimeLeft += 1f;
		}
		if (Deck)
		{
			Deck.ReturnCard(Name);
		}
		Destroy(gameObject);
	}
}
