using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartAnimation : MonoBehaviour
{
    public static GameObject character;
    public static GameObject title;
    public static GameObject PlayButton;
    public static GameObject HowToPlayScreen;
    public static GameObject CreditsScreen;

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
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("character");
        title = GameObject.Find("title");
        // PlayButton = GameObject.Find("PlayButton");
        HowToPlayScreen = GameObject.Find("HowToPlayScreen");
        CreditsScreen = GameObject.Find("CreditsScreen");
        
        character.transform.position = characterEP;
        title.transform.position = titleEP;
        // PlayButton.transform.position = PlayButtonEP;
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
        HowToPlayScreen.transform.localPosition = new Vector2(0.0f, 0.0f);
        HowToPlayOn = true;
    }
    public void hideHowToPlay() {
        HowToPlayScreen.transform.localPosition = new Vector2(0.0f, -1000.0f);
        HowToPlayOn = false;
    }

    public void showCredits() {
        CreditsScreen.transform.localPosition = new Vector2(0.0f, 0.0f);
        CreditsOn = true;
    }
    public void hideCredits() {
        CreditsScreen.transform.localPosition = new Vector2(0.0f, -1000.0f);
        CreditsOn = false;
    }
}
