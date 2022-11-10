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

    // Start is called before the first frame update
    void Start()
    {
        txtComponent = (GetComponentInChildren(typeof(TextMeshProUGUI)) as TextMeshProUGUI);
        pController = GameObject.Find("TestPlayer").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        txtComponent.text = "PLAYER 1" 
                            + "\nPLAYER LEVEL: " + pController.currLevel
                            + "\nMAP LEVEL: " + gameManager.level
                            + "\nENEMIES DEFEATED: " + pController.enemiesDefeated;
        
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
