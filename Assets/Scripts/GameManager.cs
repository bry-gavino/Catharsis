using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // TODO:
    // - tag GameManager instance with GameController

    public static GameManager Instance = null;



    #region Variables
    public string floorType = ""; // POST-MVP: curr floor emotion
    public int level = 0;
    // OPTIONS for screenType: "Menu", "InGame", "Cutscene"
    public string screenType = "InGame"; // POST-MVP: implement "Menu" & "Cutscene"
    public int numPlayers = 1; // POST-MVP: 2 player ability
    // POST-MVP: save profiles
    public int bossesEncountered = 0;
    public List<string> abilitiesList = new List<string>();
    #endregion



    #region LevelVars
    private Vector2 spawnPt = Vector2.zero;
    #endregion



    #region Variable_functions
    public void advanceLevel(){level += 1;}
    public void beatBoss(){bossesEncountered += 1;}
    public void gainAbility(string ability){abilitiesList.Add(ability);}
    public void addPlayer()
    {
        if (numPlayers > 1) {
            return;
        } else {
            numPlayers += 1;
        }
        // POST-MVP: add anything else needed
    }
    public void subtractPlayer()
    {
        if (numPlayers == 1) {
            return;
        } else {
            numPlayers -= 1;
        }
        // POST-MVP: add anything else needed
    }
    public void restartRun()
    {
        bossesEncountered = 0;
        level = 0;
        floorType = ""; // POST-MVP: default floorType?

        // TODO: get player object and spawn at spawnPt
        // TODO: make MapGenerator re-generate
        // TODO: loading screen
            // TODO: make player object SLEEP during loading

        changeScreenInGame();
    }
    #endregion



    #region Screen_functions
    private void changeScreenInGame(){screenType = "InGame";}
    private void changeScreenMenu(){screenType = "Menu";}
    private void changeScreenCutscene(){screenType = "Cutscene";}
    #endregion



    #region Unity_functions
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
        
        // Instantly loads first level
        restartRun();
    }
    #endregion



    #region Scene_transitions
    private void StartGame()
    {
        SceneManager.LoadScene(""); // TODO: PUT LANDING SCENE HERE!!!
    }
    #endregion
}
