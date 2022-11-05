using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    #region Enemy_Type
    public bool hurtWhenTouched = true;
    #endregion


    #region Movement_variables
    public float movespeed;
    public bool isAwake;
    private bool isHurt = false;
    private bool isChasing = false;
    private bool isImmobile = false;
    private float coolDown = 0.0f;
    float dashPushBackLength = 0.25f;
    private float pushBackTimer = 0.0f;
    public float recoveryTime = 0.7f;
    #endregion

    #region Physics_Components
    Rigidbody2D EnemyRB;
    #endregion

    #region Targeting_variables
    public GameObject Player;
    #endregion

    #region Attack_variables
    public float attackDamage = 2;
    private bool isAttacking = false;
    public float setupTimer = 0.2f;
    float attackPushBackLength = 0.4f;
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
    private HealingVial healthpot;



    #region Unity_functions
    
    private void Awake(){
        EnemyRB = GetComponent<Rigidbody2D>();
        Player = GameObject.Find("TestPlayer");
        currHealth = maxHealth;
    }

    private void Update(){

        if (isChasing && !isImmobile) {
            Move();
        }
        if (coolDown > 0.0f) {
            coolDown -= Time.deltaTime;
            if (coolDown <= 0.0f) {
                coolDown = 0.0f;
                isImmobile = false;
            } else {
                isImmobile = true;
            }
        }
        if (pushBackTimer > 0.0f) {
            pushBackTimer -= Time.deltaTime;
            if (pushBackTimer <= 0.0f) {
                pushBackTimer = 0.0f;
                EnemyRB.velocity = Vector2.zero;
                isHurt = false;
            }
        }

    }

    #endregion

    #region Movement_functions
    private void Move(){

        Vector2 direction = Player.transform.position - transform.position;
        EnemyRB.velocity = direction.normalized * movespeed;
        (GetComponentInChildren(typeof(EnemyHurtBox)) as EnemyHurtBox).HandleDirection(direction.normalized);

    }
    #endregion

    #region Health_functions



    public void PlayerInHurtBox() {

    }

    public bool getHurtWhenTouched() {
        return  hurtWhenTouched;
    }


    public void GetHit(float value, Transform from, bool isDashing){
        Debug.Log("GET HIT!");
        if (!isHurt) {
            TakeDamage(value, from.position);
            GetPushedBack(from, isDashing);
        }
    }
    private void TakeDamage(float value, Vector2 from){
        currHealth -= value;
        Debug.Log("Enemy Health is now " + currHealth.ToString());
        if(currHealth <= 0){
            Die();
            Instantiate(healthpot, transform.position, transform.rotation);
        }
    }
    private void GetPushedBack(Transform from, bool playerIsDashing) {
        isHurt = true;
        if (playerIsDashing) {
            coolDown = dashPushBackLength + recoveryTime;
            pushBackTimer = dashPushBackLength;
        } else {
            coolDown = attackPushBackLength + recoveryTime;
            pushBackTimer = attackPushBackLength;
        }
        EnemyRB.velocity = (-1) * (EnemyRB.transform.position - from.position);
    }

    private void Die(){
        Destroy(this.gameObject);
    }
    #endregion

    public void becomeAwake(){
        isAwake = true;
    }
    public void becomeIdle(){
        isAwake = false;
    }

    public void chasePlayer(){
        isChasing = true;
    }


}
