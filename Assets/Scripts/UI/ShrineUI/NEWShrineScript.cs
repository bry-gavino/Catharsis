using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NEWShrineScript : MonoBehaviour
{
    public static NEWShrineScript powerShrine;

	public List<Power> powerList = new List<Power>();

	public GameObject powerHolderPrefab;

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
			GameObject holder = Instantiate (powerHolderPrefab, grid, false);
			PowerInfo holderScript = holder.GetComponent<PowerInfo> ();

            holderScript.GetName.GetComponent<TextMeshProUGUI>().text = powerList[i].GetName;
            holderScript.GetDesc.GetComponent<TextMeshProUGUI>().text = powerList[i].powerDesc;
            // Could use for power level decrement: holderScript.GetDesc.GetComponent<TextMeshProUGUI>().text = "$" + powerList[i].powerDesc.ToString("N2");
			holderScript.powerID = powerList[i].powerID;

			holderScript.GetComponent<BuyButton>().powerID = powerList [i].powerID;

			powerHolderList.Add(holder);

			if (powerList[i].selected) {
				holderScript.powerImage.sprite = powerList [i].selectedSprite;

			} else {
				holderScript.powerImage.sprite = powerList [i].unselectedSprite;
			}
		}
	}

	public void UpdateSprite(int powerID) {
		for (int i   = 0; i < powerHolderList.Count; i++) {
			PowerInfo holderScript = powerHolderList[i].GetComponent<PowerInfo> ();
			if (holderScript.powerID == powerID) {
				for (int j = 0; j < powerList.Count; j++) {
					if (powerList [j].powerID == powerID) {
						if (powerList [j].selected) {
							holderScript.powerImage.sprite = powerList[j].selectedSprite;
							holderScript.powerDesc.GetComponent<TextMeshProUGUI>().text += "Selected!\n";
						} else {
							holderScript.powerImage.sprite = powerList [j].unselectedSprite;
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
