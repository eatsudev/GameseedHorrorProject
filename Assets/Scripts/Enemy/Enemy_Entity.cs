using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.AI;
using UnityEngine.Video;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEditor.TerrainTools;
using UnityEditor;
using UnityEngine.Rendering.PostProcessing;

public class Enemy_Entity : Base_Entity
{
    public float detectRadius = 10f;
    [Range(0f, 360f)]
    public float detectAngle = 45f;
    public float jumpScareDistance = 5f; 
    public float moveSpeed = 2f;
    public float chaseSpeed = 5f;
    public float speedModifier = 1f;


    public LayerMask playerLayer;
    public LayerMask obstructionLayer;
    

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
        StartCoroutine(DetectPlayerProcess());
    }

    void Update()
    {
        if (!hasJumpScared)
        {
            //DetectPlayer();

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
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius, playerLayer);

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

    private IEnumerator DetectPlayerProcess()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            DetectPlayerWithFOV();
        }
    }

    private void DetectPlayerWithFOV()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius, playerLayer);

        Player_Entity newest_Player = null;
        foreach (Collider hit in hits)
        {
            newest_Player = hit.gameObject.GetComponentInChildren<Player_Entity>();
            if (newest_Player)
            {
                player = newest_Player;
                Debug.Log("Detected Player");
                Vector3 dir = (player.transform.position - transform.position).normalized;

                if(Vector3.Angle(transform.forward, dir) < detectAngle / 2)
                {
                    float distance = Vector3.Distance(player.transform.position, transform.position);

                    if(!Physics.Raycast(transform.position, dir, distance, obstructionLayer))
                    {
                        return;
                    }
                    else
                    {
                        player = null;
                    }
                    
                }
                else
                {
                    player = null;
                }
            }
        }

        if (newest_Player == null)
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
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        Gizmos.color = Color.yellow;

        Vector3 viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -detectAngle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, detectAngle / 2);
        Gizmos.DrawLine(transform.position, transform.position + viewAngle01 * detectRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngle02 * detectRadius);

        if (player)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }


    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
