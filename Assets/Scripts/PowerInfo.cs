using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerInfo : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("Power Name")]
    private string p_name;
    public string GetName
    {
        get
        {
            return p_name;
        }
    }
    [SerializeField]
    [Tooltip("Damage Power puts out")]
    private float p_dmg;
    public float P_Dmg
    {
        get
        {
            return p_dmg;
        }
    }

    [SerializeField]
    [Tooltip("Color of ability")]
    private Color p_color;
    public Color GetPowerColor
    {
        get
        {
            return p_color;
        }
    }

    [SerializeField]
    [Tooltip("Description of ability")]
    private string p_desc;
    public string GetDesc
    {
        get
        {
            return p_desc;
        }
    }

    [SerializeField]
    [Tooltip("Drawback of power ,,, to be implemented laer")]
    //Consider this as a possible multiplier and check for
    //tag and/or name. If matches the name, matches the drawback of power
    //ex: could be a health drain. If mutliplier is high, drains
    //much faster. if slows player, multiplier is gravity modifier.
    private float p_drawpower;
    public float GetDrawBackPower
    {
        get
        {
            return p_drawpower;
        }
    }
    #endregion

}
