using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIEnemyController : MonoBehaviour
{

    //--------- Raycast ---------------
    [Header("Raycast")]
    public float radioActual;
    public float radioPatrullaje;
    public float radioAlerta;
    [Range(0, 360)]
    public float angulo;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstruccionMask;

    public bool canSeePlayer;

    //--------- Enemy Controller -----------
    [Header("Controlador de enemigo")]
    public NavMeshAgent agente;
    public Transform player;
    public Animator animator;

    //patrullando

    [Header("Sistema Patrullaje")]
    public List<Transform> refPuntos = new List<Transform>();
    public bool alcancePunto;
    public float moveSpeed = 4;

    [Header("Modo Alerta")]
    //recibe daño
    public bool enAlerta;
    public float disBusqueda;
    public bool loVeo;
    [SerializeField]
    private GameObject[] puntosDeCobertura;
    [SerializeField]
    private GameObject puntoDeCoberturaMasCercano;

    public bool aCubierto;

    //Atacando
    //EnemyFire Shooting;

    [Header("Sistema Ataque")]
    public GameObject proyectil;
    public Transform puntoProyectil;


    public bool alreadyAttacked;
    public float tiempoDisparo = 0.5f;

    [Header("Sistema Perseguir")]
    //ChasePlayer - persiguiendo jugador
    public float runSpeed = 6;
    public bool persiguiendo;
    public float minDisPerseguir = 10;
    public bool alcanceDistance;
    public float miDistanciaPlayer;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    private void Start() 
    {
        radioActual = radioAlerta;
        enAlerta = false;
        loVeo = false; //! cambio

        playerRef = GameObject.FindGameObjectWithTag("Player");
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        agente.autoBraking = false;

        alcanceDistance = false;

        StartCoroutine(FOVRoutine());
    }

    private void Update() 
    {
        persiguiendo = canSeePlayer;

        if(canSeePlayer)
        {   
            //Perseguir
            agente.SetDestination(player.position);
            //ChasePlayer();
        }

    }


    private IEnumerator FOVRoutine()
    {

        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while(true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangoChecks = Physics.OverlapSphere(transform.position, radioActual, targetMask);

        if(rangoChecks.Length != 0)
        {
            Transform target = rangoChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, directionToTarget) < angulo / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstruccionMask))
                {
                    loVeo = true;
                    canSeePlayer = true;
                    float distance = Vector3.Distance(agente.transform.position, player.position);
                    miDistanciaPlayer = distance;
                    if(distance < minDisPerseguir)
                    {
                        //canSeePlayer = false;
                    }
                    else
                    {
                        canSeePlayer = true;
                    }
                }
                else
                {
                    loVeo = false;
                    //canSeePlayer = false;
                }
                //attackPlayer();
            }
            else{
                //canSeePlayer = false;
            }
        }
        else if(canSeePlayer)
        {
            //Patrullando();
            //canSeePlayer = false;
        }
    }

    private void ChasePlayer()
    {
        agente.SetDestination(player.position);
        agente.speed = runSpeed;
        //animator.SetBool("isAtacando", false);
        //animator.SetBool("isPersiguiendo", true);
        float distance = Vector3.Distance(agente.transform.position, player.position);
        miDistanciaPlayer = distance;
        if(distance < minDisPerseguir)
        {
           alcanceDistance = true;
           //canSeePlayer = false;
           //attackPlayer();
           
        }
        else
        {
            alcanceDistance = false;
            canSeePlayer = true;
        }

        
    }


}
