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

    //In order to add up percentages, you do 1 + p_upgrade_1 * <attribute-to-modify>
    [SerializeField]
    [Tooltip("Upgrades percentage increase/decrease #1")]
    private float p_upgrade_1;
    public float P_Upgrade_1
    {
        get
        {
            return p_upgrade_1;
        }
    }
    public void Set_P_Upgrade_1(float increase)
    {
        p_upgrade_1 += increase;
    }

    //In order to add up percentages, you do 1 + p_upgrade_2 * <attribute-to-modify>
    [SerializeField]
    [Tooltip("Upgrades percentage increase/decrease #2")]
    private float p_upgrade_2;
    public float P_Upgrade_2
    {
        get
        {
            return p_upgrade_2;
        }
    }
    public void Set_P_Upgrade_2(float increase)
    {
        p_upgrade_2 += increase;
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
    public void SetDrawBackPower(float increase)
    {
        p_drawpower += increase;
    }
    #endregion

}
