using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpgrade : MonoBehaviour
{
	public int powerID;

	public void UpgradePower() {
        Debug.Log("Power Upgraded!");
        /**
		
		for (int i = 0; i < ShrineScript.powerShrine.powerList.Count; i++) {
			if (!(ShrineScript.powerShrine.powerList[i].maxLevel == ShrineScript.powerShrine.powerList[i].cur_level) && ShrineScript.powerShrine.powerList[i].powerID == powerID) //implement maxLevel 
            {
				if (GameManager.gameManager.HasEnough(ShrineScript.powerShrine.powerList[i].powerReq)){
					ShrineScript.powerShrine.powerList[i].level += 1; //upgrade level
					GameManager.gameManager.ReducePoints(ShrineScript.powerShrine.powerList[i].powerReq);
                    ShrineScript.powerShrine.powerList[i].powerReq += ShrineScript.powerShrine.powerList[i].level * 3 //up the points required to get to that level
				}
			}
		}
		ShrineScript.powerShrine.UpdateSprite(powerID);
        */
	}
}
