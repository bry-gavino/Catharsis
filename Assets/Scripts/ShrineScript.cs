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
    }
    #endregion
}
