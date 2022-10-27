using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){}

    // Update is called once per frame
    void Update(){}

    #region Collision Detection
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player"){
            // TODO: trigger GameManager to pass/loading screen
            GameObject.Find("EndPointFX").GetComponent<AudioSource>().Play();
            GameObject gameManager = GameObject.FindWithTag("GameManager");
            gameManager.GetComponent<GameManager>().transitionToLevelScreen();
        }
    }
    #endregion
}
