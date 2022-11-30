using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartAnimation : MonoBehaviour
{
    [SerializeField] [Tooltip("Button")]
    private AudioClip ButtonFX;
    
    public static GameObject character;
    public static GameObject title;
    public static GameObject PlayButton;
    public static GameObject HowToPlayScreen;
    public static GameObject CreditsScreen;
    public static GameObject NumPlayersScreen;
    public static GameObject Player1;
    public static GameObject Player2;
    public static NumPlayers NumPlayers;
    private MusicManager musicManager;
    public TextMeshProUGUI buttonTxt;

    #region StartPos
    private static Vector3 StartAnimationSP = new Vector3(960.0f, 540.0f, 0.0f);
    private Vector3 characterSP = new Vector3(-975.0f, -541.1652f, 0.0f) + StartAnimationSP;
    private Vector3 titleSP = new Vector3(-951.6f, -531.8f, 0.0f) + StartAnimationSP;
    private Vector3 PlayButtonSP = new Vector3(489.0f, -267.0f, 0.0f) + StartAnimationSP;
    #endregion

    #region EndPos
    private Vector3 characterEP = new Vector3(-964.2525f, -541.1652f, 0.0f) + StartAnimationSP;
    private Vector3 titleEP = new Vector3(-955.76f, -538.09f, 0.0f) + StartAnimationSP;
    private Vector3 PlayButtonEP = new Vector3(489.0f, -267.0f, 0.0f) + StartAnimationSP;
    #endregion

    #region Done
    private bool characterDone = false;
    private bool titleDone = false;
    private bool PlayButtonDone = false;
    #endregion

    #region Move
    private float speed = 1.0f;
    #endregion

    #region Screens
    bool HowToPlayOn = false;
    bool CreditsOn = false;
    bool PlayersOn = false;
    #endregion

    private int numPlayers = 1;

    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("character");
        title = GameObject.Find("title");
        // PlayButton = GameObject.Find("PlayButton");
        HowToPlayScreen = GameObject.Find("HowToPlayScreen");
        CreditsScreen = GameObject.Find("CreditsScreen");
        NumPlayersScreen = GameObject.Find("NumPlayersScreen");
        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");
        NumPlayers = GameObject.Find("NumPlayers").GetComponent<NumPlayers>();
        
        // character.transform.position = characterEP;
        // title.transform.position = titleEP;
        // PlayButton.transform.position = PlayButtonEP;
        musicManager = GameObject.Find("GameManager").GetComponent<MusicManager>();
        buttonTxt = GameObject.Find("NumPlayersTxt").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (!characterDone) {
        //     updateCharacter();
        // }
        if (Input.GetMouseButtonDown(0)) {
            if (HowToPlayOn) {
            hideHowToPlay();
            }
            if (CreditsOn) {
            hideCredits();
            }
        }
    }

    // sub-update
    private void updateCharacter() {
        while (character.transform.position != characterEP) {
            character.GetComponent<Rigidbody2D>().velocity = (characterEP - character.transform.position)*speed;
        }
    }

    public void showHowToPlay() {
        musicManager.playClip(ButtonFX, 1);
        HowToPlayScreen.transform.localPosition = new Vector2(0.0f, 0.0f);
        HowToPlayOn = true;
    }
    public void hideHowToPlay() {
        HowToPlayScreen.transform.localPosition = new Vector2(0.0f, -1000.0f);
        HowToPlayOn = false;
    }

    public void showCredits() {
        musicManager.playClip(ButtonFX, 1);
        CreditsScreen.transform.localPosition = new Vector2(0.0f, 0.0f);
        CreditsOn = true;
    }
    public void hideCredits() {
        CreditsScreen.transform.localPosition = new Vector2(0.0f, -1000.0f);
        CreditsOn = false;
    }

    public void showPlayers() {
        musicManager.playClip(ButtonFX, 1);
        NumPlayersScreen.transform.localPosition = new Vector2(0.0f, 0.0f);
        PlayersOn = true;
    }
    public void hidePlayers() {
        NumPlayersScreen.transform.localPosition = new Vector2(0.0f, -1000.0f);
        PlayersOn = false;
    }

    public void addPlayer() {
        if (numPlayers == 1) {
            NumPlayers.setNumPlayers(2);
            Player1.transform.localPosition = new Vector2(-300.0f, 0.0f);
            Player2.transform.localPosition = new Vector2(300.0f, 0.0f);
            numPlayers += 1;
            buttonTxt.text = "SUB PLAYER";
        } else {
            NumPlayers.setNumPlayers(1);
            Player1.transform.localPosition = new Vector2(0.0f, 0.0f);
            Player2.transform.localPosition = new Vector2(0.0f, -1000.0f);
            numPlayers -= 1;
            buttonTxt.text = "ADD PLAYER";
        }
    }

}
