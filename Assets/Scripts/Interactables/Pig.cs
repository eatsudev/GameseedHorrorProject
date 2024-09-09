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
    void Start()
    {
        player = Entities_Manager.Instance.player;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isScared)
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
        if(Vector3.Distance(player.transform.position, transform.position) < 5f)
        {
            StartCoroutine(PigScaredProcess());
        }
    }

    private void PigScared()
    {
        audioSource.clip = pigSounds;
        audioSource.Play();
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
}
