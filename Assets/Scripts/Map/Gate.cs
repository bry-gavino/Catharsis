using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {
    [Tooltip("Approximate amount of time until gate fades out completely.")] [SerializeField]
    private float fade_out_time = 3f;

    private SpriteRenderer gateSpriteRenderer;

    void Start() {
        gateSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    /**
    * After fade_out_time seconds, this object will be disabled.
    * As time passes, this object becomes more transparent.
    */
    public void FadeOut() {
        Debug.Log("In FadeOut function.");
        StartCoroutine(helper());
    }

    IEnumerator helper() {
        Debug.Log("Helper coroutine in Gate.cs started.");
        Color fade = gateSpriteRenderer.color;
        float time_elapsed = 0f;
        float startAlpha = fade.a;
        float desiredAlpha = 0f;
        while (time_elapsed <= fade_out_time) {
            time_elapsed += Time.deltaTime;
            Debug.Log(time_elapsed + " seconds elapsed.");
            fade.a = Mathf.MoveTowards(startAlpha, desiredAlpha, time_elapsed / fade_out_time);
            gateSpriteRenderer.color = fade;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}