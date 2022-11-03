using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerInfo : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("Power ID")]
    public int powerID;

    [SerializeField]
    [Tooltip("Power Name")]
    private GameObject p_name;
    public GameObject GetName
    {
        get
        {
            return p_name;
        }
        set
        {
            p_name = value;
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
        set
        {
            p_upgrade_1 = value;
        }
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
        set
        {
            p_upgrade_2 = value;
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
        set
        {
            p_color = value;
        }
    }

    [SerializeField]
    [Tooltip("Description of ability")]
    private GameObject p_desc;
    public GameObject GetDesc
    {
        get
        {
            return p_desc;
        }
        set
        {
            p_desc = value;
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
        set
        {
            p_drawpower = value;
        }
    }

    [SerializeField]
    [Tooltip("Image of power")]
    private Image powerImage;
    public Image P_Image
    {
        get
        {
            return powerImage;
        }
    }

    public void SetDrawBackPower(float increase)
    {
        p_drawpower += increase;
    }
    #endregion

}
