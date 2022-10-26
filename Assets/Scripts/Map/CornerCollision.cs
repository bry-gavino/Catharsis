using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerCollision : MonoBehaviour
{
    void Start() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.01f);
        foreach (Collider2D c in colliders) {
            if (c.CompareTag("Corner") && (c.gameObject != gameObject)) {
                Destroy(c.gameObject);
            }
        }
    }
}
