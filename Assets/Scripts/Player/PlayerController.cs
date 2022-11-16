using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour {
    #region player sounds
    [SerializeField] [Tooltip("Sound when you dash.")]
    private AudioClip DashFX;
    [SerializeField] [Tooltip("Sound when you attack.")]
    private AudioClip AttackFX;
    [SerializeField] [Tooltip("Sound when you attack2.")]
    private AudioClip Attack2FX;
    [SerializeField] [Tooltip("Sound when you hurt.")]
    private AudioClip HurtFX;
    [SerializeField] [Tooltip("Sound when you level up.")]
    private AudioClip LvlUpFX;
    [SerializeField] [Tooltip("Sound when you charge1 up.")]
    private AudioClip Charge1FX;
    [SerializeField] [Tooltip("Sound when you charge2 up.")]
    private AudioClip Charge2FX;
    [SerializeField] [Tooltip("Sound when you release.")]
    private AudioClip ReleaseFX;

    [SerializeField] [Tooltip("HurtBox Prefab")]
    private GameObject HurtBoxPrefab;
    [SerializeField] [Tooltip("ChargeBox Prefab")]
    private GameObject ChargeBoxPrefab;
    [SerializeField] [Tooltip("LevelUp Prefab")]
    private GameObject LevelUpPrefab;

    private MusicManager musicManager;
    private Slider HPSlider;
    private Slider XPSlider;
    private TextMeshProUGUI LevelTxt;
    #endregion

    #region player info variables
    public int playerID;
    string leftKey;
    string rightKey;
    string upKey;
    string downKey;
    string dashKey;
    string attackKey;
    string healKey;
    string shrineKey; //anthony addition
    bool dead = false;
    bool isSleeping = false;
    public int enemiesDefeated = 0;
    #endregion

    #region xp variables
    float exp = 0;
    float skillPoints = 0;
    public int currLevel = 1;

    float expThreshold;
    #endregion

    #region movement variables
    public float moveSpeed = 8;
    private float baseMoveSpeed;
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
    public float Damage = 1;
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
    int combo = 0; // 0 -> 1 -> 2 -> 3 -> 1/0
    float lenHoldingAttack = 0.0f;
    bool chargingAttack = false;
    bool releasingChargeAttack = false;
    float chargeAttackLength = 0.53f;
    float chargeAttackCooldown = 1.0f;
    int chargeOrder = 1;
    float chargeOrderLength = 0.3f;
    float chargeOrderTimer = 0.0f;
    float chargeSoundTimer = 0.0f;
    #endregion

    #region physics
    Rigidbody2D PlayerRB;
    GameObject HurtBox;
    GameObject ChargeBox;
    GameObject Effects;
    #endregion

    #region animation
    Animator anim;
    #endregion

    #region health
    public float maxHealth;
    public float currHealth;
    // public Slider HPSlider;
    public string[] powers;

    public ArrayList inventory = new ArrayList();
    #endregion

    #region powers
    [SerializeField] [Tooltip("Current power")]
    public PowerInfo curr_power;

    [SerializeField]
    [Tooltip("Shrine to access")]
    public GameObject shrine_obj;

    private GameObject cur_player;
    #endregion

    #region unity funcs
    // called once when object created
    private void Awake() {
        // HurtBox = GameObject.Find("PlayerHurtBox");
        Effects = GameObject.Find("PlayerEffects");
        PlayerRB = GetComponent<Rigidbody2D>();
        baseMoveSpeed = moveSpeed;

        anim = GetComponent<Animator>();
        musicManager = GameObject.Find("GameManager").GetComponent<MusicManager>();

        //Anthony Addition - cur player gameobject for shrine
        cur_player = this.gameObject;

        currHealth = maxHealth;
        if (playerID == 1) {
            leftKey = "a";
            rightKey = "d";
            upKey = "w";
            downKey = "s";

            dashKey = "g";
            attackKey = "f";
            healKey = "r";
            shrineKey = "z"; //Anthony Addition

            HPSlider = GameObject.Find("HealthP1").GetComponent<Slider>();
            XPSlider = GameObject.Find("ExpP1").GetComponent<Slider>();
            LevelTxt = GameObject.Find("LevelP1").GetComponent<TextMeshProUGUI>();
        }
        else if (playerID == 2) {
            leftKey = "k";
            rightKey = ";";
            upKey = "o";
            downKey = "l";

            dashKey = "h";
            attackKey = "j";
            healKey = "u";

            // HPSlider = {GameObject.Find("HealthP2").GetComponent<Slider>()}
            // XPSlider = GameObject.Find("ExpP2").GetComponent<Slider>();
        }
        expThreshold = 10;
        HPSlider.value = currHealth / maxHealth;
        XPSlider.value = exp / expThreshold;
        LevelTxt.text = currLevel.ToString();
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
        chargeOrderTimer -= Time.deltaTime;
        chargeSoundTimer -= Time.deltaTime;
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
            releasingChargeAttack = false;
        } if (attackCooldownTimer <= 0) {
            combo = 0; // resets combo to 0
        } if (chargeOrderTimer <= 0) {
            chargeOrder = 2;
        } if (chargingAttack && chargeSoundTimer <= 0) {
            musicManager.playClip(Charge2FX, 1);
            chargeSoundTimer = 0.55f;
        }
        if (Input.GetKey(attackKey)) {
            lenHoldingAttack += Time.deltaTime;
        } else {
            lenHoldingAttack = 0.0f;
            if (chargingAttack) {
                ReleaseChargeAttack();
            }
        }
    }

    private void HandleInput() {
        if (!isHurt && !isPushed) {
            if (Input.GetKey(attackKey) && attackTimer <= 0) {
                if (combo != 3) {
                    Attack();
                } else if (combo == 3 && attackCooldownTimer <= 0) {
                    Attack();
                }
            } if (Input.GetKey(dashKey) && (dashCooldownTimer <= 0) && !chargingAttack) {
                Dash();
            }
        }
        if (Input.GetKey(shrineKey))
        {
            Debug.Log("Key press detected!");
            shrineActivate();
        }
    }

    private void HandleState() {
        if (isPushed) {
            Effects.GetComponent<PlayerEffects>().SetState(3);
        } else if (releasingChargeAttack) {
            Effects.GetComponent<PlayerEffects>().SetState(12);
        } else if (chargingAttack) {
            if (chargeOrder == 1) {
                Effects.GetComponent<PlayerEffects>().SetState(10);
            } else {
                Effects.GetComponent<PlayerEffects>().SetState(11);
            }
        } else if (isAttacking) {
            if (combo == 1) {
                Effects.GetComponent<PlayerEffects>().SetState(2);
            } else if (combo == 2) {
                Effects.GetComponent<PlayerEffects>().SetState(22);
            } else if (combo == 3) {
                Effects.GetComponent<PlayerEffects>().SetState(23);
            }
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
                PlayerRB.velocity = (fromHurt - new Vector2(transform.position.x, transform.position.y)).normalized * -pushBackStrength;
                Effects.GetComponent<PlayerEffects>().HandleDirection(-PlayerRB.velocity);
            } else if (isHurt) {
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
                } else if (isAttacking && !releasingChargeAttack) {
                    PlayerRB.velocity = oldDirection * (moveSpeed/2);
                } else if (anim.GetBool("Moving")) {
                    PlayerRB.velocity = currDirection * moveSpeed;
                } else {
                    PlayerRB.velocity = Vector2.zero;
                }
                if (chargingAttack) {
                    PlayerRB.velocity = PlayerRB.velocity/3;
                    Debug.Log(PlayerRB.velocity);
                }
                Effects.GetComponent<PlayerEffects>().HandleDirection(currDirection);
            }
        }
        else {
            PlayerRB.velocity = Vector2.zero;
            anim.SetBool("Dead", true);
        }

        // HANDLE HITBOX PLACEMENT HERE
        // HurtBox.GetComponent<PlayerHurtBox>().HandleDirection(currDirection);
    }

    private void Dash() {
        musicManager.playClip(DashFX, 1);
        dashLengthTimer = dashLength;
        dashCooldownTimer = dashCooldown;
        isDashing = true;
    }

    // IEnumerator DashRoutine() {}
    #endregion


    #region attack funcs

    private void Attacking() {
        if (isAttacking && !releasingChargeAttack) {
            
            HurtBox.GetComponent<PlayerHurtBox>().HandleDirection(currDirection); 
        } else if (HurtBox) {
            Destroy(HurtBox);
        }
    }

    private void Attack() {
        if (lenHoldingAttack >= attackLength) { // CHARGED ATTACK
            if (!chargingAttack) {
                chargeOrderTimer = chargeOrderLength;
                chargeOrder = 1;
                musicManager.playClip(Charge1FX, 1);
            }
            chargingAttack = true;
            Debug.Log("CHARGING");
        } else {
            musicManager.playClip(AttackFX, 1);
            if (combo == 3) {
                combo = 1;
            } else {
                combo += 1;
            }
            Debug.Log("combo: "+combo);
            dashLengthTimer = attackLunge; // propels character forward (like a lunge)
            dashCooldownTimer = attackLength;
            attackTimer = attackLength;
            attackCooldownTimer = attackCooldown;
            isDashing = true;
            isAttacking = true;
            HurtBox = Instantiate(HurtBoxPrefab, transform.localPosition, Quaternion.identity, transform);
        }
    }
    private void ReleaseChargeAttack() {
        musicManager.playClip(ReleaseFX, 1);
        chargingAttack = false;
        releasingChargeAttack = true;
        Debug.Log("RELEASE");
        attackTimer = chargeAttackLength;
        attackCooldownTimer = chargeAttackCooldown;
        isAttacking = true;
        chargeOrder = 1;
        // FIX 
        ChargeBox = Instantiate(ChargeBoxPrefab, transform.localPosition, Quaternion.identity, transform);
    }

    public void OnDashTriggerEnter2D(Collider2D col) {
        if(isDashing && !isAttacking) {
            col.gameObject.GetComponent<EnemyScript>().GetHit(Damage/2, PlayerRB.transform, isDashing, playerID, combo);
        }
    } 
    public void OnAttackTriggerEnter2D(Collider2D col) {
        if(isAttacking) {
            float damageCombo = Damage + Damage*((combo-1)/3.0f);
            Debug.Log("damageCombo: "+damageCombo);
            if (releasingChargeAttack) {
                damageCombo = Damage*2;
            }
            col.gameObject.GetComponent<EnemyScript>().GetHit(damageCombo, PlayerRB.transform, isDashing, playerID, combo);
        }
    } 

    public int[] identifyHurtBox() {
        return new int[]{playerID, combo};
    }
    public float getAttackLength() {return attackLength;}
    public float getDashLength() {return dashLength;}

    #endregion

    #region health funcs

    public void TakeDamage(float val, Vector2 from) {
        fromHurt = from;

        if (!isHurt) {
            musicManager.playClip(HurtFX, 1);
            GameObject.Find("UI").GetComponent<UIManager>().makeHurtUI();
            isHurt = true;
            anim.SetBool("Hurt", true);
            hurtCooldownTimer = hurtCooldown;
            currHealth -= val;
            Debug.Log("health is now " + currHealth.ToString());
        }
        if (currHealth <= 0) {
            Die();
        }
        HPSlider.value = currHealth / maxHealth;
        isPushed = true;
        pushBackLengthTimer = pushBackLength;
    }

    private void Die() {
        GameObject.Find("UI").GetComponent<UIManager>().loseGame();
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

            HPSlider.value = currHealth / maxHealth;

            inventory.RemoveAt(inventory.Count - 1);
        }
    }
    #endregion

    #region xp funcs
    public void add_xp(int add) {
        exp += add;
        // level up
        level_up();
        XPSlider.value = exp / expThreshold;
    }
    void level_up() {
        if ((expThreshold - exp) <= 0) {
            Instantiate(LevelUpPrefab, transform.position, transform.rotation, transform);
            musicManager.playClip(LvlUpFX, 1);
            exp = exp - expThreshold;
            skillPoints += 1;
            currLevel += 1;
            expThreshold = expThreshold * 1.5f;

            Damage = Damage + (0.01f + ((100 - currLevel) * 0.001f));
            // Debug.Log("New Damage: "+Damage);
            maxHealth = maxHealth + (0.01f + ((100 - currLevel) * 0.001f));
            // Debug.Log("New maxHealth: "+maxHealth);
            currHealth = currHealth + ((1/3) * maxHealth);
            if (currHealth > maxHealth) {
                currHealth = maxHealth;
            }
            // Debug.Log("New currHealth: "+currHealth);
            HPSlider.value = currHealth / maxHealth;
            LevelTxt.text = currLevel.ToString();
        }
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
        if (PlayerRB != null) {
            PlayerRB.velocity = Vector2.zero;
        }
    }

    #region shrineScript
    public void shrineActivate()
    {
        GameObject lis = GameObject.Find("ShrineShop");
        //disableUserInput();
        Debug.Log(cur_player);
        Debug.Log(lis);
        lis.GetComponent<NEWShrineScript>().enterShrine(cur_player);
        
    }
    /**
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Shrine" )
        {
            //&& Input.GetKey(shrineKey) for later putting
            StartCoroutine(CheckInputRoutine());
            //disableUserInput();
            //other.gameObject.GetComponent<NEWShrineScript>().enterShrine(cur_player);
        }
    }
    public void OnCollisionExit2D(Collision2D other)
    {
        StopCoroutine(CheckInputRoutine());
    }

    IEnumerator CheckInputRoutine(){
    if (Input.GetKey(shrineKey))
        // Or also
        //if(currentChest && Input.GetKeyDown(KeyCode.E)) 
        {
            Debug.Log("Input detected!");
        }
        yield return null;
    }
    */
    #endregion

    

    public void addEnemyDefeated(){
        enemiesDefeated += 1;
    }
    #endregion
}
