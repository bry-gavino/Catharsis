using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingVial : MonoBehaviour
{

    #region Editor Variables
    [SerializeField]
    [Tooltip("The amount of health this specific vial contains")]
    private int v_health;
    public int GetHealth
    {
        get
        {
            return v_health;
        }
    }
    #endregion
}
