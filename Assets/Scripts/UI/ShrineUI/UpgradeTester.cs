using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTester : MonoBehaviour
{
    #region Public Variables
    public float increaser = 0.01f;
    public int limit = 20;
    #endregion

    void Start()
    {
       CanvasObject = GetComponent<Canvas>();
    }

    #region Collision Detection
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player1" and Input.GetKeyUp(KeyCode.Escape)) //change key 
        {
            CanvasObject.enabled = !CanvasObject.enabled;
            collision.   
        }
        /**
        if (limit > 0){
            PowerInfo power = collision.gameObject.GetComponent<PlayerController>().curr_power;
            power.Set_P_Upgrade_1(0.01f);
            Debug.Log("The Power {" + power.GetName + "} outputs damage modifier {" + (1 + power.P_Upgrade_1) + "}");
            limit--;
        }
        else{
            Debug.Log("You have reached max upgrades with " + collision.gameObject.GetComponent<PlayerController>().curr_power.GetName);
        }
        */
    }
    #endregion
}
