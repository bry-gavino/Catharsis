using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSelect : MonoBehaviour {

	public int powerID;

	public void SelectPower() {
        Debug.Log("Power Selected!");
        NEWShrineScript.powerShrine.UpdateSprite(powerID);
        /**
		
		for (int i = 0; i < ShrineScript.powerShrine.powerList.Count; i++) {
			if (!ShrineScript.powerShrine.powerList[i].selected && ShrineScript.powerShrine.powerList[i].powerID == powerID) {
                //give player the power
                //double check if the power levels are saved!!! VERY IMPORTANT
			}
		}
		ShrineScript.powerShrine.UpdateSprite(powerID);
        */
	}
}