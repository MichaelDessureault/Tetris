using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays the held shape and the upAndComingShapes
/// </summary>
public class ShapeDisplayer : MonoBehaviour {

    public Transform parentTransform;
    public Vector2 heldLocation;
    public Vector2[] upAndComingLocations;

    private GameObject held;
    private List<GameObject> upAndComingObjects = new List<GameObject>();

    #region up and coming objects
    public void UpdateUpAndComingDisplay (Queue<GameObject> shapes) {
        ClearUpAndComing();

        for (int i = 0; i < SmallestLength(upAndComingLocations, shapes); i++) {
            var obj = shapes.Dequeue();

            GameObject displayObj = Instantiate(obj, parentTransform, true);
            displayObj.transform.position = upAndComingLocations[i];
            displayObj.transform.parent = parentTransform;
            upAndComingObjects.Add(displayObj);

            shapes.Enqueue(obj);
        }
    }

    void ClearUpAndComing() {
        // If the count is not zero then destroy all the objects in the list
        if (upAndComingObjects.Count != 0) {
            for (int i = upAndComingObjects.Count - 1; i >= 0; i--) {
                Destroy(upAndComingObjects[i]);
            }
        }

        upAndComingObjects.Clear();
    }
    int SmallestLength(Vector2[] list1, Queue<GameObject> queue) {
        return Mathf.Min(list1.Length, queue.Count);
    }
    #endregion

    public void UpdateHeldDisplay(GameObject shape) {
        if (held != null)
            Destroy(held);

        shape.transform.position = heldLocation;

        Shape shapeScript = shape.GetComponent<Shape>();
        shape.transform.rotation = Quaternion.Euler(shapeScript.defaultRotation);

        shape.SetActive(true);
        held = shape;
    }

}
