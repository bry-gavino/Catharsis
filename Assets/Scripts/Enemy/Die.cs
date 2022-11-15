using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    float timer;
    private MusicManager musicManager;
    [SerializeField] [Tooltip("Sound when you die.")]
    private AudioClip DieFX;

    // Start is called before the first frame update
    void Start()
    {
        timer = 1.8f;
        musicManager = GameObject.Find("GameManager").GetComponent<MusicManager>();
        musicManager.playClip(DieFX, 1);
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
