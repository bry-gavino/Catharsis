using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    #region player info variables
    public float playerID;
    string leftKey;
    string rightKey;
    string upKey;
    string downKey;
    string dashKey;
    string attackKey;
    string healKey;
    bool dead = false;
    bool isSleeping = false;
    #endregion 

    #region xp variables
    float exp = 0;
    float skillPoints = 0;
    float currLevel = 1;
    float expThreshold;
    // public Slider XPSlider;
    #endregion

    #region movement variables
    public float moveSpeed = 8;
    float currSpeed;
    float x_input;
    float y_input;
    Vector2 fromHurt;
    #endregion

    #region hurt variables
    bool isHurt = false;
    public float hurtCooldown = 0.3f;
    float hurtCooldownTimer = 0.0f;
    bool isPushed = false;
    public float pushBackLength = 0.15f;
    float pushBackLengthTimer = 0.0f;
    float pushBackStrength = 10.0f;
    #endregion

    #region dash variables
    bool isDashing = false;
    public float dashLength = 0.12f;
    public float dashCooldown = 0.5f;
    public float dashSpeed = 33;
    float dashLengthTimer;
    float dashCooldownTimer;
    #endregion

    #region attack variables
    public float Damage;
    public float attackSpeed = 1;
    float attackTimer = 0.0f;
    public float hitBoxTiming;
    public float endAnimationTiming;
    public float speedUpTiming;
    bool isAttacking;
    public Vector2 currDirection;
    float attackLunge = 0.08f;
    float attackLength = 0.33f;
    float attackCooldown = 0.7f;
    float attackCooldownTimer = 0.0f;
    #endregion

    #region physics
    Rigidbody2D PlayerRB;
    GameObject HurtBox;
    GameObject Effects;
    #endregion

    #region animation
    Animator anim;
    #endregion

    #region health
    public float maxHealth;
    public float currHealth;
    public string[] powers;
    // public Slider HPSlider;
    public ArrayList inventory = new ArrayList();
    #endregion

    #region powers
    [SerializeField]
    [Tooltip("Current power")]
    public PowerInfo curr_power;
    #endregion

    #region unity funcs

    // called once when object created
    private void Awake() {
        HurtBox = GameObject.Find("PlayerHurtBox");
        Effects = GameObject.Find("PlayerEffects");
        PlayerRB = GetComponent<Rigidbody2D>();

        // TODO: make Animator
        anim = GetComponent<Animator>();

        // currSpeed = moveSpeed;
        currHealth = maxHealth;
        if (playerID == 1) {

            leftKey = "a";
            rightKey = "d";
            upKey = "w";
            downKey = "s";

            dashKey = "g";
            attackKey = "f";
            healKey = "r";
        } else if (playerID == 2) {

            leftKey = "k";
            rightKey = ";";
            upKey = "o";
            downKey = "l";
            
            dashKey = "h";
            attackKey = "j";
            healKey = "u";
        }
        expThreshold = exp * 100;
        // HPSlider.value = currHealth / maxHealth;
        // XPSlider.value = exp;
    }

    // called once per frame
    private void Update() {
        if (!isSleeping) {
            x_input = Input.GetAxisRaw("Horizontal");
            y_input = Input.GetAxisRaw("Vertical");

            HandleInput();
            HandleTimer();
            Attacking();
            Move();
            HandleState();
        }
    }
    private void HandleTimer() {
        attackTimer -= Time.deltaTime;
        attackCooldownTimer -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        dashLengthTimer -= Time.deltaTime;
        hurtCooldownTimer -= Time.deltaTime;
        pushBackLengthTimer -= Time.deltaTime;
        // HANDLE TIMER
        if (dashLengthTimer <= 0) {
            isDashing = false;
        } if (hurtCooldownTimer <= 0) {
            isHurt = false;
            anim.SetBool("Hurt", false);
        } if (pushBackLengthTimer <= 0) {
            isPushed = false;
        } if (attackTimer <= 0) {
            isAttacking = false;
        }
    }
    private void HandleInput() {
        if (!isHurt && !isPushed) {
            if (Input.GetKey(attackKey) && attackCooldownTimer <= 0) {
                Attack();
            } if (Input.GetKey(dashKey) && (dashCooldownTimer <= 0)) {
                Dash();
            }
        }
    }
    private void HandleState() {
        if (isPushed) {
            Effects.GetComponent<PlayerEffects>().SetState(3);
        } else if (isAttacking) {
            Effects.GetComponent<PlayerEffects>().SetState(2);
        } else if (isDashing) {
            Effects.GetComponent<PlayerEffects>().SetState(1);
        } else {
            Effects.GetComponent<PlayerEffects>().SetState(0);
        }
    }
    #endregion 







    #region movement dash funcs

    private void Move() {

        if (!dead)
        {
            if (isPushed) {
                Debug.Log("is pushed!");
                PlayerRB.velocity = (fromHurt - new Vector2(transform.position.x, transform.position.y)).normalized * -pushBackStrength;
                Effects.GetComponent<PlayerEffects>().HandleDirection(-PlayerRB.velocity);
            } else if (isHurt) {
                Debug.Log("is hurt!");
                PlayerRB.velocity = Vector2.zero;
            } else {
                // HANDLE MOVEMENT HERE
                Vector2 oldDirection = currDirection;
                currDirection = Vector2.zero;
                if (Input.GetKey(leftKey) || Input.GetKey(rightKey) || Input.GetKey(upKey) || Input.GetKey(downKey)) {    
                    if (Input.GetKey(rightKey)) { // EAST
                        currDirection += Vector2.right;
                    } if (Input.GetKey(leftKey)) { // WEST
                        currDirection += Vector2.left;
                    } if (Input.GetKey(upKey)) { // NORTH
                        currDirection += Vector2.up;
                    } if (Input.GetKey(downKey)) { // SOUTH
                        currDirection += Vector2.down;
                    }
                    anim.SetBool("Moving", true);
                    anim.SetFloat("DirX", currDirection.x);
                    anim.SetFloat("DirY", currDirection.y);
                } else {
                    anim.SetBool("Moving", false);
                    currDirection = oldDirection;
                }

                if (isDashing) {
                    PlayerRB.velocity = currDirection * dashSpeed;
                } else if (isAttacking) {
                    PlayerRB.velocity = oldDirection * (moveSpeed/2);
                } else if (anim.GetBool("Moving")) {
                    PlayerRB.velocity = currDirection * moveSpeed;
                } else {
                    PlayerRB.velocity = Vector2.zero;
                }
                Effects.GetComponent<PlayerEffects>().HandleDirection(currDirection);
            }
        }
        else
        {
            PlayerRB.velocity = Vector2.zero;
            anim.SetBool("Dead", true);
        }

        // HANDLE HITBOX PLACEMENT HERE
        HurtBox.GetComponent<PlayerHurtBox>().HandleDirection(currDirection);
    }

    private void Dash() {
        GameObject.Find("DashFX").GetComponent<AudioSource>().Play();
        dashLengthTimer = dashLength;
        dashCooldownTimer = dashCooldown;
        isDashing = true;
    }

    #endregion








    #region attack funcs

    private void Attacking() {
        if (isAttacking) {
            HurtBox.GetComponent<PlayerHurtBox>().HurtAll(Damage, transform);   
        }
    }

    private void Attack() {
        Debug.Log("attacking now");
        Debug.Log(currDirection);
        attackTimer = attackSpeed;
        dashLengthTimer = attackLunge; // propels character forward (like a lunge)
        dashCooldownTimer = dashCooldown;
        attackTimer = attackLength;
        attackCooldownTimer = attackCooldown;
        isDashing = true;
        isAttacking = true;
        anim.SetTrigger("Attacking");
    }

    public void OnDashTriggerEnter2D(Collider2D col) {
        if(isDashing) {
            col.gameObject.GetComponent<EnemyScript>().GetHit(Damage/4, PlayerRB.transform, isDashing);
        }
    } 

    // public void HurtEnemy(Collision2D col) {
    //     if(isAttacking) {
    //         col.gameObject.GetComponent<EnemyScript>().GetHit(Damage, transform, false);
    //     }
    // }
    #endregion

    #region health funcs

    public void TakeDamage(float val, Vector2 from) {
        fromHurt = from;
        currHealth -= val;
        Debug.Log("health is now " + currHealth.ToString());

        // HPSlider.value = currHealth / maxHealth;

        if (currHealth <= 0) {
            Die();
        }
        if (!isHurt) {
            isHurt = true;
            anim.SetBool("Hurt", true);
            hurtCooldownTimer = hurtCooldown;
        }
        isPushed = true;
        pushBackLengthTimer = pushBackLength;
    }

    private void Die() {
        //FindObjectOfType<AudioManager>().Play("PlayerDeath");
        Destroy(this.gameObject);

        //GameObject gm = GameObject.FindWithTag("GameController");
        //gm.GetComponent<GameManager>().LoseGame();
    }

    private void Interact() {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position + currDirection, 
            new Vector2(0.5f, 0.5f), 0f, Vector2.zero, 0f);
        foreach (RaycastHit2D hit in hits) {
            if (!hit.transform.CompareTag("Enemy")) {
                inventory.Add(GetComponent<HealingVial>());
            }
        }
    }

    public void Heal(float val) {
        if (inventory.Count > 0) {
            currHealth = Mathf.Min(currHealth + val, maxHealth);
            Debug.Log("health is now " + currHealth.ToString());

            // HPSlider.value = currHealth / maxHealth;

            inventory.RemoveAt(inventory.Count - 1);
        } 
    }

    #endregion

    #region xp funcs

    void add_xp(int add) {
        exp += add;
        if ((expThreshold - exp) <= 0) {
            exp = exp - expThreshold;
            skillPoints += currLevel * 50;
            currLevel += 1;
            expThreshold = currLevel * 100;
        }
        // HPSlider.value = exp;
    }

    #endregion

    #region power funcs

    public void changePower(PowerInfo power) {
        curr_power = power;
    }

    #endregion





    #region game manager
    public void enableUserInput() {isSleeping = false;}
    public void disableUserInput() {
        isSleeping = true;
        PlayerRB.velocity = Vector2.zero;
    }
    #endregion

    
}
