using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour {
    enum DoorDirectionNeeded {
        North,
        South,
        West,
        East
    }

    [Tooltip("The direction a door of the next room must face")] [SerializeField]
    DoorDirectionNeeded door_opening;

    private RoomTemplates templates;
    private int roomIndex;
    private bool spawned = false;

    void Start() {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        roomIndex = 0;

        Invoke("Spawn", 0.1f);
    }

    // Update is called once per frame
    void Spawn() {
        if (!spawned) {
            if (door_opening == DoorDirectionNeeded.North) {
                // spawn a room with a top-facing door
                roomIndex = Random.Range(0, templates.northRooms.Length);
                Instantiate(templates.northRooms[roomIndex], transform.position, transform.rotation);
            }
            else if (door_opening == DoorDirectionNeeded.South) {
                // spawn a room with a bottom-facing door
                roomIndex = Random.Range(0, templates.southRooms.Length);
                Instantiate(templates.southRooms[roomIndex], transform.position, transform.rotation);
            }
            else if (door_opening == DoorDirectionNeeded.East) {
                // spawn a room with a right-facing door
                roomIndex = Random.Range(0, templates.eastRooms.Length);
                Instantiate(templates.eastRooms[roomIndex], transform.position, transform.rotation);
            }
            else if (door_opening == DoorDirectionNeeded.West) {
                // spawn a room with a left-facing door
                roomIndex = Random.Range(0, templates.westRooms.Length);
                Instantiate(templates.westRooms[roomIndex], transform.position, transform.rotation);
            }
        }

        spawned = true;
    }

    void OnTriggerExit2D(Collider2D other) {
        // check if another room is already here
        if (other.CompareTag("SpawnPoint") && other.GetComponent<RoomSpawner>().spawned == true) {
            Destroy(gameObject);
        }
    }
}