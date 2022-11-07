using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtBox : MonoBehaviour
{

    #region Player
    Transform transform;
    public LayerMask mask;
    Rigidbody2D RB;
    #endregion




    # region UnityFunctions
    private void Awake()
    {
        transform = GetComponent<Transform>();
        RB = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {

    }
    #endregion



    public void HandleDirection(Vector2 currDirection) {
        transform.localPosition = currDirection / 2;
    }

    public void HurtAll(float val, Transform from) {
        RaycastHit2D[] hitColliders = Physics2D.BoxCastAll(RB.position, transform.localScale, 0f, Vector2.zero);
        // if ((hitColliders.Length) == 0) {Debug.Log(hitColliders);}
        // NOTE: some errors here -- doesn't detect EnemyBody all the time...use "new" keyword?
        foreach (RaycastHit2D col in hitColliders) {
            if (col.transform.CompareTag("EnemyBody")) {
                col.transform.GetComponentInParent<EnemyScript>().GetHit(val, from, false);
                Debug.Log("ATTACK BALL HIT!");
            }
        }
        // Debug.Log(hit);
    }
}
