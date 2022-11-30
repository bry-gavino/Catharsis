using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonGenerator : MonoBehaviour {
    [Tooltip("Holds the information of a cell in the maze.")]
    public class Cell {
        [Tooltip("Tracks if the algorithm has visited this cell.")]
        public bool visited = false;

        [Tooltip("If enabled, door. Else, wall. North, South, East, West.")]
        public bool[] status = new bool[4];
    }

    [Tooltip("Dimensions of the dungeon grid.")] [SerializeField]
    public Vector2 size;

    [Tooltip("Basic room to use.")] [SerializeField]
    public GameObject room;

    [Tooltip("Boss room to use.")] [SerializeField]
    public GameObject bossRoom;

    [Tooltip("Endpoint of map.")] [SerializeField]
    public GameObject endPoint;

    [Tooltip("Distance between each room.")] [SerializeField]
    public Vector2 offset;

    [Tooltip("The position where the dungeon will start.")] [SerializeField]
    public int startPos = 0;

    [Tooltip("List of cells that will be the board.")]
    private List<Cell> board;

    [Tooltip("List of all rooms generated.")]
    public List<GameObject> activeRooms;

    [SerializeField] [Tooltip("Enemy Prefabs.")]
    private List<GameObject> enemyTypes;

    [SerializeField] [Tooltip("Boss Prefabs.")]
    private List<GameObject> bossTypes;

    [Tooltip("Delete all of these on scene transition.")]
    private List<GameObject> spawnedEnemies;

    [Tooltip("If flagged, will create a boss room.")]
    public bool createBoss = false;

    [Tooltip("Which boss to spawn by index of the list.")][SerializeField]
    public int bossIndex = 0;

    // Start is called before the first frame update
    void Start() {
        activeRooms = new List<GameObject>();
        spawnedEnemies = new List<GameObject>();
        endPoint = this.transform.Find("EndPoint").gameObject;
        if (createBoss == false) {
            /* Create a big maze */
            MazeGenerator();
        }
        else {
            /* Otherwise, create the single boss room */
            Debug.Log("Creating boss room.");
            Vector3 position = new Vector3(0.0f, 10.5f, 0.0f);
            var the_boss_room = Instantiate(bossRoom, position, Quaternion.identity, transform);
            activeRooms.Add(the_boss_room);
            // the_boss_room.GetComponent<Room>().myType = Room.RoomType.Boss;
        }

        PopulateRooms();
    }

    private void OnDestroy() {
        while (spawnedEnemies.Count > 0) {
            GameObject enemy = spawnedEnemies[0];
            spawnedEnemies.RemoveAt(0);
            Destroy(enemy);
        }
    }

    /**
     * Instantiates an enemy of random type at this position.
     */
    public void CreateEnemy(Vector3 position) {
        int enemyIndex = Random.Range(0, enemyTypes.Count);
        Vector3 enemyPosition = new Vector3(position.x, position.y, position.z);
        GameObject enemyPrefab = enemyTypes[enemyIndex];
        spawnedEnemies.Add(Instantiate(enemyPrefab, enemyPosition, Quaternion.identity));
    }

    /**
     * Fades out all gates in the scene.
     */
    public void DisableGates() {
        Debug.Log("Disable boss gate in DungeonGenerator.");
        /*
        // alternative approach: this disables all gates in a room, might be useful later
        var bossRoom = activeRooms[0];
        foreach (Transform child_transform in bossRoom.transform) {
            if (child_transform.CompareTag("BossGate")) {
                Debug.Log("Found a gate.");
                child_transform.gameObject.GetComponent<Gate>().FadeOut();
            }
        }
        */
        var gates = GameObject.FindGameObjectsWithTag("BossGate");
        foreach (var gate in gates) {
            Debug.Log("Found a gate.");
            gate.GetComponent<Gate>().FadeOut();
        }
    }

    public void CreateBoss(Vector3 position) {
        if (bossTypes.Count <= 0) {
            Debug.Log("No bosses in bossTypes list, can't spawn anything.");
        }
        else {
            Debug.Log("Spawning boss at index: " + bossIndex);
        }
        Vector3 enemyPosition = new Vector3(position.x, position.y + 10, position.z);
        GameObject enemyPrefab = bossTypes[bossIndex];
        spawnedEnemies.Add(Instantiate(enemyPrefab, enemyPosition, Quaternion.identity));

        // update index, rotate around the size of list
        bossIndex = (bossIndex + 1) % bossTypes.Count;
    }

    public void SetupRoom(GameObject room) {
        Room.RoomType type = room.GetComponent<Room>().myType;
        if (Room.RoomType.Enemy == type) {
            /* Spawns in the center of room */
            CreateEnemy(room.transform.position);
        }
        else if (Room.RoomType.Boss == type) {
            /* Spawns north of the center of room */
            CreateBoss(room.transform.position);
        }
        // other room type conditions here
    }

    /**
     * Now that everything is generated, can do post-processing here.
     * Make the last room (bottom right) the boss room, choose a spawn room (top left or some other spot).
     * Randomly pick rooms to be of type treasure, trap, monster, etc.
     */
    void PopulateRooms() {
        for (int i = 0; i < activeRooms.Count; i++) {
            GameObject currentRoom = activeRooms[i];
            GameObject currentGround = currentRoom.transform.Find("Ground").gameObject;
            GameObject currentOverlay = currentGround.transform.Find("Overlay").gameObject;
            // GameObject physicalShrine = currentRoom.transform.Find("Physical_Shrine").gameObject;
            // physicalShrine.SetActive(false); //disable shrine in beginning
            Room.RoomType type = Room.RoomType.Uninitialized;
            float color_dampening_constant = 0.35f;
            if (i == 0) {
                // boss room or start room?
                if (createBoss) {
                    Debug.Log("Moving boss room endpoint.");
                    Vector3 position = currentRoom.transform.position;
                    Vector3 newPosition = new Vector3(position.x, position.y + 20, position.z);
                    endPoint.transform.position = newPosition;
                    
                    type = Room.RoomType.Boss;
                    // physicalShrine.SetActive(true); //disable shrine in beginning
                }
                else {
                    currentOverlay.GetComponent<SpriteRenderer>().color = (new Color(0.34f,0.7f,1f,0.7f));
                    type = Room.RoomType.Start;
                    //Adding shrine merge - first level gets shrine for now
                    // currentRoom.transform.Find("Physical_Shrine").gameObject.SetActive(false);
                }
            }
            else if (i == 1) {
                // second room
                currentOverlay.GetComponent<SpriteRenderer>().color = (new Color(0.2f,1,0.82f,0.5f));
                type = Room.RoomType.Shrine;
            }
            else if (i == activeRooms.Count - 1) {
                // last room
                currentOverlay.GetComponent<SpriteRenderer>().color = (new Color(1,0,0.6f,0.6f));
                // puts EndPoint in last room
                Vector3 position = currentRoom.transform.position;
                endPoint.transform.position = position;
                type = Room.RoomType.End;
            }
            else {
                // every other room...
                // randomly put in enemies or treasure
                // rainbow mode
                float vRange = 0.15f;
                float v1 = Random.Range(0f, 1f);
                float v2 = Random.Range(0f, 1f);
                float v3 = Random.Range(vRange, 1f);
                float r = -1;
                float g = -1;
                float b = -1;
                if (v2 < 1.0f/3.0f) {r = 1;} 
                else if (v2 >= 2.0f/3.0f) {g = 1;} 
                else {b = 1;}
                if (v2 < 0.5f) {
                    if (r == -1) {r = vRange;}
                    else if (g == -1) {g = vRange;}
                    else {b = vRange;}
                } else {
                    if (r == -1) {
                        if (g == -1) {g = vRange;}
                        else {b = vRange;}
                    } else if (g == -1) {
                        if (b == -1) {b = vRange;}
                        else {r = vRange;}
                    } else {
                        if (r == -1) {r = vRange;}
                        else {g = vRange;}
                    }
                }
                if (r == -1) {r = v3;}
                else if (g == -1) {g = v3;}
                else {b = v3;}

                var color = new Color(r,
                    g,
                    b,
                    0.7f);
                currentOverlay.GetComponent<SpriteRenderer>().color = color;
                type = Room.RoomType.Enemy;
            }

            currentRoom.GetComponent<Room>().myType = type;
            SetupRoom(currentRoom);
        }
    }

    void GenerateDungeon() {
        for (int i = 0; i < size.x; i++) {
            for (int j = 0; j < size.y; j++) {
                Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)];
                if (currentCell.visited) {
                    float xpos = i * offset.x;
                    float ypos = -j * offset.y;
                    Vector3 position = new Vector3(xpos, ypos, 0);
                    var newRoom = Instantiate(room, position, Quaternion.identity, transform)
                        .GetComponent<RoomBehavior>();
                    newRoom.UpdateRoom(currentCell.status);
                    newRoom.name = i + "-" + j;
                    // add to list
                    activeRooms.Add(newRoom.gameObject);
                }
            }
        }
    }


    void MazeGenerator() {
        board = new List<Cell>();
        /* Populate the board */
        for (int i = 0; i < size.x; i++) {
            for (int j = 0; j < size.y; j++) {
                board.Add(new Cell());
            }
        }

        int currentCell = startPos; // tracks which position we are at
        Stack<int> path = new Stack<int>(); // tracks the path we've made to the cell we're currently at

        int k = 0;
        // hard stop at 1000, but could use dimensions of maze
        while (k < 1000) {
            k++;
            board[currentCell].visited = true;
            // optimization
            if (currentCell == board.Count - 1) {
                break;
            }

            // check neighbors
            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0) {
                // no available neighbors
                if (path.Count == 0) {
                    // break from loop, reached the last cell on the path
                    break;
                }
                else {
                    currentCell = path.Pop(); // will now be the last cell added into the path. backtrack
                }
            }
            else {
                path.Push(currentCell); // add ourselves to path and pick a random neighbor to visit
                int newCell = neighbors[Random.Range(0, neighbors.Count)];
                if (newCell > currentCell) {
                    // south or east
                    if (newCell - 1 == currentCell) {
                        // going east
                        board[currentCell].status[2] = true; // east path of this cell open
                        currentCell = newCell;
                        board[currentCell].status[3] = true; // west path of new cell open
                    }
                    else {
                        // going south
                        board[currentCell].status[1] = true; // south path of this cell open
                        currentCell = newCell;
                        board[currentCell].status[0] = true; // north path of new cell open
                    }
                }
                else {
                    // north or west
                    if (newCell + 1 == currentCell) {
                        // going west
                        board[currentCell].status[3] = true; // west path of this cell open
                        currentCell = newCell;
                        board[currentCell].status[2] = true; // east path of new cell open
                    }
                    else {
                        // going north
                        board[currentCell].status[0] = true; // north path of this cell open
                        currentCell = newCell;
                        board[currentCell].status[1] = true; // south path of new cell open
                    }
                }
            }
        }

        GenerateDungeon();
    }

    List<int> CheckNeighbors(int cell) {
        List<int> neighbors = new List<int>();
        // check north, south, east, west
        int northNeighborIndex = Mathf.FloorToInt(cell - size.x);
        if (cell - size.x >= 0 && !board[northNeighborIndex].visited) {
            // check that it's not on the first row
            neighbors.Add(northNeighborIndex);
        }

        int southNeighborIndex = Mathf.FloorToInt(cell + size.x);
        if (cell + size.x < board.Count && !board[southNeighborIndex].visited) {
            // check that it's not on the last row
            neighbors.Add(southNeighborIndex);
        }

        int eastNeighborIndex = Mathf.FloorToInt(cell + 1);
        if (((cell + 1) % Mathf.FloorToInt(size.x) != 0) && !board[eastNeighborIndex].visited) {
            // check that it's not on the rightmost column
            neighbors.Add(eastNeighborIndex);
        }

        // west neighbor
        int westNeighborIndex = Mathf.FloorToInt(cell - 1);
        if (cell % Mathf.FloorToInt(size.x) != 0 && !board[westNeighborIndex].visited) {
            // check that it's not the leftmost column
            neighbors.Add(westNeighborIndex);
        }

        return neighbors;
    }
}