using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShrineScript : MonoBehaviour {

	public static ShrineScript powerShrine;

	public List<Power> powerList = new List<Power>();

	public GameObject powerHolderPrefab;

	public Transform grid;

	private List<GameObject> powerHolderList = new List<GameObject> ();

	// Use this for initialization
	void Start () {
		powerShrine = this;
		FillList ();
	}

	void FillList(){
		for(int i = 0; i < powerList.Count; i++) {
			GameObject holder = Instantiate (powerHolderPrefab, grid, false);
			powerHolder holderScript = holder.GetComponent<powerHolder> ();

            holderScript.powerName.GetComponent<TextMeshProUGUI>().text = powerList[i].powerName;
            holderScript.powerDesc.GetComponent<TextMeshProUGUI>().text = "$" + powerList[i].powerDesc.ToString("N2");
			holderScript.powerID = powerList [i].powerID;

			holderScript.GetComponent<BuyButton> ().powerID = powerList [i].powerID;

			powerHolderList.Add (holder);

			if (powerList [i].bought) {
				holderScript.powerImage.sprite = powerList [i].boughtSprite;

			} else {
				holderScript.powerImage.sprite = powerList [i].unboughtSprite;
			}
		}
	}

	public void UpdateSprite(int powerID) {
		for (int i = 0; i < powerHolderList.Count; i++) {
			powerHolder holderScript = powerHolderList [i].GetComponent<powerHolder> ();
			if (holderScript.powerID == powerID) {
				for (int j = 0; j < powerList.Count; j++) {
					if (powerList [j].powerID == powerID) {
						if (powerList [j].bought) {
							holderScript.powerImage.sprite = powerList [j].boughtSprite;
							holderScript.powerDesc.GetComponent<TextMeshProUGUI>().text = "Sold Out!";
						} else {
							holderScript.powerImage.sprite = powerList [j].unboughtSprite;
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
