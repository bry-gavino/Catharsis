using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    #region Enemy_Type
    public bool hurtWhenTouched = true;
    public string enemyType = "NoEvil";
    #endregion

    #region enemy sounds
    [SerializeField] [Tooltip("Sound when enemy attacks.")]
    private AudioClip AttackFX;
    [SerializeField] [Tooltip("Sound when enemy hurts.")]
    private AudioClip HurtFX;
    [SerializeField] [Tooltip("Sound when enemy setup.")]
    private AudioClip SetupFX;
    [SerializeField] [Tooltip("Object to instantiate when die.")]
    private GameObject DieObject;

    private MusicManager musicManager;
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
    float attackPushBackLength = 0.4f;
    private float hurtTimer = 0.0f;
    float hurtAttackTimer;
    float hurtDashTimer;
    public float graceLength = 0.0f;
    private float graceTimer = 0.0f;
    Vector2 currDirection;
    #endregion

    #region Animation
    Animator anim;
    private bool isMoving = false;
    private bool isSetup = false;
    EnemyEffects Effects;
    #endregion

    #region Physics_Components
    Rigidbody2D EnemyRB;
    #endregion

    #region Targeting_variables
    public GameObject Player;
    private int PlayerIDHitBy;
    private int PlayersCombo;
    #endregion

    #region Attack_variables
    public float attackDamage = 2;
    private bool isAttacking = false;
    public float setupLength = 0.7f;
    private float setupTimer = 0.0f;
    private float attackTimer = 0.0f;
    public float attackLength = 0.3f;
    public float attackSpeed = 10;
    #endregion

    #region Health_variables
    public float maxHealth;
    float currHealth;
    float vial_drop;
    #endregion

    #region xp
    public int xp_val = 3;
    #endregion

    [SerializeField]
    private HealingVial healthpot;

    #region handle enemy type
    #endregion



    #region Unity_functions
    
    private void Awake(){
        musicManager = GameObject.Find("GameManager").GetComponent<MusicManager>();
        Effects = (GetComponentInChildren(typeof(EnemyEffects)) as EnemyEffects);
        EnemyRB = GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Player1"); // ADJUST FOR 2 PLAYER
        anim = GetComponent<Animator>();
        currHealth = maxHealth;
        hurtAttackTimer = Player.GetComponent<PlayerController>().getAttackLength();
        hurtDashTimer = Player.GetComponent<PlayerController>().getDashLength();
    }

    private void Update(){
        // Effects.GetComponent<EnemyEffects>().HandleDirection(EnemyRB.velocity, true);
        if (graceTimer > 0.0f) {
            graceTimer -= Time.deltaTime;
            if (graceTimer <= 0.0f) {
                graceTimer = 0.0f;
                if ((GetComponentInChildren(typeof(EnemyHurtBox)) as EnemyHurtBox).playerInside) {
                    PlayerInHurtBox();
                }
            }
        }
        if (attackTimer > 0.0f) {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0.0f) {
                attackTimer = 0.0f;
                isAttacking = false;
            } else {
                isAttacking = true;
                if (enemyType == "Ignorance") {
                    Vector2 direction = Player.transform.position - transform.position;
                    EnemyRB.velocity = direction.normalized * attackSpeed;
                } else if (enemyType == "Guilt") {
                    EnemyRB.velocity = Vector2.zero;
                } else {
                    EnemyRB.velocity = currDirection * attackSpeed;
                }
                (GetComponentInChildren(typeof(EnemyHurtBox)) as EnemyHurtBox).HurtPlayer(attackDamage);
                if (enemyType == "Guilt") {
                    Effects.HandleDirection(new Vector2(0, -1), true);
                } else {
                    Effects.HandleDirection(EnemyRB.velocity, true);
                }
            }
        }
        if (setupTimer > 0.0f) {
            if (enemyType == "Guilt") {
                // if (coolDown)
                EnemyRB.velocity = Vector2.zero;
            }
            isSetup = true;
            setupTimer -= Time.deltaTime;
            if (setupTimer <= 0.0f) {
                isSetup = false;
                setupTimer = 0.0f;
                // trigger attack
                EnactAttack();
            }
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
        if (hurtTimer > 0.0f) {
            isHurt = true;
            hurtTimer -= Time.deltaTime;
            if (hurtTimer <= 0.0f) {
                hurtTimer = 0.0f;
                isHurt = false;
            }
        }
        if (isChasing && !isImmobile && !isAttacking) {
            Move();
            isMoving = true;
        } else {
            isMoving = false;
        }
        
        anim.SetFloat("DirX", currDirection.x);
        anim.SetFloat("DirY", currDirection.y);
        HandleState();
    }
    void HandleState() {
        if (isHurt) {
            anim.SetInteger("State", 4);
            Effects.SetState(4);
        } else if (isAttacking) {
            anim.SetInteger("State", 3);
            Effects.SetState(3);
        } else if (isSetup) {
            anim.SetInteger("State", 2);
            Effects.SetState(2);
        } else if (isMoving) {
            anim.SetInteger("State", 1);
            Effects.SetState(1);
        } else {
            anim.SetInteger("State", 0);
            Effects.SetState(0);
        }
    }

    #endregion

    #region Movement_functions
    private void Move() {
        if (Player != null) {
            Vector2 direction = Player.transform.position - transform.position;
            EnemyRB.velocity = direction.normalized * movespeed;
            currDirection = direction.normalized;
            if (enemyType == "Ignorance" || enemyType == "Guilt") {
                (GetComponentInChildren(typeof(EnemyHurtBox)) as EnemyHurtBox).HandleDirection(Vector2.zero);
            }
            else {
                (GetComponentInChildren(typeof(EnemyHurtBox)) as EnemyHurtBox).HandleDirection(currDirection);
            }
        }
    }
    #endregion



    #region Attacking
    void EnactAttack() {
        musicManager.playClip(AttackFX, 1); // fix for enemyType
        isAttacking = true;
        attackTimer = attackLength;
        graceTimer = graceLength;
    }
    #endregion




    #region Health_functions


    public void PlayerInHurtBox() {
        if (graceTimer <= 0.0f) {
            musicManager.playClip(SetupFX, 1);
            setupTimer = setupLength;
            coolDown = setupLength;
            EnemyRB.velocity = Vector2.zero;
        }
    }

    public bool getHurtWhenTouched() {
        return  hurtWhenTouched;
    }

    public void GetHit(float value, Transform from, bool isDashing, int playerID, int combo){
        Debug.Log("BONK ENEMY");
        if (!isHurt || combo != PlayersCombo || playerID != PlayerIDHitBy) {
            isHurt = true;
            musicManager.playClip(HurtFX, 1);
            PlayersCombo = combo;
            PlayerIDHitBy = playerID;
            TakeDamage(value, from.position);
            GetPushedBack(from, isDashing);
        }
    }
    private void TakeDamage(float value, Vector2 from){
        currHealth -= value;
        Debug.Log("Enemy Health is now " + currHealth.ToString());
        if(currHealth <= 0){
            Die();
            // Instantiate(healthpot, transform.position, transform.rotation);
        }
    }
    private void GetPushedBack(Transform from, bool playerIsDashing) {
        if (playerIsDashing) {
            coolDown = dashPushBackLength + recoveryTime;
            pushBackTimer = dashPushBackLength;
            hurtTimer = hurtDashTimer;
        } else {
            coolDown = attackPushBackLength + recoveryTime;
            pushBackTimer = attackPushBackLength;
            hurtTimer = hurtAttackTimer;
        }
        EnemyRB.velocity = (-1) * (EnemyRB.transform.position - from.position).normalized;
        if (enemyType == "Guilt") {
            Effects.HandleDirection(new Vector2(0, -1), true);
        } else {
            Effects.HandleDirection(EnemyRB.velocity, true);
        }
    }

    private void Die(){
        if (PlayerIDHitBy == 1) {
            GameObject.Find("Player1").GetComponent<PlayerController>().add_xp(xp_val); // FIX FOR PLAYER 1
            GameObject.Find("Player1").GetComponent<PlayerController>().addEnemyDefeated(); // FIX FOR PLAYER 1
        } else if (PlayerIDHitBy == 2) {} // FIX FOR PLAYER 2
        Instantiate(DieObject, transform.position, transform.rotation);
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
