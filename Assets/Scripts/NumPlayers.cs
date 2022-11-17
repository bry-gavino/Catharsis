using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumPlayers : MonoBehaviour
{
    public int numPlayers = 1;
    public int pID;
    public int pLvl;
    public int pMapLvl;
    public int pEnemies;

    public void setNumPlayers(int v) {
        numPlayers = v;
    }

    public void savePlayerStats(int playerID, int currLevel, int level, int enemiesDefeated) {
        pID = playerID;
        pLvl = currLevel;
        pMapLvl = level;
        pEnemies = enemiesDefeated;
    }
}
