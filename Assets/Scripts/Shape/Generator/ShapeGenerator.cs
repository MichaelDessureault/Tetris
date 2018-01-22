using System.Collections.Generic;
using UnityEngine;
using System;

public class ShapeGenerator : MonoBehaviour {
    public Transform parentTransform;
    public GameObject[] shapes;
    
    private Queue<GameObject> upAndComingShapesQueue = new Queue<GameObject>();
    
    private GameManager gmInstance;
    private ShapeDisplayer shapeDisplayer;

    // Use this for initialization
    void Start () {
        gmInstance = GameManager.Instance;
        shapeDisplayer = FindObjectOfType<ShapeDisplayer>();

        for (int i = 0; i < 4; i++) {
            AddShape();
        }
        
        SpawnShape();
    }

    void AddShape() {
        upAndComingShapesQueue.Enqueue(shapes[UnityEngine.Random.Range(0, shapes.Length)]);
    }

    public void SpawnShape () {
        // Spawn the next shape
        GameObject currentShape = Instantiate(upAndComingShapesQueue.Dequeue(), parentTransform, false);
        MakeGhost(currentShape);
        currentShape.AddComponent<ActiveShape>();

        // Add another shape to the queue
        AddShape();
        shapeDisplayer.UpdateUpAndComingDisplay(upAndComingShapesQueue);
    }

    public void SpawnShapeByType (ShapeType type) {

        GameObject shapeObj = Array.Find(shapes, obj => obj.GetComponent<Shape>().type == type);

        if (shapeObj != null) {
            GameObject currentShape = Instantiate(shapeObj, parentTransform, false);
            MakeGhost(currentShape);
            currentShape.AddComponent<ActiveShape>();
        } else {
            Debug.LogWarning("No shape found for ShapeType: " + type);
            // Dequeue a shape instead
            SpawnShape();
        }
    }

    /// <summary>
    /// Creates a clone of the current gameobject named "Ghost"
    /// </summary>
    private void MakeGhost(GameObject ghostMe) {
        GameObject ghost = Instantiate(ghostMe, parentTransform, false);
        ghost.name = "Ghost";
        ghost.tag = "Ghost";
        ghost.GetComponent<SpriteRenderer>().enabled = false;
    }
}
