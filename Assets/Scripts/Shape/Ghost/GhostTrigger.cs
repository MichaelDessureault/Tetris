using UnityEngine;

public class GhostTrigger : MonoBehaviour {
    public Movement movementScript;

    private void OnTriggerEnter2D(Collider2D other) {
        if (movementScript == null)
            return;
        
        if (other.gameObject.tag == "RightSideTrigger") {
            movementScript.CantMoveRight = true;
        } else if (other.gameObject.tag == "LeftSideTrigger") {
            movementScript.CantMoveLeft = true;
        } else if (other.gameObject.tag == "BottomTrigger") {
            movementScript.CantMoveDown = true;
        } else if (other.gameObject.tag == "OutOfBoundsTrigger") {
            movementScript.HitOutOfBoundsTrigger = true;
        } else if (other.gameObject.tag == "Tile") {
            movementScript.HitTile = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (movementScript == null)
            return;
        
        if (other.gameObject.tag == "RightSideTrigger") {
            movementScript.CantMoveRight = false;
        } else if (other.gameObject.tag == "LeftSideTrigger") {
            movementScript.CantMoveLeft = false;
        } else if (other.gameObject.tag == "BottomTrigger") {
            movementScript.CantMoveDown = false;
        } else if (other.gameObject.tag == "OutOfBoundsTrigger") {
            movementScript.HitOutOfBoundsTrigger = false;
        } else if (other.gameObject.tag == "Tile") {
            movementScript.HitTile = false;
        }
    }
}
