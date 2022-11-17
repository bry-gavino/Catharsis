using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public TextMeshProUGUI txtComponent;
    public PlayerController pController;
    public GameManager gameManager;
    public int PlayerID = 1;

    // Start is called before the first frame update
    void Start()
    {
        txtComponent = (GetComponentInChildren(typeof(TextMeshProUGUI)) as TextMeshProUGUI);
        if (PlayerID == 1) {
            if (GameObject.Find("Player1") == null) {
                NumPlayers nPlayers = GameObject.Find("NumPlayers").GetComponent<NumPlayers>(); 
                txtComponent.text = "PLAYER 1" 
                                    + "\nPLAYER LEVEL: " + nPlayers.pLvl
                                    + "\nMAP LEVEL: " + nPlayers.pMapLvl
                                    + "\nENEMIES DEFEATED: " + nPlayers.pEnemies;  
            } else {
                pController = GameObject.Find("Player1").GetComponent<PlayerController>(); 
                gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
                txtComponent.text = "PLAYER 1" 
                                    + "\nPLAYER LEVEL: " + pController.currLevel
                                    + "\nMAP LEVEL: " + gameManager.level
                                    + "\nENEMIES DEFEATED: " + pController.enemiesDefeated;  
            }

        } else {
           if (GameObject.Find("Player2") == null) {
                NumPlayers nPlayers = GameObject.Find("NumPlayers").GetComponent<NumPlayers>(); 
                txtComponent.text = "PLAYER 2" 
                                    + "\nPLAYER LEVEL: " + nPlayers.pLvl
                                    + "\nMAP LEVEL: " + nPlayers.pMapLvl
                                    + "\nENEMIES DEFEATED: " + nPlayers.pEnemies;  
            } else {
                pController = GameObject.Find("Player2").GetComponent<PlayerController>(); 
                gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
                txtComponent.text = "PLAYER 2" 
                                    + "\nPLAYER LEVEL: " + pController.currLevel
                                    + "\nMAP LEVEL: " + gameManager.level
                                    + "\nENEMIES DEFEATED: " + pController.enemiesDefeated;  
            }
        }
        
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
