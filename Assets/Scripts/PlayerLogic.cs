using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerLogic : CubeObject
{
    public static PlayerLogic Instance;
    
    public float TimeLeft = 0;

    private bool _ready = false;

    private Rigidbody _rigidbody;
    private bool _canMove = true;

    // Use this for initialization
    void Start()
    {
        SetStat(Stat.MaxHealth, 100);
        SetStat(Stat.Health, 100);
        Instance = this;
        TimeLeft = 0;
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (GetComponent<Rigidbody>().velocity.sqrMagnitude > 0.01)
            TimeLeft = Mathf.Max(TimeLeft, 1);
        
        TimeLeft -= Time.deltaTime;
        if (TimeLeft <= Mathf.Epsilon)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
        
        if (!_ready)
        {
            CustomEventManager.Instance().SubscribeMove(HandleMovement);
        }

        
        _ready = true;
        base.Update();
    }

    void HandleMovement(Vector3 movement)
    {
        if (!_rigidbody || !CanMove()) return;
        TimeLeft = Mathf.Max(1, TimeLeft);
        var scale = GetStat(Stat.Agillity) + 1;
        _rigidbody.AddForce(scale * 5 * movement);

        var cos = Vector3.Dot(_rigidbody.velocity.normalized, movement);
        var arg = -(cos + 1) * 10;
        var exp = Mathf.Exp(arg);

        
        
        _rigidbody.AddForce(scale * 10 * exp * movement);
    }

    bool CanMove()
    {
        return _canMove;
    }

    public void PreventMovement(float time)
    {
        StartCoroutine(PrevenentMovementImpl(time));
    }
    
    IEnumerator PrevenentMovementImpl(float time)
    {
        _canMove = false;
        yield return new WaitForSeconds(time);
        _canMove = true;
    }

    public override void OnDeath()
    {
        Debug.Log("Game over!");
        base.OnDeath();
        Application.Quit();
    }
}