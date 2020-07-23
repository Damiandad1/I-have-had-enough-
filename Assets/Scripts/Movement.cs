using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody2D _myRb;
    private Vector2 moveVelocity;

    private void Awake()
    {
        _myRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveVelocity = moveInput.normalized * _speed;
    }

    private void FixedUpdate()
    {
        _myRb.MovePosition(_myRb.position + moveVelocity * Time.fixedDeltaTime);
    }
}
