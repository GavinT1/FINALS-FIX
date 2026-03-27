using UnityEngine;
using System.Collections.Generic; // Required for Lists

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
    [Range(0, 10)]
    public int obstacleCount = 3; // Control how many obstacles appear

    void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        // 1. Clear the board first (stops boxes from stacking/doubling up)
        foreach (GameObject slot in allGridSlots)
        {
            foreach (Transform child in slot.transform) {
                Destroy(child.gameObject);
            }
        }

        // 2. Create a list of available indices (0 to 15)
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < allGridSlots.Length; i++) {
            availableIndices.Add(i);
        }

        // 3. Pick and Spawn Start Point
        int startPosInList = Random.Range(0, availableIndices.Count);
        int startSlotIdx = availableIndices[startPosInList];
        SpawnNode(allGridSlots[startSlotIdx], startPointSprite, Color.green, "StartNode");
        availableIndices.RemoveAt(startPosInList); 

        // 4. Pick and Spawn End Point
        int endPosInList = Random.Range(0, availableIndices.Count);
        int endSlotIdx = availableIndices[endPosInList];
        SpawnNode(allGridSlots[endSlotIdx], endPointSprite, Color.red, "EndNode");
        availableIndices.RemoveAt(endPosInList); 

        // 5. Spawn ONLY A FEW obstacles (leaves the rest of the boxes empty!)
        for (int i = 0; i < obstacleCount; i++)
        {
            if (availableIndices.Count > 0)
            {
                int randomListIdx = Random.Range(0, availableIndices.Count);
                int slotIdx = availableIndices[randomListIdx];
                availableIndices.RemoveAt(randomListIdx);

                int randomSpriteIndex = Random.Range(0, scribbleSprites.Length);
                Sprite chosenSprite = scribbleSprites[randomSpriteIndex];

                SpawnNode(allGridSlots[slotIdx], chosenSprite, Color.white, "Obstacle");
            }
        }
    }

    void SpawnNode(GameObject slot, Sprite graphic, Color color, string nodeTag)
    {
        GameObject newNode = Instantiate(nodePrefab, slot.transform.position, Quaternion.identity);
        newNode.transform.SetParent(slot.transform);
        newNode.transform.localPosition = Vector3.zero;
        newNode.transform.localScale = Vector3.one;

        SpriteRenderer sr = newNode.GetComponent<SpriteRenderer>();
        if (sr == null) sr = newNode.AddComponent<SpriteRenderer>();

        if (graphic != null)
        {
            sr.sprite = graphic;
        }

        sr.color = color;
        sr.sortingOrder = 10;
        
        // This sets the tag for the LineDrawer to detect
        newNode.tag = nodeTag; 
        
        // Ensure the node has a collider so the mouse can hit it
        if (newNode.GetComponent<Collider2D>() == null)
        {
            newNode.AddComponent<BoxCollider2D>();
        }
    }
}