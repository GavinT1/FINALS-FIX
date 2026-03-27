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
        
        // 1. Only start if we click the Green Dot (StartNode)
        if (hit.collider != null && hit.collider.CompareTag("StartNode"))
        {
            isDrawing = true;
            points.Clear();
            lineRenderer.positionCount = 0;
            
            AddPoint(hit.collider.transform.position);
            lastHitNode = hit.collider.gameObject;
        }
    }

    void UpdateLine()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Visual: Line follows mouse
        lineRenderer.positionCount = points.Count + 1;
        lineRenderer.SetPosition(points.Count, mousePos);

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject != lastHitNode)
        {
            // 2. If we hit an obstacle, fail!
            if (hit.collider.CompareTag("Obstacle"))
            {
                StopDrawing();
                return;
            }

            // 3. ONLY COMPLETE IF IT'S THE END NODE
            if (hit.collider.CompareTag("EndNode"))
            {
                AddPoint(hit.collider.transform.position);
                CompleteLevel();
                return;
            }

            // 4. SNAPPING: If it's a regular grid box, snap to its center
            // We check if the box is NOT a StartNode to avoid snapping back to start
            if (!hit.collider.CompareTag("StartNode"))
            {
                AddPoint(hit.collider.transform.position);
                lastHitNode = hit.collider.gameObject;
            }
        }
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
    }

    void CompleteLevel()
    {
        isDrawing = false;
        Debug.Log("Connected!");
        FindObjectOfType<PuzzleManager>().GenerateLevel();
    }
}