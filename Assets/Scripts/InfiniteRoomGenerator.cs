using System.Collections.Generic;
using UnityEngine;

public class InfiniteRoomGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs;       // Different room types
    public Transform player;
    public int gridRadius = 2;             // How many rooms around the player to generate
    public float roomSize = 10f;           // Width/Length of each room (should match prefab size)

    private Dictionary<Vector2Int, GameObject> activeRooms = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<Vector2Int, int> roomMemory = new Dictionary<Vector2Int, int>(); // Remember room types
    private Vector2Int currentPlayerGridPos;

    void Start()
    {
        currentPlayerGridPos = GetGridPosition(player.position);
        GenerateRoomsAroundPlayer();
    }

    void Update()
    {
        Vector2Int newGridPos = GetGridPosition(player.position);

        if (newGridPos != currentPlayerGridPos)
        {
            currentPlayerGridPos = newGridPos;
            GenerateRoomsAroundPlayer();
        }
    }

    // Convert world position to grid coordinates
    Vector2Int GetGridPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / roomSize);
        int y = Mathf.RoundToInt(position.z / roomSize);
        return new Vector2Int(x, y);
    }

    // Generate rooms around the player based on grid
    void GenerateRoomsAroundPlayer()
    {
        List<Vector2Int> roomsToRemove = new List<Vector2Int>();

        // Identify rooms to remove
        foreach (Vector2Int gridPos in activeRooms.Keys)
        {
            if (Vector2Int.Distance(gridPos, currentPlayerGridPos) > gridRadius)
                roomsToRemove.Add(gridPos);
        }

        // Remove out-of-range rooms
        foreach (Vector2Int gridPos in roomsToRemove)
        {
            Destroy(activeRooms[gridPos]);
            activeRooms.Remove(gridPos);
        }

        // Generate new rooms
        for (int x = -gridRadius; x <= gridRadius; x++)
        {
            for (int y = -gridRadius; y <= gridRadius; y++)
            {
                Vector2Int newGridPos = currentPlayerGridPos + new Vector2Int(x, y);

                if (!activeRooms.ContainsKey(newGridPos))
                {
                    int roomIndex;

                    // Remember or generate new room
                    if (roomMemory.ContainsKey(newGridPos))
                    {
                        roomIndex = roomMemory[newGridPos]; // Use stored room
                    }
                    else
                    {
                        roomIndex = Random.Range(0, roomPrefabs.Length); // Generate new room
                        roomMemory[newGridPos] = roomIndex; // Store for later
                    }

                    // Position the room exactly in grid space
                    Vector3 roomWorldPos = new Vector3(newGridPos.x * roomSize, 0, newGridPos.y * roomSize);
                    GameObject room = Instantiate(roomPrefabs[roomIndex], roomWorldPos, Quaternion.identity);
                    activeRooms[newGridPos] = room;
                }
            }
        }
    }
}
