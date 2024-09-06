using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class JumpscareUI : MonoBehaviour
{
    public RawImage jumpScareScreen;
    public VideoPlayer jumpScareVideoPlayer;
    public AudioClip jumpScareAudioClip;

    private AudioSource audioSource;

    private void Start()
    {
        jumpScareScreen.gameObject.SetActive(false);
    }

    public void TriggerJumpScare()
    {
        StartCoroutine(JumpScareProcess());
    }

    private IEnumerator JumpScareProcess()
    {
        jumpScareScreen.gameObject.SetActive(true);
        jumpScareVideoPlayer.Play();

        if (jumpScareAudioClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpScareAudioClip);
        }

        yield return new WaitForSeconds((float)jumpScareVideoPlayer.clip.length);

        jumpScareScreen.gameObject.SetActive(false);

        InGame_UI_Manager.Instance.deathScreenUI.ShowDeathScreen();
    }
}
