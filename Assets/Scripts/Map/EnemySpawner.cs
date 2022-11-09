using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] [Tooltip("Enemy Prefabs")]
    private List<GameObject> enemies;

    [Tooltip("List of active rooms, given to us by the dungeon generator.")]
    private List<GameObject> rooms;

    public void InitialSpawner(List<GameObject> activeRooms) {
        rooms = activeRooms;
        for (int i = 0; i < rooms.Count; i++) {
            GameObject currentRoom = rooms[i];
            GameObject currentGround = currentRoom.transform.Find("Ground").gameObject;
            SetupRoom(currentRoom);
        }
    }

    public void SetupRoom(GameObject room) {
        Room.RoomType type = room.GetComponent<Room>().myType;
        if (Room.RoomType.Enemy == type) {
            /* Spawns in the center of room */
            CreateEnemy(room.transform.position);
        }
        // other room type conditions here
    }

    public void CreateEnemy(Vector3 position) {
        int enemyIndex = Random.Range(0, enemies.Count);
        Vector3 enemyPosition = new Vector3(position.x, position.y, position.z);
        GameObject enemyPrefab = enemies[enemyIndex];
        Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
    }
}