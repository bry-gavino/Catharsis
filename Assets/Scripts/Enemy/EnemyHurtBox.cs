using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtBox : MonoBehaviour
{

    #region Enemy
    Transform transform;
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
            GetComponentInParent<EnemyScript>().PlayerInHurtBox();
        }
    }
}
