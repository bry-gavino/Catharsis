using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NumPlayers nPlayers = GameObject.Find("NumPlayers").GetComponent<NumPlayers>();
        if (GameObject.Find("NumPlayers").GetComponent<NumPlayers>().numPlayers == 1) {
            Destroy(GameObject.Find("PlayerStats2"));
            GameObject.Find("PlayerStats1").transform.localPosition = new Vector2(0.0f, 0.0f);
        } else {
            GameObject.Find("PlayerStats1").transform.localPosition = new Vector2(-350.0f, 0.0f);
            GameObject.Find("PlayerStats2").transform.localPosition = new Vector2(350.0f, 0.0f);
        }
        nPlayers.setNumPlayers(1);
        
    }
}
