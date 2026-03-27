using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Added for easier list handling

public class PuzzleManager : MonoBehaviour
{
    [Header("Prefab & Assets")]
    public GameObject nodePrefab;
    public Sprite[] scribbleSprites; 
    public Sprite startPointSprite;
    public Sprite endPointSprite;

    [Header("Grid Slots")]
    public GameObject[] allGridSlots = new GameObject[16];

    [Header("Level Settings")]
    [Range(0, 10)] public int obstacleCount = 3; 
    public float minStartEndDist = 2.5f; // Ensures Start/End aren't neighbors

    void Start() { GenerateLevel(); }

    public void GenerateLevel()
    {
        // 1. Clear board
        foreach (GameObject slot in allGridSlots)
        {
            foreach (Transform child in slot.transform) Destroy(child.gameObject);
        }

        List<int> availableIndices = new List<int>();
        for (int i = 0; i < allGridSlots.Length; i++) availableIndices.Add(i);

        // 2. Spawn Start Point (Green)
        int startPosInList = Random.Range(0, availableIndices.Count);
        int startSlotIdx = availableIndices[startPosInList];
        Vector3 startPos = allGridSlots[startSlotIdx].transform.position;
        SpawnNode(allGridSlots[startSlotIdx], startPointSprite, Color.green, "StartNode");
        availableIndices.RemoveAt(startPosInList); 

        // 3. SMART SPAWN: Shuffle before picking End Point (Red)
        // This stops it from always picking the top-left/top-right
        for (int i = 0; i < availableIndices.Count; i++)
        {
            int temp = availableIndices[i];
            int randomIndex = Random.Range(i, availableIndices.Count);
            availableIndices[i] = availableIndices[randomIndex];
            availableIndices[randomIndex] = temp;
        }

        int endSlotIdx = -1;
        int listIndexToRemove = -1;

        for (int i = 0; i < availableIndices.Count; i++)
        {
            int potentialIdx = availableIndices[i];
            if (Vector3.Distance(startPos, allGridSlots[potentialIdx].transform.position) >= minStartEndDist)
            {
                endSlotIdx = potentialIdx;
                listIndexToRemove = i;
                break; 
            }
        }

        if (endSlotIdx == -1) // Fallback if no far spot found
        {
            listIndexToRemove = Random.Range(0, availableIndices.Count);
            endSlotIdx = availableIndices[listIndexToRemove];
        }

        Vector3 endPos = allGridSlots[endSlotIdx].transform.position;
        SpawnNode(allGridSlots[endSlotIdx], endPointSprite, Color.red, "EndNode");
        availableIndices.RemoveAt(listIndexToRemove); 

        // 4. SMART OBSTACLE SPAWN: Ensure exit paths are open
        int spawnedObstacles = 0;
        
        // Re-shuffle remaining spots for obstacles
        for (int i = 0; i < availableIndices.Count; i++)
        {
            int temp = availableIndices[i];
            int randomIndex = Random.Range(i, availableIndices.Count);
            availableIndices[i] = availableIndices[randomIndex];
            availableIndices[randomIndex] = temp;
        }

        foreach (int slotIdx in availableIndices)
        {
            if (spawnedObstacles >= obstacleCount) break;

            Vector3 slotPos = allGridSlots[slotIdx].transform.position;
            
            // RULE: Must be further than 1.1 units from both Start and End
            // This guarantees at least one empty box next to the dots
            bool tooCloseToStart = Vector3.Distance(slotPos, startPos) < 1.1f;
            bool tooCloseToEnd = Vector3.Distance(slotPos, endPos) < 1.1f;

            if (!tooCloseToStart && !tooCloseToEnd)
            {
                int spriteIdx = Random.Range(0, scribbleSprites.Length);
                SpawnNode(allGridSlots[slotIdx], scribbleSprites[spriteIdx], Color.white, "Obstacle");
                spawnedObstacles++;
            }
        }
    }

    void SpawnNode(GameObject slot, Sprite graphic, Color color, string nodeTag)
    {
        GameObject newNode = Instantiate(nodePrefab, slot.transform.position, Quaternion.identity, slot.transform);
        newNode.transform.localPosition = Vector3.zero;
        newNode.transform.localScale = Vector3.one;

        SpriteRenderer sr = newNode.GetComponent<SpriteRenderer>() ?? newNode.AddComponent<SpriteRenderer>();
        sr.sprite = graphic;
        sr.color = color;
        sr.sortingOrder = 10; // Ensure it draws on top of the grid
        newNode.tag = nodeTag; 

        if (newNode.GetComponent<Collider2D>() == null)
            newNode.AddComponent<BoxCollider2D>();
    }
}