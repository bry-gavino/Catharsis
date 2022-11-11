using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Power
{

    public string powerName;
    public string powerDesc;
    public int powerID;

    public Sprite unselectedSprite;
    public Sprite selectedSprite;

    public int maxLevel;
    public bool selected;
    
}