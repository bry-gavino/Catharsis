using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineScript : MonoBehaviour
{
    #region Editor Variables
    public PowerInfo power;
    #endregion 

    #region Collision Detection
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("You have Collided! Your new power is:" + power.GetName +" and it deals " + power.P_Dmg);
        
        //For implememting a switching powers on Thursday
        PowerInfo temppower = collision.gameObject.GetComponent<TestPlayer>().cur_power;
        collision.gameObject.GetComponent<TestPlayer>().changePower(power);
        power = temppower;
    }
    #endregion
}
