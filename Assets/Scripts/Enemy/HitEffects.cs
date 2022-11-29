using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffects : MonoBehaviour
{

    #region Player
    Transform transform;
    Animator anim;
    float timer;
    #endregion

    # region UnityFunctions
    private void Awake()
    {
        transform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        timer = 0.3f;
    }
    private void Update()
    {
        if (timer <= 0.0f) {
            Destroy(this.gameObject);
        }
        timer -= Time.deltaTime;
    }
    #endregion



    public void HandleDirection(Vector2 currDirection) {
        float deg = 0;
        float rad = Mathf.Atan2(currDirection.x, -currDirection.y);
        deg = rad * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3 (0, 0, deg);
    }
}
