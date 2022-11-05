using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtBox : MonoBehaviour
{

    #region Player
    Transform transform;
    #endregion

    private List<GameObject> collisions;



    # region UnityFunctions
    private void Awake()
    {
        transform = GetComponent<Transform>();
        collisions = new List<GameObject>();
    }
    private void Update()
    {

    }
    #endregion



    public void HandleDirection(Vector2 currDirection) {
        transform.localPosition = currDirection / 2;
    }

    // void OnTriggerEnter2D(Collider2D col) {
    //     if(col.gameObject.tag == "Enemy"){
    //         // GetComponentInParent<PlayerController>().HurtEnemy(col);
    //         collisions.Add(col.gameObject);
    //         Debug.Log(collisions.Count);
    //     }
    // }

    // void OnTriggerExit2D(Collider2D col) {
    //     if(col.gameObject.tag == "Enemy"){
    //         collisions.Remove(col.gameObject);
    //         Debug.Log(collisions.Count);
    //     }
    // }

    public void HurtAll(float val, Transform from) {
        // foreach (GameObject col in collisions) {
        //     col.GetComponent<EnemyScript>().GetHit(val, from, false);
        // }
        Collider[] hitColliders = Physics.OverlapBox(
            gameObject.transform.position, transform.localScale, Quaternion.identity);
        foreach (Collider col in hitColliders) {
            col.gameObject.GetComponent<EnemyScript>().GetHit(val, from, false);
        }
    }
}
