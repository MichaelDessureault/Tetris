using System.Collections.Generic;
using UnityEngine;

public class LineSplitter : MonoBehaviour {
    public static bool spawnNext = true;

    /// <summary>
    /// Loops through the blockGrid checking the first block in the row if it's a backgroundBlockSprite it continues to the next row
    /// If it's a different sprite it loops through that row until it finds a backgroundBlockSprite, if not it's added to foundCompletedLinesList
    /// </summary>
    /// <param name="blockGrid"></param>
    public static void CheckForFullLine (Block[,] blockGrid) {
        spawnNext = false;
        List<int> foundCompletedLinesList = new List<int>();

        for (int y = 0; y < GameManager.KGirdHeight; y++) {

            bool validRow = true;

            for (int x = 0; x < GameManager.KGirdWidth; x++) {
                if (blockGrid[y,x].sprite.Equals(GameManager.Instance.backgroundBlockSprite)) {
                    validRow = false;
                    break;
                }
            }

            if (validRow)
                foundCompletedLinesList.Add(y);
        }

        if (foundCompletedLinesList.Count != 0)
            LinesFound(blockGrid, foundCompletedLinesList);
        else
            spawnNext = true;
    }
    
    private static void LinesFound (Block[,] blockGrid, List<int> foundCompletedLinesList) {
        SplitLines(blockGrid, foundCompletedLinesList);

        var count = foundCompletedLinesList.Count;
        // Update score for lines found
        Score.score += Score.scorePerLine[count - 1];

        // Update level manager linesLeft
        LevelManager.linesLeft -= count;
    }

    // Note: blockGrid goes from bottom left corner to top right corner in the world board
    /// <summary>
    /// SplitLines removes the lines that are completed and the lines above move down in place of the others (The sprites move) 
    /// </summary>
    /// <param name="blockGrid">The blockGrid to remove from</param>
    /// <param name="foundCompletedLinesList">List of all the lines found that are fully completed</param>
    private static void SplitLines (Block[,] blockGrid, List<int> foundCompletedLinesList) {
        Sprite backgroundBlockSprite = GameManager.Instance.backgroundBlockSprite;
        
        for (int i = 0; i < foundCompletedLinesList.Count; i++) {
            var startingY = foundCompletedLinesList[i] - i;
            for (int y = startingY; y < GameManager.KGirdHeight; y++) {
                var aboveY = y + 1;
                for (int x = 0; x < GameManager.KGirdWidth; x++) {
                    Block block = blockGrid[y, x];
                    Sprite newSprite = (aboveY == GameManager.KGirdHeight) ? backgroundBlockSprite : blockGrid[aboveY, x].sprite;
                    block.ChangeSprite(newSprite);
                    if (newSprite == backgroundBlockSprite)
                        block.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
        spawnNext = true;
    }
}
