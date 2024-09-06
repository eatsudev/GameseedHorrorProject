using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class JumpscareUI : MonoBehaviour
{
    public GameObject jumpscare;
    public VideoPlayer jumpScareVideoPlayer;
    public AudioClip jumpScareAudioClip;

    private AudioSource audioSource;

    private void Start()
    {
        jumpscare.SetActive(false);
    }

    public void TriggerJumpScare()
    {
        StartCoroutine(JumpScareProcess());
    }

    private IEnumerator JumpScareProcess()
    {
        jumpscare.SetActive(true);
        jumpScareVideoPlayer.Play();

        if (jumpScareAudioClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpScareAudioClip);
        }

        yield return new WaitForSeconds((float)jumpScareVideoPlayer.clip.length);

        jumpscare.SetActive(false);

        InGame_UI_Manager.Instance.deathScreenUI.ShowDeathScreen();
    }
}
