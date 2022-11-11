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

    public int curPowerID;

    private GameObject player; // cur player object

	// Use this for initialization
	void Start () {
		powerShrine = this;
        curPowerID = 1; // wistfullness selected
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
        curPowerID = powerID;
		for (int i   = 0; i < powerHolderList.Count; i++) {
			PowerInfo holderScript = powerHolderList[i].GetComponent<PowerInfo>();
            powerList[i].selected = false;
            holderScript.P_Image.sprite = powerList[i].unselectedSprite;
			if (holderScript.powerID == powerID) {
				for (int j = 0; j < powerList.Count; j++) {
					if (powerList[j].powerID == powerID) {
                        Debug.Log("Power ID: " + powerID + "and is it selected? " + powerList[j].selected);
                        powerList[j].selected = true;
						if (powerList[j].selected) {
							holderScript.P_Image.sprite = powerList[j].selectedSprite;
                            PowerInfo tempP = getPowerID();
                            Debug.Log("Player gets: " + tempP.GetName + "\nStats:\nPower: " + (1 + tempP.P_Upgrade_1) 
                                + "\nSecondary upgrade: " + (1 + tempP.P_Upgrade_2) + "\nDrawback multiplier: " + (1 + tempP.GetDrawBackPower)
                                + "\nDescription: " + tempP.GetDesc);
							//holderScript.GetDesc.GetComponent<TextMeshProUGUI>().text += "Selected!\n";
						} else {
							holderScript.P_Image.sprite = powerList [j].unselectedSprite;
						}
					}
				}
			}
		}
	}

    public PowerInfo getPowerID()
    {
        return powerHolderList[curPowerID - 1].GetComponent<PowerInfo>();
    }

    public void exitShrine()
    {
        powerShrine.gameObject.SetActive(false);
        player.GetComponent<PlayerController>().enableUserInput();
        Debug.Log("Closing Shrine");
    }

    public void enterShrine(GameObject player)
    {
        Debug.Log("Opening Shrine");
        powerShrine.gameObject.SetActive(true);
        this.player = player;

    }
	// Update is called once per frame
	void Update () {
		
	}
}
