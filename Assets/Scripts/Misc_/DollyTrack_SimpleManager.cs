using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyTrack_SimpleManager : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public float spinSpeed;

    void FixedUpdate()
    {
        AddPathPos();
    }

    private void AddPathPos()
    {
        cam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition += spinSpeed * Time.deltaTime;
    }
}
