using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTester : MonoBehaviour
{
    #region Public Variables
    public float increaser = 0.01f;
    public int limit = 20;
    #endregion
    
    #region Collision Detection
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Consider having the color turn darker as the upgrade increases?
        //Therefore this Upgrader will check for level and update Colors
        //accordingly
        //For implememting a switching powers on Thursday
        if limit > 0:
            PowerInfo power = collision.gameObject.GetComponent<PlayerController>().curr_power;
            power.Set_P_Upgrade_1(0.01f);
            Debug.Log("The Power {" + power.GetName + "} outputs damage modifier {" + (1 + power.P_Upgrade_1) + "}");
            limit--;
        else:
            Debug.Log("You have reached max upgrades with " + power.GetName);
    }
    #endregion
}
