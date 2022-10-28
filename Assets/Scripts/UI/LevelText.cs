using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTest : MonoBehaviour
{
    #region vars
    public float alpha = 1.0f;
    bool fadingIn = true;
    public TextMeshPro txt;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        txt = this.GetComponent<TextMeshPro>();
        txt.alpha = alpha;
        txt.text = ("LEVEL 1");
        Debug.Log(txt);
        Debug.Log(txt.alpha);
        Debug.Log(txt.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingIn) {
            fadeIn();
        } else {
            fadeOut();
        }
        // Debug.Log("alpha: "+alpha);
    }

    // handle visuals
    private void fadeIn() {
        if (alpha < 1.0f) {
            txt.alpha += Time.deltaTime;
        } else {
            txt.alpha += 1.0f;
        }
    }
    private void fadeOut() {
        if (alpha > 0.0f) {
            txt.alpha -= Time.deltaTime;
        } else {
            txt.alpha += 0.0f;
        }
    }
    public void isFadingIn(int lvl) {
        alpha = 0.0f; 
        fadingIn = true; 
        txt.text = ("LEVEL"+lvl.ToString());
        Debug.Log("fading in");}
    public void isFadingOut() {alpha = 1.0f; fadingIn = false; Debug.Log("fading out");}
}
