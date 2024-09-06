using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.AI;
using UnityEngine.Video;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Enemy_Entity : Base_Entity
{
    public float detectionRadius = 10f; 
    public float jumpScareDistance = 5f; 
    public float moveSpeed = 2f;
    public float chaseSpeed = 5f;
    public float speedModifier = 1f;


    public LayerMask playerLayer;
    

    //public Animator ghostAnimator; 

    //public GameObject jumpScareObject;


    private Player_Entity player;
    private AudioSource audioSource;
    private NavMeshAgent navMeshAgent;

    private bool hasJumpScared = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.isStopped = true;
    }

    void Update()
    {
        if (!hasJumpScared)
        {
            DetectPlayer();

            if (player)
            {
                Debug.Log("Detected");
                MoveTowardsPlayer();

                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                if (distanceToPlayer <= jumpScareDistance)
                {

                    TriggerJumpScare();
                }
            }
        }

        navMeshAgent.speed = chaseSpeed * speedModifier;
    }

    void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

        Player_Entity newest_Player = null;
        foreach (Collider hit in hits)
        {
            newest_Player = hit.gameObject.GetComponentInChildren<Player_Entity>();
            if (newest_Player)
            {
                player = newest_Player;
                return;
            }
        }

        if(newest_Player == null)
        {
            player = null;
        }
    }

    void MoveTowardsPlayer()
    {
        if(navMeshAgent != null && player != null)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(player.transform.position);
        }
    }

    private void TriggerJumpScare()
    {
        hasJumpScared = true;
        navMeshAgent.isStopped = true;

        InGame_UI_Manager.Instance.jumpscareUI.TriggerJumpScare();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, jumpScareDistance);
    }
}
