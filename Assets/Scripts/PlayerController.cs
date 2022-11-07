using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    #region player sounds
    [SerializeField] [Tooltip("Sound when you dash.")]
    private AudioClip DashFX;

    private MusicManager musicManager;
    #endregion

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
    public float moveSpeed;
    float currSpeed;
    float x_input;
    float y_input;
    #endregion

    #region dash variables
    bool isDashing = false;
    public float dashLength;
    public float dashCooldown;
    public float dashSpeed;
    float dashLengthTimer;
    float dashCooldownTimer;
    #endregion

    #region attack variables
    public float Damage;
    public float attackSpeed = 1;
    float attackTimer;
    public float hitBoxTiming;
    public float endAnimationTiming;
    public float speedUpTiming;
    bool isAttacking;
    public Vector2 currDirection;
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
    [SerializeField] [Tooltip("Current power")]
    public PowerInfo curr_power;
    #endregion

    #region unity funcs
    // called once when object created
    private void Awake() {
        HurtBox = GameObject.Find("PlayerHurtBox");
        Effects = GameObject.Find("PlayerEffects");
        PlayerRB = GetComponent<Rigidbody2D>();
        attackTimer = 0;

        // TODO: make Animator
        anim = GetComponent<Animator>();
        musicManager = GameObject.Find("GameManager").GetComponent<MusicManager>();

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
        }
        else if (playerID == 2) {
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
            if (isAttacking) {
                return;
            }

            x_input = Input.GetAxisRaw("Horizontal");
            y_input = Input.GetAxisRaw("Vertical");

            HandleInput();
            Move();
            HandleState();
        }
    }

    private void HandleInput() {
        if (Input.GetKey(attackKey) && attackTimer <= 0) {
            Attack();
        }

        if (Input.GetKey(dashKey) && (dashCooldownTimer <= 0)) {
            Dash();
        }

        attackTimer -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        dashLengthTimer -= Time.deltaTime;
        // HANDLE TIMER
        if (dashLengthTimer <= 0) {
            isDashing = false;
        }
    }

    private void HandleState() {
        if (isAttacking) {
            Effects.GetComponent<PlayerEffects>().SetState(2);
        }
        else if (isDashing) {
            Effects.GetComponent<PlayerEffects>().SetState(1);
        }
        else {
            Effects.GetComponent<PlayerEffects>().SetState(0);
        }
    }
    #endregion


    #region movement dash funcs
    private void Move() {
        if (!dead) {
            // HANDLE MOVEMENT HERE
            Vector2 oldDirection = currDirection;
            currDirection = Vector2.zero;
            if (Input.GetKey(leftKey) || Input.GetKey(rightKey) || Input.GetKey(upKey) || Input.GetKey(downKey)) {
                if (Input.GetKey(rightKey)) {
                    // EAST
                    currDirection += Vector2.right;
                }

                if (Input.GetKey(leftKey)) {
                    // WEST
                    currDirection += Vector2.left;
                }

                if (Input.GetKey(upKey)) {
                    // NORTH
                    currDirection += Vector2.up;
                }

                if (Input.GetKey(downKey)) {
                    // SOUTH
                    currDirection += Vector2.down;
                }

                anim.SetBool("Moving", true);
                anim.SetFloat("DirX", currDirection.x);
                anim.SetFloat("DirY", currDirection.y);
            }
            else {
                anim.SetBool("Moving", false);
                currDirection = oldDirection;
            }

            if (isDashing) {
                PlayerRB.velocity = currDirection * dashSpeed;
            }
            else if (anim.GetBool("Moving")) {
                PlayerRB.velocity = currDirection * moveSpeed;
            }
            else {
                PlayerRB.velocity = Vector2.zero;
            }
        }
        else {
            PlayerRB.velocity = Vector2.zero;
            anim.SetBool("Dead", true);
        }

        // HANDLE HITBOX PLACEMENT HERE
        HurtBox.GetComponent<PlayerHurtBox>().HandleDirection(currDirection);
        Effects.GetComponent<PlayerEffects>().HandleDirection(currDirection);
    }

    private void Dash() {
        musicManager.playClip(DashFX, 1);
        dashLengthTimer = dashLength;
        dashCooldownTimer = dashCooldown;
        isDashing = true;
        // StartCoroutine(DashRoutine()); 
    }

    // IEnumerator DashRoutine() {}
    #endregion


    #region attack funcs
    private void Attack() {
        Debug.Log("attacking now");
        Debug.Log(currDirection);
        attackTimer = attackSpeed;
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine() {
        isAttacking = true;
        PlayerRB.velocity = Vector2.zero;

        anim.SetTrigger("Attacking");
        //FindObjectOfType<AudioManager>().Play("PlayerAttack");

        yield return new WaitForSeconds(hitBoxTiming);
        // Debug.Log("casting hitbox now");
        /*RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position + currDirection, Vector2.one, 0f, Vector2.zero);

        foreach (RaycastHit2D hit in hits) {
            Debug.Log(hit.transform.name);
            if(hit.transform.CompareTag("Enemy")) {
                if (hit.transform.GetComponent<Enemy>() != null) {
                    hit.transform.GetComponent<Enemy>().TakeDamage(Damage);
                }
            }     
        }*/
        yield return new WaitForSeconds(hitBoxTiming);
        isAttacking = false;
        yield return null;
    }
    #endregion

    #region health funcs
    public void TakeDamage(float val) {
        currHealth -= val;
        Debug.Log("health is now " + currHealth.ToString());

        // HPSlider.value = currHealth / maxHealth;

        if (currHealth <= 0) {
            Die();
        }
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
    public void enableUserInput() {
        isSleeping = false;
    }

    public void disableUserInput() {
        isSleeping = true;
        PlayerRB.velocity = Vector2.zero;
    }
    #endregion
}