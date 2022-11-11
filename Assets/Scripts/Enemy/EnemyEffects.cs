using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffects : MonoBehaviour
{

    #region Player
    Transform transform;
    Animator anim;
    #endregion
    // Using int values to set state:
    //  0 --> nothing
    //  1 --> move
    //  2 --> setup
    //  3 --> attack
    //  4 --> hurt



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



    public void HandleDirection(Vector2 currDirection, bool front) {
        float deg = 0;
        if (front) {
            float rad = Mathf.Atan2(currDirection.x, -currDirection.y);
            deg = rad * Mathf.Rad2Deg;
        } else {
            float rad = Mathf.Atan2(currDirection.x, -currDirection.y);
            deg = rad * Mathf.Rad2Deg;
        }

        transform.eulerAngles = new Vector3 (0, 0, deg);
    }

    public void SetState(int s) {
        anim.SetInteger("state", s);
    }
}
