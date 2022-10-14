using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    #region Variables
    private bool playerInRoom = false;
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            Debug.Log("Player entered room");
            playerInRoom = true;

            // TODO: awaken all enemies
            Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity);
            foreach (var hit in hitColliders)
            {
                if(hit.transform.CompareTag("Enemy"))
                {
                    // TODO: call enemy script's awake function
                    Debug.Log("Enemy awakens");
                    hit.transform.GetComponent<EnemyScript>().becomeAwake();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            Debug.Log("Player exited room");
            playerInRoom = false;

            // TODO: make all enemies sleep
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, Vector2.one, 0f, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                if(hit.transform.CompareTag("Enemy"))
                {
                    // TODO: call enemy script's sleep function
                    Debug.Log("Enemy sleeps");
                    hit.transform.GetComponent<EnemyScript>().becomeIdle();
                }
            }
        }
    }
}
