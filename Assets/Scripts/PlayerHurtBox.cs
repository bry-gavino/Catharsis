using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtBox : MonoBehaviour
{

    #region Player
    Transform transform;
    #endregion

    private List<GameObject> collisions;



    # region UnityFunctions
    private void Awake()
    {
        transform = GetComponent<Transform>();
        collisions = new List<GameObject>();
    }
    private void Update()
    {

    }
    #endregion



    public void HandleDirection(Vector2 currDirection) {
        transform.localPosition = currDirection / 2;
    }

    public void HurtAll(float val, Transform from) {
        Collider[] hitColliders = Physics.OverlapBox(
            gameObject.transform.position, transform.localScale, Quaternion.identity);
        foreach (Collider col in hitColliders) {
            col.gameObject.GetComponent<EnemyScript>().GetHit(val, from, false);
        }
    }
}
