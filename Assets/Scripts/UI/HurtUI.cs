using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtUI : MonoBehaviour
{
    float timer;
    private MusicManager musicManager;
    [SerializeField] [Tooltip("Sound when you hurt.")]
    private AudioClip HurtFX;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.65f;
        musicManager = GameObject.Find("GameManager").GetComponent<MusicManager>();
        musicManager.playClip(HurtFX, 1);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f) {
            Destroy(this.gameObject);
        }
    }
}
