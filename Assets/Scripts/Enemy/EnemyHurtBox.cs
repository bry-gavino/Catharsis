using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtBox : MonoBehaviour
{

    #region Enemy
    private Transform transform;
    public bool playerInside = false;
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
            playerInside = true;
            GetComponentInParent<EnemyScript>().PlayerInHurtBox();
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if(col.gameObject.tag == "Player"){
            playerInside = false;
        }
    }

    public void HurtPlayer(float val) {
        if (playerInside) {
            // ADJUST HERE FOR PLAYER 2 COMPATIBILITY
            GameObject.Find("Player1").GetComponent<PlayerController>().TakeDamage(val, transform.position);
        }    
    }
}
