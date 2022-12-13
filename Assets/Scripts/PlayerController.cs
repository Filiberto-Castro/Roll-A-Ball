using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Player Setting")]
    public float moveSpeed = 0f;
    public float rotationSpeed = 280.0f;
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
        // Asignando direccion al jugador
        Vector3 moveDirection = Vector3.forward * vertical + Vector3.right * horizontal;

        // Asigando rotacion del jugador
        Vector3 projectedCameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);
        Quaternion rotationToCamera = Quaternion.LookRotation(projectedCameraForward, Vector3.up);

        // Aplicando rotacion a la dirección
        moveDirection = rotationToCamera * moveDirection;

        // Rotando al jugador
        Quaternion rotationToMoveDirection = Quaternion.LookRotation(moveDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToMoveDirection, rotationSpeed * Time.deltaTime);
        
        // Moviendo al jugador
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void GameWin()
    {
        winTextObject.SetActive(true);
    }

    // Detenctando collisiones
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