using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
    #region singleton
    private static bool applicationQuitting = false;
    private static GameManager _instance;
    public static GameManager Instance {
        get {
            if (applicationQuitting)
                return null;

            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }
    #endregion

    public const int KGirdHeight = 16;
    public const int KGirdWidth = 10;

    public Sprite backgroundBlockSprite;
    public GameObject pausePanel;
    public bool gameOver = false;

    public delegate void LandedDelegate(GameObject landedShape);
    public LandedDelegate landedEvent;

    public delegate void HoldDelegate(GameObject shape);
    public HoldDelegate holdEvent;

    private Block[,] _blockGrid;
    private ShapeGenerator shapeGenerator;
    private ShapeDisplayer shapeDisplayer;
    private ShapeType heldShapeType = ShapeType.None;

    public Block[,] BlockGrid {
        get { return _blockGrid; }
        set { _blockGrid = value; }
    }

    public bool HeldUsed { get; set; }

    private int[,] grid = new int[KGirdHeight, KGirdWidth] {
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
    };

    void Awake () {
        _instance = this;
	}

    private void Start() {
        shapeGenerator = FindObjectOfType<ShapeGenerator>();
        shapeDisplayer = FindObjectOfType<ShapeDisplayer>();
    }

    private void OnEnable() {
        landedEvent += Landed;
        holdEvent += Hold;
    }

    private void OnDisable() {
        landedEvent -= Landed;
        holdEvent -= Hold;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pausePanel.SetActive(true);
        }
    }

    void Landed (GameObject landedShape) {
        
        Shape shape = landedShape.GetComponent<Shape>();

        // Convert the landedShape to blocks and add to the gird
        ShapeConverter.ConvertShapesToBlocksOnGrid(shape, _blockGrid);

        if (gameOver)
            return;

        StartCoroutine(CheckLines());
    }
    
    // Once it's valid to spawn a new shape it will spawn
    IEnumerator CheckLines () {
        // Check for full line
        LineSplitter.CheckForFullLine(_blockGrid);

        yield return new WaitUntil(() => LineSplitter.spawnNext == true);

        // Spawn a new block
        shapeGenerator.SpawnShape();
    }
    
    void Hold(GameObject holdShape) {
        // Hide the gameObject 
        holdShape.SetActive(false);

        ShapeType previousShapeType = heldShapeType;

        heldShapeType = holdShape.GetComponent<Shape>().type;
        
        // Update the shape display
        shapeDisplayer.UpdateHeldDisplay(holdShape);

        // Spawn the new next shape, either it be from the previousHeldShapeType, or dequeue a new one
        if (previousShapeType != ShapeType.None) {
            shapeGenerator.SpawnShapeByType(previousShapeType);
        } else {
            shapeGenerator.SpawnShape();
        }
    }
    
    public void GameOver() {
        gameOver = true;
        Pause pause = pausePanel.GetComponent<Pause>();
        pause.ResumeButton.enabled = false;
        pause.SetMainText("Game Over");
        pause.DisplayScore(Score.score);
        pausePanel.SetActive(true);
    }

}
