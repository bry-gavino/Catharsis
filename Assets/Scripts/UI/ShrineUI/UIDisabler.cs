using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class UIDisabler : MonoBehaviour {
    private Canvas CanvasObject; // Assign in inspector
 
    void Start()
    {
       CanvasObject = GetComponent<Canvas>();
    }
 
    void Update() 
    {
        if (Input.GetKeyUp(KeyCode.Escape)) //change key 
        {
            CanvasObject.enabled = !CanvasObject.enabled;
        }
    }
 }