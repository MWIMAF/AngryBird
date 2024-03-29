﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{

    public enum BirdState { Idle, Thrown, HitSomething }
    public GameObject Parrent;
    public Rigidbody2D rigidBody;
    public CircleCollider2D Collider;

    public UnityAction OnBirdDestroyed = delegate { };
    public UnityAction<Bird> OnBirdShot = delegate { };

    public BirdState State { get { return _state; } }
    void OnDestroy()
    {
        if (_state == BirdState.Thrown || _state == BirdState.HitSomething)
        {
            OnBirdDestroyed();
        }
    }

    private BirdState _state;
    private float _minVelocity = 0.05f;
    private bool _flagDestroy = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
        Collider.enabled = false;
        _state = BirdState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(_state == BirdState.Idle && rigidBody.velocity.sqrMagnitude >= _minVelocity)
        {
            _state = BirdState.Thrown;
        }

        if((_state == BirdState.Thrown || _state == BirdState.HitSomething)&& rigidBody.velocity.sqrMagnitude < _minVelocity && !_flagDestroy)
        {
            _flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _state = BirdState.HitSomething;
    }

    private IEnumerator DestroyAfter(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    public void moveTo(Vector2 target, GameObject parrent)
    {
        gameObject.transform.SetParent(parrent.transform);
        gameObject.transform.position = target;
    }

    public virtual void OnTap()
    {

    }

    public void Shoot(Vector2 velocity, float distance, float speed)
    {
        Collider.enabled = true;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.velocity = velocity * speed * distance;
        OnBirdShot(this);
    }

}
