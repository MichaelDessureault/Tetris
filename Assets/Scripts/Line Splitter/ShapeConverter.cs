using UnityEngine;

public class ShapeConverter : Object {

    public static void ConvertShapesToBlocksOnGrid (Shape shapeToConvert, Block[,] blockGrid) {
        
        Block[] blocks = shapeToConvert.GetBlocks();

        foreach (Block block in blocks) {
            ReplaceBlockInGridWithNewBlock(block, blockGrid);
        }

        // Destroy the LandedShape it's now it blocks
        Destroy(shapeToConvert.gameObject);
    }

    /// <summary>
    /// Replace the block in the grid based on the coordinates (position) of the newBlock.
    /// If block is not found in grid then nothing changes.
    /// </summary>
    /// <param name="newBlock"> The new block that will be taking the old blocks place</param>
    /// <param name="blockGrid"> The blockGrid to search through </param>
    private static void ReplaceBlockInGridWithNewBlock(Block newBlock, Block[,] blockGrid) {
        var newBlockY = newBlock.YPosition;

        // Check to see if the newBlock's Y is above the height y value in grid, if so game is over
        if (newBlockY > blockGrid[GameManager.KGirdHeight - 1, 0].YPosition) {
            GameManager.Instance.GameOver();
            return;
        }

        for (int y = 0; y < GameManager.KGirdHeight; y++) {
            
            // Check if the y location for this row is approximately the same as the blockY location
            // if so then it's on the correct row therefore check the x values
            
            var gridBlockY = blockGrid[y, 0].YPosition;
            
            if (Mathf.Approximately(gridBlockY, newBlockY)) {
                for (int x = 0; x < GameManager.KGirdWidth; x++) {
                    if (newBlock.Equals(blockGrid[y, x])) {
                        // cache the found block to destroy once it's been replaced
                        Block oldBlock = blockGrid[y, x];

                        // Set the found block location to the newBlock
                        blockGrid[y, x] = newBlock;

                        // Update the newBlock parent, along with the gameObject name, enable the collider and enable the spriteRenderer
                        newBlock.transform.parent = oldBlock.transform.parent;
                        newBlock.gameObject.name = "Background";
                        newBlock.GetComponent<BoxCollider2D>().enabled = true;
                        newBlock.spriteRenderer.enabled = true;
                        // Destroy the old block
                        Destroy(oldBlock.gameObject);
                        return;
                    }
                }

                // The x coordinate was not found if it hits this break
                break;
            }
        }
    } 
}
