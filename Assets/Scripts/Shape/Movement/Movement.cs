using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public static float downSpeed = .5f;
    public static float forceNextShapeTime = 10f;

    private Transform ghostTransform;
    private GameManager gmInstance;

    private Shape ghostShapeScript;
    private WaitForSeconds wait = new WaitForSeconds(0.05f);
    public bool CantMoveRight { get; set; }
    public bool CantMoveLeft { get; set; }
    public bool CantMoveDown { get; set; }
    public bool HitOutOfBoundsTrigger { get; set; }
    public bool HitTile { get; set; }

    private int moveDownFailed = 0;
    private bool movementEnabled;
    private bool isRotating;
    private bool isMovingHorizontal;

    private GameObject ghostObject;

    private float previousDownSpeed;
    private bool isMovingDown;

    private void Start() {
        gmInstance = GameManager.Instance;
        // cache the ghostTransform
        ghostObject = GameObject.Find("Ghost");
        ghostTransform = ghostObject.transform;
        ghostObject.AddComponent<GhostTrigger>().movementScript = this;
        ghostObject.GetComponent<PolygonCollider2D>().enabled = true;
        ghostShapeScript = ghostObject.GetComponent<Shape>();

        movementEnabled = true;
        previousDownSpeed = downSpeed;

        // Start an loop for moving down
        StartCoroutine(AutoDown());

        // Start an invoke call when the forceNextSpaceTime is up (this will prevent a shape from not landing)
        StartCoroutine(ForceNextShape());
    }
    

    private void Update() {

        if (!movementEnabled)
            return;

        if (!isRotating && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))) {
            StartCoroutine(Rotate());
        }
        
        if (!isMovingHorizontal && !CantMoveRight && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))) {
            StartCoroutine(Move(Vector3.right * 2));
        }

        if (!isMovingHorizontal && !CantMoveLeft && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))) {

            StartCoroutine(Move(Vector3.left * 2));
        }

        if (!isMovingDown && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))) {
            StartCoroutine(Down());
            // Score is incremented by 10 for each line
            Score.score += 10;
        }
            
        // Fast drop
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(FastDrop());
        }

        // Hold shape
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) {
            if (!gmInstance.HeldUsed) {
                InvokeHoldEvent();
            }
        }

    }

    /// <summary>
    /// Will prevent a user from moving a shape left or right or rotating a shape infinitely (these actions reset the moveDownFailed variable)
    /// </summary>
    IEnumerator ForceNextShape() {
        yield return new WaitForSeconds(forceNextShapeTime);
        StartCoroutine(FastDrop());
    }

    /// <summary>
    /// Every "X" amount of seconds the block will go down one block;
    /// Seconds are set by downSpeed static variable
    /// </summary>
    IEnumerator AutoDown() {
        while (movementEnabled) {
            yield return new WaitUntil(() => Valid());
            StartCoroutine(Down());
            Score.score++;
            yield return new WaitForSeconds(downSpeed);
        }
    }
    
    IEnumerator FastDrop() {
        movementEnabled = false;
        yield return new WaitUntil(() => Valid());
        // Less then 20 prevents an infinite loop issue
        int safetyCount = 0;
        while (safetyCount < 20) {
            // Score is incremented by 15 each line
            Score.score += 15;
            yield return StartCoroutine(Down());
            safetyCount++;
        }
    }

    bool Valid() {
        return (!isMovingDown && !isMovingHorizontal && !isRotating);
    }

    #region movement
    /// <summary>
    /// Rotate will attempt to rotate the ghost object to the next location before rotating the main shape
    /// If the ghost hit a tile or outofboundstrigger it will rotate back to the oringal state and try the next offset if theres more.
    /// If it didn't hit than it rotates the main shape
    /// </summary>
    IEnumerator Rotate() {
        isRotating = true;
        
        Vector3 rotation;
        List<Vector3> offSets;

        ghostShapeScript.GetNextRotationStateAndPositionOffSets(out offSets, out rotation);

        // If there is no rotation then leave
        if (rotation == Vector3.zero) {
            isRotating = false;
            yield break;
        }

        Vector3 startingPos = ghostTransform.position;
        Vector3 endingPostion = startingPos;

        bool makeChange = true;

        // Try all possible offsets before giving up
        foreach (Vector3 newOffset in offSets) {
            ghostTransform.position += newOffset;
            ghostTransform.Rotate(rotation);

            yield return wait;

            if (HitTile || HitOutOfBoundsTrigger) {
                ghostTransform.Rotate(-rotation);
                ghostTransform.position -= newOffset;
                makeChange = false;
                yield return wait;
            } else { 
                makeChange = true;
                endingPostion = startingPos + newOffset;
                break;
            }
        } 

        // If there was no change that could be made, then makeChange will be false
        if (makeChange) {
            transform.Rotate(rotation);
            transform.position = endingPostion;
            // Reset the moveDownFialed variable 
            moveDownFailed = 0;
        } else {
            // Revert the changes made in the ghostShapeScript
            ghostShapeScript.RevertRotationStateChange();
        }

        isRotating = false;
    }

    /// <summary>
    /// Move will attempt to move the ghost object to the direction passed in world space
    /// If the ghost hit a tile it will move back to it's original location
    /// If it didn't hit than it moves the main shape
    /// </summary>
    /// <param name="change"> The amount of translation the transform will performs in world space </param>
    IEnumerator Move(Vector3 change) {

        isMovingHorizontal = true;

        ghostTransform.position += change;
        
        yield return wait;

        if (HitTile) {
            ghostTransform.position -= change;
        } else {
            transform.position += change;
            // Reset the moveDownFialed variable 
            moveDownFailed = 0;
        }
        isMovingHorizontal = false;
    }

    /// <summary>
    /// Down will attempt to move the ghost object down in world space
    /// If the ghost hit a tile or outofboundstrigger it will move back to it's original location
    /// If it didn't hit either of them than it moves the main shape
    /// 
    /// Upon landing the LandedEvent in GameManager is invoked.
    /// </summary>
    IEnumerator Down() {
        if (moveDownFailed == 1) {
            InvokeLandedEvent();
            yield break;
        }

        isMovingDown = true;

        Vector3 prevPos = ghostTransform.position;
        ghostTransform.position += new Vector3(0, -2, 0);

        // Wait for the trigger detecton
        yield return wait;

        if (HitTile || HitOutOfBoundsTrigger) {
            ghostTransform.position -= new Vector3(0, -2, 0);
            moveDownFailed++;
        } else {
            transform.Translate(new Vector2(0, -2), Space.World);
        }

        isMovingDown = false;
    }
    #endregion

    #region Invoke Events
    void InvokeHoldEvent() {
        StopAllCoroutines();

        // Remove the ghost object
        Destroy(ghostObject);

        // Invoke the holdEvent
        if (gmInstance.holdEvent != null)
            gmInstance.holdEvent(gameObject);

        // Destroy this Script
        Destroy(this);
    }

    void InvokeLandedEvent() {
        StopAllCoroutines();

        // Remove the ghost object
        Destroy(ghostObject);

        // Invoke the landedEvent
        if (gmInstance.landedEvent != null)
            gmInstance.landedEvent(gameObject);

        // Destroy this Script
        Destroy(this);
    }
    #endregion
}