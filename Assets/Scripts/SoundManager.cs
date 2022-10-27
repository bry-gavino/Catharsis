using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region vars
    bool isPlaying = false;
    #endregion



    // Start is called before the first frame update
    void Start(){}

    // Update is called once per frame
    void Update(){}

    public void playMusic() {
        GameObject.Find("OST_FirstStep").GetComponent<AudioSource>().Play();
        isPlaying = true;
    }
    public void stopMusic() {
        isPlaying = false;
    }
}
