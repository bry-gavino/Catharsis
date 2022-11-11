using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void MoveToScene(int sceneID) {
        SceneManager.LoadScene(sceneID);
        if (sceneID == 1) {
            GameObject.Find("GameManager").GetComponent<GameManager>().restartRun();
            // GameObject.Find("GameManager").GetComponent<MusicManager>().ReplayMusicLoop();
        } else {
            // stop music
            // GameObject.Find("GameManager").GetComponent<MusicManager>().StopMusicLoop();
        }
        
    }
}
