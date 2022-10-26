using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallCollision : MonoBehaviour {
    /**
     * The group with this script does the collision check.
     * The other group with the tag gets removed.
     *
     * For example:
     * North walls (have this script), and they destroy overlapping South walls (have tag)
     * West walls (have this script), and they destroy East walls (have tag)
     *
     * Note: Standard collision entry functions don't work.
     */
    void Start() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.01f);
        foreach (Collider2D c in colliders) {
            if (c.CompareTag("WallRemovable")) {
                Destroy(c.gameObject);
                return; // only need to destroy 1. Ex: North does South, West does East.
            }
        }
    }
}