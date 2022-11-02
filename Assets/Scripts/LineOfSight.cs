using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public static bool isChasing = false;
    Collision2D coll;
    
    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("H");
         if(col.gameObject.tag == "Player"){
            Debug.Log("SEE PLAYER RUN AT PLAYER");
            isChasing = true;
        } else {
            isChasing = false;
        }

    }

}
