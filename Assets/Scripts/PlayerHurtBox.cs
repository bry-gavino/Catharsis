using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtBox : MonoBehaviour
{

    #region Player
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

    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Enemy"){
            // hurt enemy if attacking
        }
    }
}