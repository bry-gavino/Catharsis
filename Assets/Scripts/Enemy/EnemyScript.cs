using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    #region Enemy_Type
    public bool hurtWhenTouched = true;
    public string enemyType = "NoEvil";
    [SerializeField][Tooltip("Set true if this enemy prefab is a boss. Will open the boss gate.")]
    public bool isBoss = false; /* Addition for boss implementation */
    #endregion

    #region enemy sounds
    [SerializeField] [Tooltip("Sound when enemy attacks.")]
    private AudioClip AttackFX;
    [SerializeField] [Tooltip("Sound when enemy hurts.")]
    private AudioClip HurtFX;
    [SerializeField] [Tooltip("Sound when enemy setup.")]
    private AudioClip SetupFX;
    [SerializeField] [Tooltip("Sound2 when enemy setup.")]
    private AudioClip Setup2FX;
    [SerializeField] [Tooltip("Object to instantiate when die.")]
    private GameObject DieObject;
    [SerializeField] [Tooltip("Object to instantiate when hit.")]
    private GameObject HitObject;

    private MusicManager musicManager;
    private bool canPlayMusic = true;
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
    public GameObject Player1;
    public GameObject Player2;
    GameObject HitObj;
    private int PlayerIDHitBy;
    private int PlayersCombo;
    private GameObject PlayerTarget;
    public int numPlayers;
    #endregion

    #region Attack_variables
    public float attackDamage = 2;
    private bool isAttacking = false;
    public float setupLength = 0.7f;
    private float setupTimer = 0.0f;
    private float attackTimer = 0.0f;
    public float attackLength = 0.3f;
    public float attackSpeed = 10;
    Collider2D CC;
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
        CC = GetComponent<Collider2D>();
        EnemyRB = GetComponent<Rigidbody2D>();
        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");
        if (Player1 != null) {
            PlayerTarget = Player1;
            hurtAttackTimer = Player1.GetComponent<PlayerController>().getAttackLength();
            hurtDashTimer = Player1.GetComponent<PlayerController>().getDashLength();
        } else {
            PlayerTarget = Player2;
            hurtAttackTimer = Player2.GetComponent<PlayerController>().getAttackLength();
            hurtDashTimer = Player2.GetComponent<PlayerController>().getDashLength();
        }
        anim = GetComponent<Animator>();
        currHealth = maxHealth;
    }

    private void Update(){
        if (graceTimer > 0.0f) {
            graceTimer -= Time.deltaTime;
            if (graceTimer <= 0.0f) {
                graceTimer = 0.0f;
                if (enemyType == "Zealotry" || enemyType == "Loathing") {
                    PlayerInHurtBox();
                } else if ((GetComponentInChildren(typeof(EnemyHurtBox)) as EnemyHurtBox).player1Inside || (GetComponentInChildren(typeof(EnemyHurtBox)) as EnemyHurtBox).player2Inside) { // TODO
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
                    Vector2 direction = PlayerTarget.transform.position - transform.position;
                    EnemyRB.velocity = direction.normalized * attackSpeed;
                } else if (enemyType == "Guilt" || enemyType == "Loathing") {
                    EnemyRB.velocity = Vector2.zero;
                } else {
                    EnemyRB.velocity = currDirection * attackSpeed;
                }
                Debug.Log("ATTACKING PLAYER!");
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
            } else if (enemyType == "Zealotry") {
                if (setupTimer > 1.0f) {
                    hurtWhenTouched = false;
                    EnemyRB.velocity = Vector2.zero;
                    currDirection = (PlayerTarget.transform.position - transform.position).normalized;
                } else if (setupTimer > 0.3f) {
                    if (canPlayMusic) {
                        musicManager.playClip(SetupFX, 1);
                        canPlayMusic = false;
                    }
                    hurtWhenTouched = true;
                    EnemyRB.velocity = currDirection * attackSpeed;
                } else {
                    hurtWhenTouched = false;
                    EnemyRB.velocity = Vector2.zero;
                    currDirection = (PlayerTarget.transform.position - transform.position).normalized;
                }
            } else if (enemyType == "Loathing") {
                if (setupTimer > 1.1f) {
                    EnemyRB.velocity = Vector2.zero;
                    if (canPlayMusic) {
                        musicManager.playClip(SetupFX, 1);
                        canPlayMusic = false;
                    }
                } else if (setupTimer > 0.6f) {
                    CC.isTrigger = true;
                    canPlayMusic = true;
                    EnemyRB.velocity = (PlayerTarget.transform.position - transform.position).normalized * movespeed;
                } else {
                    CC.isTrigger = false;
                    EnemyRB.velocity = Vector2.zero;
                    if (canPlayMusic) {
                        musicManager.playClip(Setup2FX, 1);
                        canPlayMusic = false;
                    }
                }
            }
            isSetup = true;
            setupTimer -= Time.deltaTime;
            if (setupTimer <= 0.0f) {
                canPlayMusic = true;
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
            if (enemyType == "Zealotry" || enemyType == "Loathing") {
                PlayerInHurtBox();
            }
            if (enemyType != "Loathing") {
                Move();
            }
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
            // Effects.SetState(4);
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
        if (PlayerTarget != null) {
            Vector2 direction = PlayerTarget.transform.position - transform.position;
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
        if (graceTimer <= 0.0f && setupTimer <= 0.0f) {
            if (enemyType != "Zealotry" && enemyType != "Loathing") {
                musicManager.playClip(SetupFX, 1);
            }
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
            HitObj = Instantiate(HitObject, transform.position, transform.rotation, transform);
            HitObj.GetComponent<HitEffects>().HandleDirection(EnemyRB.velocity);
            isHurt = true;
            musicManager.playClip(HurtFX, 1);
            PlayersCombo = combo;
            PlayerIDHitBy = playerID;
            TakeDamage(value, from.position);
            GetPushedBack(from, isDashing);
            if (PlayerIDHitBy == 1) {
                PlayerTarget = Player1;
            } else {
                PlayerTarget = Player2;
            }
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
        if (HitObj != null) {
            if (enemyType == "Guilt") {
                HitObj.GetComponent<HitEffects>().HandleDirection(new Vector2(0, -1));
            } else {
                HitObj.GetComponent<HitEffects>().HandleDirection(EnemyRB.velocity);
            }
        }
    }

    private void Die(){
        if (PlayerIDHitBy == 1) {
            GameObject.Find("Player1").GetComponent<PlayerController>().add_xp(xp_val); 
            GameObject.Find("Player1").GetComponent<PlayerController>().addEnemyDefeated(); 
        } else {
            GameObject.Find("Player2").GetComponent<PlayerController>().add_xp(xp_val); 
            GameObject.Find("Player2").GetComponent<PlayerController>().addEnemyDefeated(); 
        }
        if (isBoss) {
            /* Disable the gate */
            Debug.Log("Disable boss gate in enemy Die().");
            GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<DungeonGenerator>().DisableGates();
        }
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

    public void chasePlayer(int id){
        isChasing = true;
        if (id == 2) {
            PlayerTarget = Player2;
        }
    }


}
