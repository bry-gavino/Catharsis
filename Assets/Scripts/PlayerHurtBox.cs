using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtBox : MonoBehaviour
{

    #region Player
    Transform transform;
    public LayerMask mask;
    Rigidbody2D RB;
    int PlayerIDHitBy;
    int PlayersCombo;
    #endregion




    # region UnityFunctions
    private void Awake()
    {
        transform = GetComponent<Transform>();
        RB = GetComponent<Rigidbody2D>();
        int[] ids = GetComponentInParent<PlayerController>().identifyHurtBox();
        PlayerIDHitBy = ids[0];
        PlayersCombo = ids[1];
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
        // Debug.Log("HURT ALL");
        foreach (RaycastHit2D col in hitColliders) {
            // Debug.Log("HURT ALL");
            if (col.transform.CompareTag("EnemyBody")) {
                col.transform.GetComponentInParent<EnemyScript>().GetHit(val, from, false, PlayerIDHitBy, PlayersCombo);
            }
        }
        // Debug.Log(hit);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Enemy") {
            GetComponentInParent<PlayerController>().OnAttackTriggerEnter2D(col);
        }
    } 
}
