using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{

    public void ExitUI()
    {
        Debug.Log("Exiting Shrine.");
        NEWShrineScript.powerShrine.exitShrine();
    }
}
