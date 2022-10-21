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

    [Tooltip("Debugging: Check if this point spawned.")] [SerializeField]
    private bool spawned = false;

    [Tooltip("How long until next room is spawned")] [SerializeField]
    private float spawnInterval = 3f;

    [Tooltip("How long before this object gets destroyed.")]
    private float destroyTime;

    void Start() {
        templates = GameObject.FindGameObjectWithTag("RoomTemplates").GetComponent<RoomTemplates>();
        roomIndex = 0;
        destroyTime = spawnInterval * 5;

        Destroy(this.gameObject, destroyTime);
        Invoke("Spawn", 3f);
    }

    // Update is called once per frame
    void Spawn() {
        int maxRooms = templates.maxRoomsAllowed; // temporary until we finish other rooms
        if (!spawned && templates.rooms.Count <= maxRooms) {
            if (door_opening == DoorDirectionNeeded.North) {
                // spawn a room with a top-facing door
                roomIndex = Random.Range(0, templates.northRooms.Length);
                Instantiate(templates.northRooms[roomIndex], transform.position, Quaternion.identity);
            }
            else if (door_opening == DoorDirectionNeeded.South) {
                // spawn a room with a bottom-facing door
                roomIndex = Random.Range(0, templates.southRooms.Length);
                Instantiate(templates.southRooms[roomIndex], transform.position, Quaternion.identity);
            }
            else if (door_opening == DoorDirectionNeeded.East) {
                // spawn a room with a right-facing door
                roomIndex = Random.Range(0, templates.eastRooms.Length);
                Instantiate(templates.eastRooms[roomIndex], transform.position, Quaternion.identity);
            }
            else if (door_opening == DoorDirectionNeeded.West) {
                // spawn a room with a left-facing door
                roomIndex = Random.Range(0, templates.westRooms.Length);
                Instantiate(templates.westRooms[roomIndex], transform.position, Quaternion.identity);
            }
        }

        spawned = true;
    }

    void OnTriggerExit2D(Collider2D other) {
        // check if another spawn point is here
        // here we may also need to consider closing stuff
        // Instantiate(templates.bigWall, transform.position, Quaternion.identity);
        if (other.CompareTag("Destroyer")) {
            // this spawn point is invading an established room
            Destroy(gameObject);
        }
        else if (other.CompareTag("SpawnPoint")) {
            // there is a spawn point invading our territory
            Destroy(other.gameObject);
        }

        spawned = true;
    }
}