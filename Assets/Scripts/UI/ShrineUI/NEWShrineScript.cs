using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NEWShrineScript : MonoBehaviour
{
    public static NEWShrineScript powerShrine;

	public List<Power> powerList = new List<Power>();

	public GameObject powerPrefab; //power prefab to base powers off of

	public Transform grid;

	private List<GameObject> powerHolderList = new List<GameObject>();

	// Use this for initialization
	void Start () {
		powerShrine = this;
		FillList ();
        // added for disabling Shrine Script at start
        //myDialogBalloon.gameObject.SetActive(false);

	}

	void FillList(){
		for(int i = 0; i < powerList.Count; i++) {
			GameObject holder = Instantiate(powerPrefab, grid, false);
			PowerInfo holderScript = holder.GetComponent<PowerInfo>();

            holderScript.GetName.GetComponent<TextMeshProUGUI>().text = powerList[i].powerName;
            holderScript.GetDesc.GetComponent<TextMeshProUGUI>().text = powerList[i].powerDesc;
            // Could use for power level decrement: holderScript.GetDesc.GetComponent<TextMeshProUGUI>().text = "$" + powerList[i].powerDesc.ToString("N2");
			holderScript.powerID = powerList[i].powerID;

			holderScript.GetComponent<PowerSelect>().powerID = powerList[i].powerID;
            //holderScript.GetComponent<PowerUpgrade>().powerID = powerList[i].powerID;

			powerHolderList.Add(holder);

			if (powerList[i].selected) 
            {
				holderScript.P_Image.sprite = powerList[i].selectedSprite;
			}
            else 
            {
				holderScript.P_Image.sprite = powerList[i].unselectedSprite;
			}
		}
	}

	public void UpdateSprite(int powerID) {
		for (int i   = 0; i < powerHolderList.Count; i++) {
			PowerInfo holderScript = powerHolderList[i].GetComponent<PowerInfo>();
			if (holderScript.powerID == powerID) {
				for (int j = 0; j < powerList.Count; j++) {
					if (powerList [j].powerID == powerID) {
						if (powerList [j].selected) {
							holderScript.P_Image.sprite = powerList[j].selectedSprite;
							holderScript.GetDesc.GetComponent<TextMeshProUGUI>().text += "Selected!\n";
						} else {
							holderScript.P_Image.sprite = powerList [j].unselectedSprite;
						}
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
