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
    public static GameObject mapGeneratorInstance;
    //public static GameObject soundManagerInstance;

    [Tooltip("Level Transition Screen")] [SerializeField]
    public GameObject levelTransitionScreen;

    [Tooltip("Map Generator")] [SerializeField]
    public GameObject mapGenerator;



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
    public float finTransitionTime = 1.0f;
    public float transitionLifeTimeLength = 3.0f;
    bool finishingTransition = false;
    bool setupLevel = false;
    public Vector2 mapSize = new Vector2(2, 3);
    #endregion



    #region LevelVars
    private Vector3 spawnPt = Vector3.zero;
    #endregion



    #region Variable_functions
    public void advanceLevel(){level += 1;}
    public int getLevel(){return level;}
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
        mapGeneratorInstance = Instantiate(mapGenerator);
        mapGeneratorInstance.GetComponent<DungeonGenerator>().size = mapSize;
        sleepPlayers(); // Players are initially immobile
        bossesEncountered = 0;
        level = 1;
        floorType = ""; // POST-MVP: default floorType?
        transitionLifeTime = finTransitionTime;
        sleepTime = finTransitionTime;

        changeScreenInGame();
        finishTransition();
        //soundManagerInstance.GetComponent<SoundManager>().playMusic(); // starts OST
    }
    // call this to start new level 
    public void transitionToLevelScreen() {
        sleepPlayers(); // stop players
        startTransition();
        setupLevel = true; // notifies transitionLifeTime timer to set up level
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
    private void doSetupLevel() {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in Players) {
            p.transform.position = spawnPt;
        }
        // MapGenerator re-generate
        GameObject MapGen = GameObject.FindWithTag("MapGenerator");
        Destroy(MapGen);
        mapGeneratorInstance = Instantiate(mapGenerator);
        mapGeneratorInstance.GetComponent<DungeonGenerator>().size = new Vector2(2+level, 3);
        // TODO: loading screen
    }
    #endregion



    #region Screen_functions
    private void changeScreenInGame(){screenType = "InGame";}
    private void changeScreenMenu(){screenType = "Menu";}
    private void changeScreenCutscene(){screenType = "Cutscene";}
    #endregion



    #region Level_transition
    public void startTransition() {
        if (!lvlSceneInst) {
            lvlSceneInst = true;
            levelSceneInstance = Instantiate(levelTransitionScreen);
        }
        advanceLevel();
        levelSceneInstance.GetComponent<LevelTransitionScreen>().isFadingIn(level);
        sleepTime = transitionLifeTimeLength;
        transitionLifeTime = transitionLifeTimeLength;
    }
    public void finishTransition() {
        finishingTransition = true;
        if (!lvlSceneInst) {
            lvlSceneInst = true;
            levelSceneInstance = Instantiate(levelTransitionScreen);
        }
        levelSceneInstance.GetComponent<LevelTransitionScreen>().isFadingOut();
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
        //soundManagerInstance = GameObject.Find("SoundManager");

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
            Destroy(levelSceneInstance);
            lvlSceneInst = false;
            transitionLifeTime = -1.0f;
            finishingTransition = false;
        } else if ((transitionLifeTime <= finTransitionTime) && !finishingTransition) {
            finishTransition();
            transitionLifeTime -= Time.deltaTime;
        } else if ((transitionLifeTime <= (transitionLifeTimeLength - finTransitionTime)) && setupLevel) {
            setupLevel = false;
            transitionLifeTime -= Time.deltaTime;
            doSetupLevel();
        } else {
            transitionLifeTime -= Time.deltaTime;
        }
    }
    #endregion



    #region Scene_transitions
    private void StartGame()
    {
        SceneManager.LoadScene(""); // TODO: PUT LANDING SCENE HERE!!!
    }
    #endregion
}
