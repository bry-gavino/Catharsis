using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour {
    [Tooltip("Where the blocking walls will be: North, South, East, West.")] [SerializeField]
    public GameObject[] walls;

    [Tooltip("Where the open doors will be: North, South, East, West.")] [SerializeField]
    public GameObject[] doors;

    public void UpdateRoom(bool[] status) {
        /* Status tells us which rooms are open, which are closed */
        /* True = door is open */
        for (int dir = 0; dir < status.Length; dir++) {
            bool doorActive = status[dir];
            bool wallActive = !status[dir];
            doors[dir].SetActive(doorActive);
            walls[dir].SetActive(wallActive);

            // handles door if open
            if (doorActive) {
                OpenDoor(dir);
            }
        }
    }

    // handles door if open
    private void OpenDoor(int dir) {
        // NORTH
        if (dir == 0) {
            Destroy(this.transform.Find("Entrances/North Wall").gameObject);
        }
        // SOUTH
        if (dir == 1) {
            Destroy(this.transform.Find("Entrances/South Wall").gameObject);
        }
        // EAST
        if (dir == 2) {
            Destroy(this.transform.Find("Entrances/East Wall").gameObject);
        }
        // WEST
        if (dir == 3) {
            Destroy(this.transform.Find("Entrances/West Wall").gameObject);
        }
    }

}