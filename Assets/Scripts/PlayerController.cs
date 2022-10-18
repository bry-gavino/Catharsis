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
    string attackKey;
    string healKey;
    bool dead = false;
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

    #region attack variables
    public float Damage;
    public float attackSpeed = 1;
    float attackTimer;
    public float hitBoxTiming;
    public float endAnimationTiming;
    public float speedUpTiming;
    bool isAttacking;
    Vector2 currDirection;
    #endregion

    #region physics
    Rigidbody2D PlayerRB;
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
        PlayerRB = GetComponent<Rigidbody2D>();
        attackTimer = 0;

        // TODO: make Animator
        anim = GetComponent<Animator>();

        // currSpeed = moveSpeed;
        currHealth = maxHealth;
        if (playerID == 1) {

            leftKey = "a";
            rightKey = "d";
            upKey = "w";
            downKey = "s";

            attackKey = "f";
            healKey = "g";
        } else if (playerID == 2) {

            leftKey = "k";
            rightKey = ";";
            upKey = "o";
            downKey = "l";
            
            attackKey = "h";
            healKey = "j";
        }
        expThreshold = exp * 100;
        // HPSlider.value = currHealth / maxHealth;
        // XPSlider.value = exp;
    }

    // called once per frame
    private void Update() {
        if (isAttacking) {
            return;
        }
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");

        Move();

        if (Input.GetKey(attackKey) && attackTimer <= 0) {
            Attack();
        } else {
            attackTimer -= Time.deltaTime;
        }
    }
    #endregion 

    #region movement funcs

    private void Move() {
        if (!dead)
        {
            anim.SetBool("Moving", true);

            // HANDLE MOVEMENT HERE
            PlayerRB.velocity = Vector2.zero;
            if (Input.GetKey(leftKey) || Input.GetKey(rightKey) || Input.GetKey(upKey) || Input.GetKey(downKey)) {    
                if (Input.GetKey(rightKey)) { // EAST
                    PlayerRB.velocity += Vector2.right;
                    currDirection = Vector2.right;
                } if (Input.GetKey(leftKey)) { // WEST
                    PlayerRB.velocity += Vector2.left;
                    currDirection = Vector2.left;
                } if (Input.GetKey(upKey)) { // NORTH
                    PlayerRB.velocity += Vector2.up;
                    currDirection = Vector2.up;
                } if (Input.GetKey(downKey)) { // SOUTH
                    PlayerRB.velocity += Vector2.down;
                    currDirection = Vector2.down;
                }
            } else {
                anim.SetBool("Moving", false);
            }
            PlayerRB.velocity = PlayerRB.velocity * moveSpeed;

            anim.SetFloat("DirX", currDirection.x);
            anim.SetFloat("DirY", currDirection.y);
        }
        else
        {
            PlayerRB.velocity = Vector2.zero;
            anim.SetBool("Dead", true);
        }
    }

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
        Debug.Log("casting hitbox now");
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

    
}
