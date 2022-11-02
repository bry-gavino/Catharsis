using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{
    public static GameObject character;
    public static GameObject title;
    public static GameObject PlayButton;

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

    // Start is called before the first frame update
    void Start()
    {
        // character = GameObject.Find("character");
        // title = GameObject.Find("title");
        // PlayButton = GameObject.Find("PlayButton");
        
        // character.transform.position = characterSP;
        // title.transform.position = titleSP;
        // PlayButton.transform.position = PlayButtonSP;
    }

    // Update is called once per frame
    void Update()
    {
        // if (!characterDone) {
        //     updateCharacter();
        // }
    }

    // sub-update
    private void updateCharacter() {
        while (character.transform.position != characterEP) {
            character.GetComponent<Rigidbody2D>().velocity = (characterEP - character.transform.position)*speed;
        }
    }
}
