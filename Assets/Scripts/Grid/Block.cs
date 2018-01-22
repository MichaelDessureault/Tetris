using UnityEngine;

public class Block : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public Sprite sprite;
    
    public Vector3 Position {
        get { return transform.position; }
    }
    
    public float YPosition {
        get { return transform.position.y; }
    }

    public float XPosition {
        get { return transform.position.x; }
    }

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void ChangeSprite (Sprite newSprite) {
        spriteRenderer.sprite = newSprite;
        sprite = newSprite;
    }

    /// <summary>
    /// Checks to see if the position of block blocks are the within a .05 distance
    /// </summary>
    /// <param name="obj">Another Block object</param>
    /// <returns>True if within distance, False if not in distance </returns>
    public bool Equals (Block obj) {
        if (obj == null)
            return false;

        if (Vector3.Distance(Position, obj.Position) < .05) {
            return true;
        }
        return false;
    }
}
