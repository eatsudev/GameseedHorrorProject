using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : Base_Holdable_Items
{
    public AudioClip pigSounds;
    public float pigLoudness = 15f;

    private AudioSource audioSource;
    private Player_Entity player;

    private bool isScared = false;
    private bool isPickedUp = false;
    public override void Start()
    {
        base.Start();
        player = Entities_Manager.Instance.player;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isScared && !isPickedUp)
        {
            CheckPlayer();
        }
        else
        {
            Entities_Manager.Instance.enemy.DetectLoudness(pigLoudness, transform);
        }
    }

    private void CheckPlayer()
    {
        if(Vector3.Distance(player.transform.position, transform.position) < 5f )
        {
            StartCoroutine(PigScaredProcess());
        }
    }

    IEnumerator PigScaredProcess()
    {
        audioSource.clip = pigSounds;
        audioSource.Play();
        isScared = true;

        yield return new WaitForSeconds(5f);

        audioSource.Stop();
        isScared = false;
    }

    public override void Interact()
    {
        base.Interact();

        StopAllCoroutines();

        audioSource.Stop();
        isScared = false;
        isPickedUp = true;
    }

    public override void ActivateRigidBody()
    {
        base.ActivateRigidBody();

        isPickedUp = false;
    }
}
