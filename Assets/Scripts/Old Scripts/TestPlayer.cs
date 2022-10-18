using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    #region Movment_variables
    public float movespeed;
    float x_input;
    float y_input;
    bool dead = false;
    #endregion

    #region Physics_components
    Rigidbody2D PlayerRB;
    #endregion

    #region Interact_variables
    Vector2 currDirection;
    #endregion

    #region Animation_components
    Animator anim;
    #endregion

    [SerializeField]
    [Tooltip("Current power")]
    public PowerInfo curr_power;

    #region Unity_functions
    private void Awake()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");

        Move();
    }
    #endregion

    #region Movment_functions
    private void Move()
    {
        if (!dead)
        {
            anim.SetBool("Moving", true);

            if (x_input > 0) {
                PlayerRB.velocity = Vector2.right * movespeed;
                currDirection = Vector2.right;
            } else if (x_input < 0) {
                PlayerRB.velocity = Vector2.left * movespeed;
                currDirection = Vector2.left;
            } else if (y_input > 0) {
                PlayerRB.velocity = Vector2.up * movespeed;
                currDirection = Vector2.up;
            } else if (y_input < 0) {
                PlayerRB.velocity = Vector2.down * movespeed;
                currDirection = Vector2.down;
            } else {
                PlayerRB.velocity = Vector2.zero;
                anim.SetBool("Moving", false);
            }

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

    public void changePower(PowerInfo power)
    {
        curr_power = power;
    }
}
