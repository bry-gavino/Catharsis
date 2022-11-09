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

    [Tooltip("Delete all of these on scene transition.")]
    private List<GameObject> spawnedEnemies;

    // Start is called before the first frame update
    void Start() {
        activeRooms = new List<GameObject>();
        spawnedEnemies = new List<GameObject>();
        endPoint = this.transform.Find("EndPoint").gameObject;
        MazeGenerator();
        PopulateRooms();
    }

    private void OnDestroy() {
        foreach (var enemy in spawnedEnemies)
        {
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
    
    public void SetupRoom(GameObject room) {
        Room.RoomType type = room.GetComponent<Room>().myType;
        if (Room.RoomType.Enemy == type) {
            /* Spawns in the center of room */
            CreateEnemy(room.transform.position);
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
            Room.RoomType type = Room.RoomType.Uninitialized;
            if (i == 0) {
                // start room
                currentGround.GetComponent<SpriteRenderer>().color = Color.blue;
                type = Room.RoomType.Start;
            }
            else if (i == 1) {
                // second room
                currentGround.GetComponent<SpriteRenderer>().color = Color.green;
                type = Room.RoomType.Shrine;
            }
            else if (i == activeRooms.Count - 1) {
                // last room -> boss room
                currentGround.GetComponent<SpriteRenderer>().color = Color.red;
                // puts EndPoint in last room
                Vector3 position = currentRoom.transform.position;
                endPoint.transform.position = position;
                type = Room.RoomType.End;
            }
            else {
                // every other room...
                // randomly put in enemies or treasure
                // rainbow mode
                currentGround.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f),
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f));
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