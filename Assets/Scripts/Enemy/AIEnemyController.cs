using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    [Header("Modo Alerta")]
    //recibe daño
    public bool enAlerta;
    public float disBusqueda;
    public bool loVeo;

    [Header("Sistema Perseguir")]
    //ChasePlayer - persiguiendo jugador
    public float moveSpeed;
    public bool persiguiendo;
    public float minDisPerseguir = 10;
    public bool alcanceDistance;
    public float miDistanciaPlayer;

    public Transform targetPlayer;

    public String tipoJugador;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetPlayer = GameObject.FindGameObjectWithTag("Target").transform;
    }


    private void Start() 
    {
        radioActual = radioAlerta;
        enAlerta = false;
        loVeo = true;

        playerRef = GameObject.FindGameObjectWithTag("Player");
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        agente.autoBraking = false;
        alcanceDistance = false;
        StartCoroutine(FOVRoutine());

        canSeePlayer = true;
    }

    private void Update() 
    {
        persiguiendo = canSeePlayer;

        if(canSeePlayer)
        {   
            if(tipoJugador == "azul")
            {
                ChasePlayer(player);
            } else if(tipoJugador == "rojo")
            {
                ChasePlayer(targetPlayer);
            }
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

    private void ChasePlayer(Transform target)
    {
        agente.SetDestination(target.position);
        agente.speed = moveSpeed;
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
