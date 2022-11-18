using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBody : MonoBehaviour
{
    bool hurtWhenTouched = false;
    float attackDamage = 1;
    // Start is called before the first frame update
    void Start()
    {
        hurtWhenTouched = GetComponentInParent<EnemyScript>().getHurtWhenTouched();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(GetComponentInParent<EnemyScript>().getHurtWhenTouched() && col.gameObject.tag == "Player") {
            col.gameObject.GetComponent<PlayerController>().TakeDamage(attackDamage, transform.position);
        }
    } 
}
