using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] [Tooltip("Object to instantiate when die.")]
    private GameObject HurtUIObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void makeHurtUI() {
        Instantiate(HurtUIObject, transform.position, transform.rotation, transform);
    }
}
