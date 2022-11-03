using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSelect : MonoBehaviour {

	public int powerID;

	public void SelectPower() {
        Debug.Log("Power Selected!");
        /**
		
		for (int i = 0; i < ShrineScript.powerScript.powerList.Count; i++) {
			if (!ShrineScript.powerScript.powerList [i].bought && ShrineScript.powerScript.powerList [i].powerID == powerID) {
				if (GameManager.gameManager.HasEnough(ShrineScript.powerScript.powerList [i].itemPrice)){
					ShrineScript.powerScript.powerList [i].bought = true;
					GameManager.gameManager.ReduceMoney(ShrineScript.powerScript.powerList [i].itemPrice);
				}
			}
		}
		ShrineScript.powerScript.UpdateSprite (powerID);
        */
	}
}