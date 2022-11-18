using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtBox : MonoBehaviour
{

    #region Enemy
    private Transform transform;
    public bool player1Inside = false;
    public bool player2Inside = false;
    public string enemyType = "NoEvil";

    #endregion



    # region UnityFunctions
    private void Awake()
    {
        transform = GetComponent<Transform>();
    }
    private void Update()
    {

    }
    #endregion



    public void HandleDirection(Vector2 currDirection) {
        transform.localPosition = currDirection / 2;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Player"){
            if (col.gameObject.GetComponent<PlayerController>().playerID == 1) {
                player1Inside = true;
            } else {
                player2Inside = true;
            }
            if (enemyType != "Zealotry") {
                GetComponentInParent<EnemyScript>().PlayerInHurtBox();
            }
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if(col.gameObject.tag == "Player"){
            if (col.gameObject.GetComponent<PlayerController>().playerID == 1) {
                player1Inside = false;
            } else {
                player2Inside = false;
            }
        }
    }

    public void HurtPlayer(float val) {
        if (player1Inside) {
            // ADJUST HERE FOR PLAYER 2 COMPATIBILITY
            GameObject.Find("Player1").GetComponent<PlayerController>().TakeDamage(val, transform.position);
        }    
        if (player2Inside) {
            // ADJUST HERE FOR PLAYER 2 COMPATIBILITY
            GameObject.Find("Player2").GetComponent<PlayerController>().TakeDamage(val, transform.position);
        }    
    }
}
