using UnityEngine;
using System.Collections.Generic;

public class LineDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private bool isDrawing = false;
    private List<Vector3> points = new List<Vector3>();
    private GameObject lastHitNode;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) StartDrawing();
        if (isDrawing) UpdateLine();
        if (Input.GetMouseButtonUp(0)) StopDrawing();
    }

    void StartDrawing()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        
        if (hit.collider != null)
        {
            // Check if object or its child is the StartNode
            if (CheckTag(hit.collider.gameObject, "StartNode"))
            {
                isDrawing = true;
                points.Clear();
                lineRenderer.positionCount = 0;
                
                AddPoint(hit.collider.transform.position);
                lastHitNode = hit.collider.gameObject;
                Debug.Log("Started drawing from Green Dot!");
            }
        }
    }

    void UpdateLine()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Visual tip follows mouse
        lineRenderer.positionCount = points.Count + 1;
        lineRenderer.SetPosition(points.Count, mousePos);

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject != lastHitNode)
        {
            GameObject hitObj = hit.collider.gameObject;

            // 1. Check for Obstacle (in parent or child)
            if (CheckTag(hitObj, "Obstacle"))
            {
                Debug.Log("Hit an Obstacle! Resetting...");
                StopDrawing();
                return;
            }

            // 2. Check for EndNode (in parent or child)
            if (CheckTag(hitObj, "EndNode"))
            {
                Debug.Log("Hit the Red Dot! Level Complete!");
                AddPoint(hitObj.transform.position);
                CompleteLevel();
                return;
            }

            // 3. SNAPPING: If it's an empty grid box, snap to center
            if (!CheckTag(hitObj, "StartNode"))
            {
                AddPoint(hitObj.transform.position);
                lastHitNode = hitObj;
            }
        }
    }

    // This helper function checks the object AND its children for the tag
    bool CheckTag(GameObject obj, string tag)
    {
        if (obj.CompareTag(tag)) return true;
        
        // Check children (for when the sprite is inside the grid slot)
        foreach (Transform child in obj.transform)
        {
            if (child.CompareTag(tag)) return true;
        }
        return false;
    }

    void AddPoint(Vector3 pos)
    {
        pos.z = 0; 
        points.Add(pos);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, pos);
    }

    void StopDrawing()
    {
        isDrawing = false;
        lineRenderer.positionCount = 0;
        points.Clear();
    }

    void CompleteLevel()
    {
        isDrawing = false;
        FindObjectOfType<PuzzleManager>().GenerateLevel();
    }
}