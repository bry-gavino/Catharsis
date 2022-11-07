using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour {
    #region player sounds
    [SerializeField] [Tooltip("Sound when you touch this.")]
    private AudioClip EndPointFX;

    private MusicManager musicManager;
    #endregion

    void Awake() {
        musicManager = GameObject.Find("GameManager").GetComponent<MusicManager>();
    }

    #region Collision Detection
    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            // TODO: trigger GameManager to pass/loading screen
            musicManager.playClip(EndPointFX, 2);
            GameObject gameManager = GameObject.FindWithTag("GameManager");
            gameManager.GetComponent<GameManager>().transitionToLevelScreen();
        }
    }
    #endregion
}