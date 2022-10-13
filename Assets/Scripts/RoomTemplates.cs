using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour {
    public GameObject[] northRooms;
    public GameObject[] southRooms;
    public GameObject[] eastRooms;
    public GameObject[] westRooms;

    public GameObject bigWall;
    public GameObject bigFloor;

    public List<GameObject> rooms;

    public float waitTime; // hardcoded for now, but might want to make event based. Like when list stops growing.
    private bool spawnedBoss;
    public GameObject boss;

    [Tooltip("Hardcoded cap on rooms until we finish every room")] [SerializeField]
    public int maxRoomsAllowed = 10;

    void Start() {
        spawnedBoss = false;
    }

    void Update() {
        if (waitTime <= 0 && spawnedBoss == false) {
            if (boss != null) {
                // spawn boss after all rooms spawned
                int lastIndex = rooms.Count - 1;
                Instantiate(boss, rooms[lastIndex].transform.position, Quaternion.identity);
            }

            spawnedBoss = true;
        }
        else {
            waitTime -= Time.deltaTime;
        }
    }
}