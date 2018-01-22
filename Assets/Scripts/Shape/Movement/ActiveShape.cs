using UnityEngine;

public class ActiveShape : MonoBehaviour {
    GameManager gmInstance;

    void Start () {
        // Add the movement script onto the active shape
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        gameObject.AddComponent<Movement>();

        gmInstance = GameManager.Instance;
        gmInstance.landedEvent += Landed;
        gmInstance.holdEvent += Hold;
    }
    
    private void Hold (GameObject holdShape) {
        ClearEvents();
        Destroy(this);
    }

    private void Landed(GameObject landedObject) {
        ClearEvents();
        // destroy this script
        Destroy(this);
    }

    private void ClearEvents() {
        gmInstance.holdEvent -= Hold;
        gmInstance.landedEvent -= Landed;
    }
}
