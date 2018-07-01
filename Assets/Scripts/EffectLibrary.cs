using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EffectLibrary : MonoBehaviour
{

	public GameObject SimpleParticleSystem;
	public GameObject SimpleUIParticleSystem;
	public GameObject SimpleLineEffect;
	
	public static EffectLibrary Instance;
	
	
	
	// Use this for initialization
	void Start ()
	{
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
