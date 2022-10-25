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
        // float angle = Vector2.Angle(Vector2.zero, currDirection);
        float rad = Mathf.Atan2(currDirection.x, -currDirection.y);
        float deg = rad * Mathf.Rad2Deg;
        Debug.Log(currDirection);
        Debug.Log(deg);

        transform.eulerAngles = new Vector3 (0, 0, deg);
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Enemy"){
            // hurt enemy if attacking
        }
    }

    public void SetState(int s) {
        anim.SetInteger("state", s);
    }
}
