using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using NUnit.Framework.Constraints;
using UnityEditor.PackageManager;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomEventManager : MonoBehaviour
{
    public delegate void HotkeyCallback(KeyCode keyCode, Vector3 mousePosition);

    public delegate void MoveCallback(Vector3 displacementVector);

    public delegate void ClickCallback(RaycastHit raycastHit);

    public delegate void DragCallback(Vector3 mousePosition);

    public delegate void DropCallback(Vector3 mousePosition);

    struct MouseCallbacks
    {
        internal ClickCallback Click;
        internal DragCallback Drag;
        internal DropCallback Drop;
    }

    private Dictionary<KeyCode, List<HotkeyCallback>> _callbacks = new Dictionary<KeyCode, List<HotkeyCallback>>();
    private Dictionary<KeyCode, bool> _keyStatus = new Dictionary<KeyCode, bool>();
    private List<MoveCallback> _moveCallbacks = new List<MoveCallback>();
    private Dictionary<Collider, MouseCallbacks> _mouseCallbacks = new Dictionary<Collider, MouseCallbacks>();
    private Collider _activeCollider = null;
    private LayerMask _colliderLayer;

    // Use this for initialization
    void Start()
    {
        _colliderLayer = LayerMask.NameToLayer("ClickableLayer");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var key in _callbacks.Keys)
        {
            var pressed = Input.GetKey(key);

            if (pressed || !_keyStatus[key]) continue;
            var callbacks = _callbacks[key];

            foreach (var callback in callbacks)
            {
                callback(key, Input.mousePosition);
            }
        }

        foreach (var key in _callbacks.Keys)
        {
            _keyStatus[key] = Input.GetKey(key);
        }

        if (_activeCollider != null)
        {
            MouseCallbacks callbacks;
            if (_mouseCallbacks.TryGetValue(_activeCollider, out callbacks))
            {
                RaycastHit hit = new RaycastHit(); // zzz c#
                bool calculated = false;
                if (callbacks.Drag != null)
                {
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Physics.Raycast(ray, out hit, Mathf.Infinity, _colliderLayer);
                    calculated = true;
                    callbacks.Drag(hit.point);
                }


                if (callbacks.Drop != null && Input.GetMouseButtonUp(0))
                {
                    if (!calculated)
                    {
                        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        Physics.Raycast(ray, out hit, Mathf.Infinity, _colliderLayer);
                    }
                    
                    callbacks.Drop(hit.point);
                }

                if (!Input.GetMouseButton(0))
                    _activeCollider = null;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Mathf.Infinity, _colliderLayer);
            _activeCollider = hit.collider;
            MouseCallbacks callbacks;
            if (_activeCollider && _mouseCallbacks.TryGetValue(_activeCollider, out callbacks) && callbacks.Click != null)
            {
                callbacks.Click(hit);
            }
        }

        Vector3 movement = new Vector3();
        if (Input.GetKey(KeyCode.A))
        {
            movement.x -= 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movement.x += 1;
        }

        if (Input.GetKey(KeyCode.W))
        {
            movement.y += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            movement.y -= 1;
        }

        if (movement.sqrMagnitude > 0)
        {
            var rotation = Camera.main.transform.rotation;
            var callbackMovement = rotation * movement.normalized;
            foreach (var callback in _moveCallbacks)
            {
                
                callback(callbackMovement);
            }
        }
    }

    public void SubscribeHotkey(KeyCode key, HotkeyCallback callback)
    {
        _keyStatus[key] = false;
        if (!_callbacks.ContainsKey(key))
        {
            _callbacks[key] = new List<HotkeyCallback>();
        }

        List<HotkeyCallback> callbacks;

        if (_callbacks.TryGetValue(key, out callbacks))
        {
            callbacks.Add(callback);
        }
    }

    public void SubscribeMouse(Collider collider, ClickCallback click, DragCallback drag, DropCallback drop)
    {
        Debug.Log(collider);
        _mouseCallbacks[collider] = new MouseCallbacks
        {
            Click = click,
            Drag = drag,
            Drop = drop,
        };
    }

    public void SubscribeMove(MoveCallback moveCallback)
    {
        _moveCallbacks.Add(moveCallback);
    }

    static CustomEventManager _customEventManager;

    public static CustomEventManager Instance()
    {
        if (_customEventManager)
            return _customEventManager;

        _customEventManager =
            GameObject.Find("EventManager").GetComponent(typeof(CustomEventManager)) as CustomEventManager;
        return _customEventManager;
    }
}