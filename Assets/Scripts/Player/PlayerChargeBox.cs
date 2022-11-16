using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeBox : MonoBehaviour
{

    #region Player
    Transform transform;
    public LayerMask mask;
    Rigidbody2D RB;
    int PlayerIDHitBy;
    int PlayersCombo;
    float chargeAttackLength = 0.53f;
    CircleCollider2D CC;
    float scale = 0.15f;
    Vector3 scaleChange;
    #endregion




    # region UnityFunctions
    private void Awake()
    {
        transform = GetComponent<Transform>();
        RB = GetComponent<Rigidbody2D>();
        CC = GetComponent<CircleCollider2D>();
        int[] ids = GetComponentInParent<PlayerController>().identifyHurtBox();
        PlayerIDHitBy = ids[0];
        PlayersCombo = ids[1];
        scaleChange = new Vector3(scale, scale, scale);
    }
    private void Update()
    {
        if (chargeAttackLength <= 0.0f) {
            Destroy(this.gameObject);
        }
        chargeAttackLength -= Time.deltaTime;
        // CC.radius += 0.1f;
        this.transform.localScale += scaleChange;
    }
    #endregion



    public void HandleDirection(Vector2 currDirection) {
        transform.localPosition = currDirection / 2;
    }

    public void HurtAll(float val, Transform from) {
        RaycastHit2D[] hitColliders = Physics2D.BoxCastAll(RB.position, new Vector2(2.5f, 2.5f), 0f, Vector2.zero);
        foreach (RaycastHit2D col in hitColliders) {
            if (col.transform.CompareTag("EnemyBody")) {
                col.transform.GetComponentInParent<EnemyScript>().GetHit(val, from, false, PlayerIDHitBy, PlayersCombo);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Enemy") {
            GetComponentInParent<PlayerController>().OnAttackTriggerEnter2D(col);
        }
    } 
}
