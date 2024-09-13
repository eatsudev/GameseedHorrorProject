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
using Unity.PlasticSCM.Editor.WebApi;

public class Enemy_Entity : Base_Entity
{
    public float minimumDetectRadius = 2f;
    public float maximumDetectRadius = 10f;
    [Range(0f, 360f)]
    public float defaultDetectAngle = 45f;
    public float jumpScareDistance = 5f;
    public float moveSpeed = 2f;
    public float chaseSpeed = 5f;
    public float speedModifier = 1f;


    public LayerMask playerLayer;
    public LayerMask obstructionLayer;
    public Transform[] patrolPoints;
    public float waitTimeAtPatrolPoint = 2f;
    

    //public Animator ghostAnimator; 

    //public GameObject jumpScareObject;


    private Player_Entity player;
    private Transform target;
    private AudioSource audioSource;
    private NavMeshAgent navMeshAgent;

    private float distance;
    private float currentDetectAngle;
    private bool hasJumpScared = false;
    private int currentPatrolIndex = 0;
    private bool isWaiting = false;
    private bool isStunned = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.isStopped = false;
        StartCoroutine(DetectPlayerProcess());
        StartCoroutine(WaitAndPatrolNextPoint());
    }

    void Update()
    {
        if (!hasJumpScared)
        {
            if (isStunned)
            {
                StopEnemy();
                return;
            }

            if (player)
            {
                Debug.Log("Detected");
                MoveTowardsTarget(player.transform);

                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                if (distanceToPlayer <= jumpScareDistance)
                {
                    TriggerJumpScare();
                }
            }
            else if (target)
            {
                MoveTowardsTarget(target.transform);

                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                if (distanceToTarget <= 2f)
                {
                    target = null;
                }
            }
            else if (!isWaiting && !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                StartCoroutine(WaitAndPatrolNextPoint());
            }
        }

        navMeshAgent.speed = player ? chaseSpeed * speedModifier : moveSpeed;
    }

    private IEnumerator DetectPlayerProcess()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            DetectPlayerWithFOV();
            DetectLoudness(LoudnessFrom_Microphone.instance.Loudness(), Entities_Manager.Instance.player.transform);
        }
    }

    private void DetectPlayerWithFOV()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, maximumDetectRadius, playerLayer);

        Player_Entity newest_Player = null;
        foreach (Collider hit in hits)
        {
            newest_Player = hit.gameObject.GetComponentInChildren<Player_Entity>();
            if (newest_Player)
            {
                player = newest_Player;
                
                Vector3 dir = (player.transform.position - transform.position).normalized;

                distance = Vector3.Distance(player.transform.position, transform.position);

                // Adjust the detect angle based on player's distance
                float t = Mathf.InverseLerp(minimumDetectRadius, maximumDetectRadius, distance); // Value between 0 and 1

                currentDetectAngle = Mathf.Lerp(360f, defaultDetectAngle, t); // Lerp between 360 and defaultDetectAngle

                if (Vector3.Angle(transform.forward, dir) < currentDetectAngle / 2)
                {
                    if (!Physics.Raycast(transform.position, dir, distance, obstructionLayer))
                    {
                        Debug.Log("Detected Player");
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

    /*private void DetectPlayerLoudness()
    {
        if (player == null)
        {
            Player_Entity tempPlayer = Entities_Manager.Instance.player;
            float playerLoudness = LoudnessFrom_Microphone.instance.Loudness();

            if (Vector3.Distance(transform.position, tempPlayer.transform.position) < playerLoudness)
            {
                target = new GameObject().transform;
                target.transform.position = tempPlayer.transform.position;

                Debug.Log("heard Player");
                Debug.Log(target);
            }
        }
    }*/

    public void DetectLoudness(float loudness, Transform source)
    {
        if (player == null)
        {
            if (Vector3.Distance(transform.position, source.transform.position) < loudness)
            {
                target = new GameObject().transform;
                target.transform.position = source.transform.position;

                Debug.Log("heard Sound and is Investigating");
                Debug.Log(target);
            }
        }
    }

    void MoveTowardsTarget(Transform target)
    {
        if(navMeshAgent != null && target != null)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(target.transform.position);
        }
    }

    private void TriggerJumpScare()
    {
        hasJumpScared = true;
        navMeshAgent.isStopped = true;

        InGame_UI_Manager.Instance.jumpscareUI.TriggerJumpScare();
    }

    private IEnumerator WaitAndPatrolNextPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTimeAtPatrolPoint);
        isWaiting = false;
        PatrolNextPoint();
    }

    private void PatrolNextPoint()
    {
        if (patrolPoints.Length > 0)
        {
            navMeshAgent.destination = patrolPoints[currentPatrolIndex].position;
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    #region Stun Codes

    void StopEnemy()
    {
        navMeshAgent.SetDestination(transform.position);
        navMeshAgent.isStopped = true;
    }

    public void StartStun(float stunTime)
    {
        StartCoroutine(StunProcess(stunTime));
    }
    private IEnumerator StunProcess(float stunTime)
    {
        ActivateStun();

        yield return new WaitForSeconds(stunTime);

        DeactivateStun();
    }

    public void ActivateStun()
    {
        isStunned = true;
    }

    public void DeactivateStun()
    {
        isStunned = false;
    }

    public bool IsStunned()
    {
        return isStunned;
    }
    #endregion



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, distance);

        Gizmos.color = Color.yellow;

        Vector3 viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -currentDetectAngle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, currentDetectAngle / 2);
        Gizmos.DrawLine(transform.position, transform.position + viewAngle01 * currentDetectAngle);
        Gizmos.DrawLine(transform.position, transform.position + viewAngle02 * currentDetectAngle);

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
