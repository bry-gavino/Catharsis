using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playertest : MonoBehaviour
{
    public static int PlayerHealth = 5;
    
    #region Movement_variables
    public float movespeed;
    float x_input;
    float y_input;
    #endregion

    #region Physics_components
    Rigidbody2D PlayerRB;
    #endregion

    #region Attack_variables
    public float Damage;
    public float attackspeed = 1;
    float attackTimer;
    public float hitboxtiming;
    public float endanimationtiming;
    bool isAttacking;
    Vector2 currDirection;
    #endregion

    #region Animation_components
    Animator anim;
    #endregion

    #region Health_variables
    public float maxHealth;
    public float currHealth;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        PlayerRB = GetComponent<Rigidbody2D>();

        attackTimer = 0;

        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");
        Move();
    }
        #region Movement_functions
    private void Move(){
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
            PlayerRB.velocity = Vector2.right *  movespeed;
            currDirection = Vector2.right;
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow)){
            PlayerRB.velocity = Vector2.left * movespeed;
            currDirection = Vector2.left;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow)){
            PlayerRB.velocity = Vector2.up *  movespeed;
            currDirection = Vector2.up;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow)){
            PlayerRB.velocity = Vector2.down * movespeed;
            currDirection = Vector2.down;
        }
        else{
            PlayerRB.velocity = Vector2.zero;
        }
    }
    #endregion
}
