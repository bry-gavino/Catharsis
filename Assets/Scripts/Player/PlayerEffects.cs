using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{

    #region Player
    Transform transform;
    Animator anim;
    #endregion
    // Using int values to set state:
    //  0 --> nothing
    //  1 --> dash
    //  2 --> attack
    //  3 --> hurt
    //  10 --> charge
    //  11 --> charge2 
    //  12 --> longAttack
    int state = 0;



    # region UnityFunctions
    private void Awake()
    {
        transform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {

    }
    #endregion



    public void HandleDirection(Vector2 currDirection) {
        float rad = Mathf.Atan2(currDirection.x, -currDirection.y);
        float deg = rad * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3 (0, 0, deg);
    }

    // void OnCollisionEnter2D(Collision2D col) {
    //     if(col.gameObject.tag == "Enemy"){
    //         // hurt enemy if attacking
    //     }
    // }

    public void SetState(int s) {
        anim.SetInteger("state", s);
        state = s;
    }
}
