using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // TODO:
    // - tag GameManager instance with GameController

    public static GameManager Instance = null;
    public static GameObject levelSceneInstance;

    [Tooltip("Level Transition Screen")] [SerializeField]
    public GameObject levelTransitionScreen;



    #region Variables
    public string floorType = ""; // POST-MVP: curr floor emotion
    public int level = 1;
    // OPTIONS for screenType: "Menu", "InGame", "Cutscene"
    public string screenType = "InGame"; // POST-MVP: implement "Menu" & "Cutscene"
    public int numPlayers = 1; // POST-MVP: 2 player ability
    // POST-MVP: save profiles
    public int bossesEncountered = 0;
    public List<string> abilitiesList = new List<string>();
    public bool isListeningUser;
    float sleepTime = -1.0f;
    float transitionLifeTime = -1.0f;
    bool lvlSceneInst = false;
    public float finTransitionTime = 1.5f;
    public float transitionLifeTimeLength = 6.0f;
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
        Debug.Log("Restart Run from beginning.");
        sleepPlayers(); // Players are initially immobile
        bossesEncountered = 0;
        level = 1;
        floorType = ""; // POST-MVP: default floorType?
        transitionLifeTime = finTransitionTime;

        changeScreenInGame();
        // starts with level 1 and fades
        finishTransition();
    }
    // call this to start new level 
    public void transitionToLevelScreen() {
        Debug.Log("transitionToLevelScreen!");
        sleepPlayers(); // stop players
        level += 1;
        startTransition();
        // TODO: get player object and spawn at spawnPt
        // TODO: make MapGenerator re-generate
        // TODO: loading screen
            // TODO: make player object SLEEP during loading
    }
    public void awakenPlayers() {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in Players) {
            p.GetComponent<PlayerController>().enableUserInput();
        }
        sleepTime = -1.0f;
    }
    public void sleepPlayers() {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in Players) {
            p.GetComponent<PlayerController>().disableUserInput();
        }
    }
    #endregion



    #region Screen_functions
    private void changeScreenInGame(){screenType = "InGame";}
    private void changeScreenMenu(){screenType = "Menu";}
    private void changeScreenCutscene(){screenType = "Cutscene";}
    #endregion



    #region Level_transition
    public void startTransition() {
        // TODO: create LevelTransitionScreen instance
        if (!lvlSceneInst) {
            lvlSceneInst = true;
            levelSceneInstance = Instantiate(levelTransitionScreen);
        }
        // TODO: call LevelTransitionScreen.fadeIn(int lvl)
        levelSceneInstance.GetComponent<LevelTransitionScreen>().isFadingIn(level);
        sleepTime = transitionLifeTimeLength;
    }
    public void finishTransition() {
        if (!lvlSceneInst) {
            lvlSceneInst = true;
            levelSceneInstance = Instantiate(levelTransitionScreen);
        }

        levelSceneInstance.GetComponent<LevelTransitionScreen>().isFadingOut();
        sleepTime = finTransitionTime;
    }
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

        lvlSceneInst = false;
        
        // Instantly loads first level
        restartRun();
    }

    private void Update() 
    {
        if (sleepTime == -1.0f) {
        } else if (sleepTime <= 0.0f) {
            awakenPlayers();
        } else {
            sleepTime -= Time.deltaTime;
        }

        if (transitionLifeTime == -1.0f) {
        } else if (transitionLifeTime <= 0.0f) {
            // Debug.Log("DESTROY");
            Destroy(levelSceneInstance);
            lvlSceneInst = false;
        } else {
            transitionLifeTime -= Time.deltaTime;
        }

        
        // Debug.Log("transitionLifeTime: " + transitionLifeTime);
    }
    #endregion



    #region Scene_transitions
    private void StartGame()
    {
        SceneManager.LoadScene(""); // TODO: PUT LANDING SCENE HERE!!!
    }
    #endregion
}
