using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour {
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("RoomWall")) {
            Destroy(gameObject);
        }
    }
}