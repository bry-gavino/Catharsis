using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLDShrineScript : MonoBehaviour
{
    #region Editor Variables
    public PowerInfo power;
    #endregion 

    #region Collision Detection
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("You have Collided! Your new power is:" + power.GetName +" and it deals " + (1 + power.P_Upgrade_1));
        
        //For implememting a switching powers on Thursday
        PowerInfo temppower = collision.gameObject.GetComponent<PlayerController>().curr_power;
        collision.gameObject.GetComponent<PlayerController>().changePower(power);
        power = temppower;
    }
    #endregion
}
