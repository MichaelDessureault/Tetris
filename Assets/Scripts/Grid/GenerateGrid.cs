using UnityEngine;

public class GenerateGrid : MonoBehaviour {
    public const int KGirdHeight = 16;
    public const int KGirdWidth = 10;

    public GameObject cloningBlock;
    public Transform parentTransform;

    private Block[,] blockGrid = new Block[KGirdHeight, KGirdWidth];
    
    void Start() {

        // Verify the Block script is found on the block that is going to be cloned
        if (cloningBlock.GetComponent<Block>() == null)
            Debug.LogError("Block Script is not found on the cloning gameobject: " + cloningBlock.name);
        
        GenerateBoard();

        // Set the blockGrid array within the gameManager to the newly created blockGrid array
        GameManager.Instance.BlockGrid = blockGrid;
    }

    /// <summary>
    /// Generates the board from the bottom left corner to the top right corner 
    /// </summary>
    void GenerateBoard() {
        for (int y = 0; y < KGirdHeight; y++) {
            for (int x = 0; x < KGirdWidth; x++) {
                GameObject newBlock = Instantiate(cloningBlock, parentTransform, false);

                // Set the newBlock's location
                Vector2 loc = new Vector2(-1 * (KGirdWidth - 1), -1 * (KGirdHeight - 1));
                Vector2 offset = new Vector2(x, y) * 2;
                newBlock.transform.position = loc + offset;

                // Add the Blockcomponent into the blockGrid array
                Block blockComponent = newBlock.GetComponent<Block>();
                blockGrid[y, x] = blockComponent;
            }
        }
    }
}
