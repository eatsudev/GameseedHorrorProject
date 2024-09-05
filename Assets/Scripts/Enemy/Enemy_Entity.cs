using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Enemy_Entity : Base_Entity
{
    public float detectionRadius = 10f; 
    public float jumpScareDistance = 5f; 
    public float moveSpeed = 2f;
    public GameObject player;
    public LayerMask playerLayer; 
    //public Animator ghostAnimator; 
    public AudioClip jumpScareSound;
    public GameObject jumpScareObject;
    private AudioSource audioSource;
    public GameObject deathScreen;

    private bool isPlayerDetected = false;
    private bool hasJumpScared = false;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player"); 
        }

        audioSource = GetComponent<AudioSource>();
        jumpScareObject.SetActive(false);
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
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    IEnumerator TriggerJumpScare()
    {
        hasJumpScared = true;
        CinemachineVirtualCamera playerCamera = player.GetComponentInChildren<CinemachineVirtualCamera>();
        Vector3 scarePosition = playerCamera.transform.position + playerCamera.transform.forward * 3.5f;
        jumpScareObject.transform.position = scarePosition;

        jumpScareObject.transform.rotation = Quaternion.LookRotation(playerCamera.transform.forward);

        jumpScareObject.SetActive(true);

        if (jumpScareSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpScareSound);
        }
        yield return new WaitForSeconds(2f); 
        jumpScareObject.SetActive(false);
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
