using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    #region Movement_variables
    public float movespeed;
    public bool isAwake;
    #endregion

    #region Physics_Components
    Rigidbody2D EnemyRB;
    #endregion

    #region Targeting_variables
    public Transform player;
    #endregion

    #region Attack_variables
    public float attackDamage;
    #endregion

    #region Health_variables
    public float maxHealth;
    float currHealth;
    float vial_drop;
    #endregion

    #region xp
    public float xp_val = 3;
    #endregion

    [SerializeField]
    private GameObject healthpot;



    #region Unity_functions
    
    private void Awake(){
        EnemyRB = GetComponent<Rigidbody2D>();
        currHealth = maxHealth;
    }

    private void Update(){

        if (isAwake) {
            Debug.Log("AWAKE!");
            Move();
        }
    }

    #endregion

    #region Movement_functions
    private void Move(){

        Vector2 direction = player.position - transform.position;
        EnemyRB.velocity = direction.normalized * movespeed;

    }
    #endregion

    #region Health_functions
    public void TakeDamage(float value){

        currHealth -= value;
        Debug.Log("Health is now " + currHealth.ToString());

        if(currHealth <= 0){
            Die();
            Instantiate(healthpot, transform.position, transform.rotation);
        }
    }
    private void Die(){
        Destroy(this.gameObject);
    }
    #endregion
}
