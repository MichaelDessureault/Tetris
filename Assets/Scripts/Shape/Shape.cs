using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vector3OffSets {
    public List<Vector3> offsets;
}

public class Shape : MonoBehaviour {
    public int currentRotationIndex = 0;
    public Vector3 defaultRotation;
    public ShapeType type;
    public RotationState rotationState;
    
    [SerializeField] protected List<RotationState> rotationStateList = new List<RotationState>();
    [SerializeField] protected List<Vector3> rotationList = new List<Vector3>();
    [SerializeField] protected List<Vector3OffSets> offSetList = new List<Vector3OffSets>();

    public void Print() {
        string information = "" + gameObject.name + ": ";
        for (int i = 0; i < rotationStateList.Count; i++) {
            information += rotationStateList[i] + " " + rotationList[i].ToString() + "\n OffsetList: ";

            foreach (Vector3 v3 in offSetList[i].offsets) {
                information += v3.ToString() + " ";
            }
        }
        Debug.Log(information);
    }

    public Block[] GetBlocks () {
        return GetComponentsInChildren<Block>();
    }
    
    public void GetNextRotationStateAndPositionOffSets (out List<Vector3> positionOffsets, out Vector3 rotation) {
        if (rotationList == null || rotationList.Count == 0) {
            rotation = Vector3.zero;
            positionOffsets = new List<Vector3>() { Vector3.zero };
        } else {
            // Loops back to 0 if it's the same length of array
            currentRotationIndex = ((currentRotationIndex + 1) % rotationList.Count);
            rotation = rotationList[currentRotationIndex];
            rotationState = rotationStateList[currentRotationIndex];
            positionOffsets = offSetList[currentRotationIndex].offsets;
        }
    }

    public void RevertRotationStateChange () {
        currentRotationIndex = (currentRotationIndex - 1 == -1) ? rotationList.Count - 1 : currentRotationIndex - 1;
        rotationState = rotationStateList[currentRotationIndex];
    }

    // used to preview the rotations and offsets without running the game
    private int previousOffSetIndex = 0;
    public void PreviewRotation () {
        previousOffSetIndex = 0;
        Vector3 rotation;
        List<Vector3> offsets;

        GetNextRotationStateAndPositionOffSets(out offsets, out rotation);
        transform.position += offsets[0];
        transform.Rotate(rotation);
    }

    public void PreviewOffSets () {

        List<Vector3> list = offSetList[currentRotationIndex].offsets;
        var count = list.Count;
        if (count <= 1) {
            return;
        }

        Vector3 old = list[previousOffSetIndex];
        int newOffSetIndex = (previousOffSetIndex + 1) % count;

        transform.position -= old;
        transform.position += list[newOffSetIndex];
        
        previousOffSetIndex = newOffSetIndex;
    }

    public void ClearAll () {
        rotationStateList.Clear();
        rotationList.Clear();
        offSetList.Clear();
    }
}
