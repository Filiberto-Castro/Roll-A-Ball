using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Player Setting")]
    public float moveSpeed = 0f;
    private Rigidbody rb;
    private float horizontal;
    private float vertical;

    [Header("UI setting")]
    public int count;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI scoreText;
    public GameObject winTextObject;
    public PickUpController pickUpController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        setCoutText();
        winTextObject.SetActive(false);
    }

    public void OnMoveInput(float horizontal, float vertical)
    {
        this.vertical = vertical;
        this.horizontal = horizontal;
    }

    void setCoutText()
    {
        countText.text = "Count: " + count.ToString();
        scoreText.text = "Score: " + pickUpController.score.ToString();
    }

    void FixedUpdate() 
    {
        Vector3 moveDirection = Vector3.forward * vertical + Vector3.right * horizontal;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void GameWin()
    {
        winTextObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("PickUp"))
        {
            Destroy(other.gameObject);
            count++;
            pickUpController.score--;
            setCoutText();
            if(pickUpController.score <= 0)
            {
                GameWin();
            }
        }
    }
}