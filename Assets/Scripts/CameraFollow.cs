using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    void Start()
    {
        vcam = this.gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    public void followPlayer2() {
        GameObject p2 = GameObject.Find("Player2");
        if (p2 != null) {
            vcam.Follow = p2.transform;
        }
    }
}
