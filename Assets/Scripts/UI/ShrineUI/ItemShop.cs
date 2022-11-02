using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemShop : MonoBehaviour {

	public static ItemShop itemShop;

	public List<Item> itemList = new List<Item>();

	public GameObject itemHolderPrefab;

	public Transform grid;

	private List<GameObject> itemHolderList = new List<GameObject> ();

	// Use this for initialization
	void Start () {
		itemShop = this;
		FillList ();
	}

	void FillList(){
		for(int i = 0; i < itemList.Count; i++) {
			GameObject holder = Instantiate (itemHolderPrefab, grid, false);
			ItemHolder holderScript = holder.GetComponent<ItemHolder> ();

            holderScript.itemName.GetComponent<TextMeshProUGUI>().text = itemList[i].itemName;
            holderScript.itemPrice.GetComponent<TextMeshProUGUI>().text = "$" + itemList[i].itemPrice.ToString("N2");
			holderScript.itemID = itemList [i].itemID;

			holderScript.GetComponent<BuyButton> ().itemID = itemList [i].itemID;

			itemHolderList.Add (holder);

			if (itemList [i].bought) {
				holderScript.itemImage.sprite = itemList [i].boughtSprite;

			} else {
				holderScript.itemImage.sprite = itemList [i].unboughtSprite;
			}
		}
	}

	public void UpdateSprite(int itemID) {
		for (int i = 0; i < itemHolderList.Count; i++) {
			ItemHolder holderScript = itemHolderList [i].GetComponent<ItemHolder> ();
			if (holderScript.itemID == itemID) {
				for (int j = 0; j < itemList.Count; j++) {
					if (itemList [j].itemID == itemID) {
						if (itemList [j].bought) {
							holderScript.itemImage.sprite = itemList [j].boughtSprite;
							holderScript.itemPrice.GetComponent<TextMeshProUGUI>().text = "Sold Out!";
						} else {
							holderScript.itemImage.sprite = itemList [j].unboughtSprite;
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