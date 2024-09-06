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
    public GameObject player;
    public LayerMask playerLayer; 
    //public Animator ghostAnimator; 
    public AudioClip jumpScareSound;
    //public GameObject jumpScareObject;
    private AudioSource audioSource;
    public GameObject deathScreen;
    public float chaseSpeed = 5f;
    public VideoPlayer jumpScareVideoPlayer;
    public RawImage jumpScareScreen;

    private NavMeshAgent navMeshAgent;
    private bool isPlayerDetected = false;
    private bool hasJumpScared = false;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player"); 
        }

        audioSource = GetComponent<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.isStopped = true;
        jumpScareScreen.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!hasJumpScared)
        {
            DetectPlayer();

            if (isPlayerDetected)
            {
                MoveTowardsPlayer();

                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                if (distanceToPlayer <= jumpScareDistance)
                {
                    StartCoroutine(TriggerJumpScare());
                }
            }
        }
    }

    void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        isPlayerDetected = hits.Length > 0;
    }

    void MoveTowardsPlayer()
    {
        if(navMeshAgent != null && player != null)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(player.transform.position);
        }
    }

    IEnumerator TriggerJumpScare()
    {
        hasJumpScared = true;
        navMeshAgent.isStopped = true;

        jumpScareScreen.gameObject.SetActive(true);
        jumpScareVideoPlayer.Play();

        if (jumpScareSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpScareSound);
        }

        yield return new WaitForSeconds((float)jumpScareVideoPlayer.clip.length);

        jumpScareScreen.gameObject.SetActive(false);
        Destroy(gameObject);
        deathScreen.SetActive(true);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, jumpScareDistance);
    }
}
