using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] [Tooltip("Object to instantiate when die.")]
    private GameObject HurtUIObject;
    [SerializeField] [Tooltip("Object to instantiate when die.")]
    private GameObject LoseScreenObject;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("NumPlayers").GetComponent<NumPlayers>().numPlayers == 1) {
            Destroy(GameObject.Find("ExpP2"));
            Destroy(GameObject.Find("HealthP2"));
            Destroy(GameObject.Find("LevelP2"));
            Destroy(GameObject.Find("LVL2"));
            Destroy(GameObject.Find("Player2"));
        }
    }

    public void makeHurtUI() {
        Instantiate(HurtUIObject, Vector2.zero, transform.rotation, transform);
    }

    public void loseGame() {
        Instantiate(LoseScreenObject, transform.localPosition, transform.rotation, transform);
    }

}
