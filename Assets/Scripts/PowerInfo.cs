using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerInfo : MonoBehaviour
{
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

}
