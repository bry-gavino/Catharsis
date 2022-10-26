using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallCollision : MonoBehaviour {
    /**
     * Destroys other walls that overlap with this one (if they have the tag).
     *
     * Note: Standard collision entry functions don't work.
     */
    void Start() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.01f);
        foreach (Collider2D c in colliders) {
            if (c.CompareTag("Walls") && (c.gameObject != gameObject)) {
                Destroy(c.gameObject);
            }
        }
    }
}