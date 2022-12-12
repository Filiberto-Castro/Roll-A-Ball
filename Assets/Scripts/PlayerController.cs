using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float horizontal;
    private float vertical;
    public float moveSpeed = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMoveInput(float horizontal, float vertical)
    {
        this.vertical = vertical;
        this.horizontal = horizontal;
    }

    void FixedUpdate() 
    {
        Vector3 moveDirection = Vector3.forward * vertical + Vector3.right * horizontal;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("PickUp"))
        {
            Destroy(other.gameObject);
        }
    }
}
