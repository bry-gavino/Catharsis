using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransitionScreen : MonoBehaviour
{
    #region vars
    private float speed = 0.005f;
    public float alpha = 1.0f;
    public CanvasGroup canvasGroup;
    bool fadingIn = true;
    int level = 1;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
        canvasGroup.alpha = alpha;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingIn) {
            fadeIn();
        } else {
            fadeOut();
        }
        Debug.Log("alpha: "+alpha);
    }

    // handle visuals
    private void fadeIn() {
        if (alpha < 1.0f) {
            canvasGroup.alpha += Time.deltaTime;
        } else {
            canvasGroup.alpha = 1.0f;
        }
    }
    private void fadeOut() {
        if (alpha > 0.0f) {
            canvasGroup.alpha -= Time.deltaTime;
        } else {
            canvasGroup.alpha = 0.0f;
        }
    }
    public void isFadingIn(int lvl) {alpha = 0.0f; fadingIn = true; level = lvl; Debug.Log("fading in");}
    public void isFadingOut() {fadingIn = false;}
}
