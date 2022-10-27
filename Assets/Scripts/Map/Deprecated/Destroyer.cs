using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other) {
        //if(other.transform.CompareTag("SpawnPoint") || other.transform.CompareTag("Destroyer"))
        if (other.transform.CompareTag("SpawnPoint")) {
            Destroy(other.gameObject);
        }
    }
}