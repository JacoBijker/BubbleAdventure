using UnityEngine;
using System.Collections;
using System.Linq;

public class CubePlacer : MonoBehaviour
{

    public GameObject LevelManagerObject;
    private ScreenManager screenManager;
    private Transform cube;

    private int currentTileType = 0;

    private float cubeSize = 0.692f;

    private TileType selectedTile;
    private int selectionTileIndex = 8;
    // Use this for initialization
    void Start()
    {
        cube = this.GetComponentsInChildren<Transform>()[1];
        screenManager = LevelManagerObject.GetComponent<ScreenManager>();
    }

    // Update is called once per frame
    void Update()
    {
        var inputVector = Input.mousePosition;
        if (inputVector.y > Screen.height * 0.85f)
            return;

        inputVector.z = 0 - Camera.main.transform.position.z;
        var cubePosition = Camera.main.ScreenToWorldPoint(inputVector);

        var midifier = cubePosition.x > 0 ? 0.346f : -0.346f;
        var ymidifier = cubePosition.y > 0 ? 0.346f : -0.346f;

        cubePosition.x = (int)((cubePosition.x + midifier) / cubeSize) * cubeSize;
        cubePosition.y = (int)((cubePosition.y + ymidifier) / cubeSize) * cubeSize;

        if (currentTileType != selectionTileIndex || selectedTile == null)
            cube.position = cubePosition;

        if (Input.GetMouseButton((int)MouseButton.Left))
            AddOrSelect(cubePosition);
        else if (Input.GetMouseButton((int)MouseButton.Right))
            DeselectOrRemove(cubePosition);

        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            var currentCameraPosition = Camera.main.transform.position;
            currentCameraPosition.z += scroll * 4f;
            Camera.main.transform.position = currentCameraPosition;
        }
    }

    void AddOrSelect(Vector3 cubePosition)
    {
        if (currentTileType == selectionTileIndex)
            SelectTile(cubePosition);
        else
            AddTile(cubePosition);
    }

    void DeselectOrRemove(Vector3 cubePosition)
    {
        if (currentTileType == selectionTileIndex)
            DeselectTile(cubePosition);
        else
            RemoveTile(cubePosition);
    }

    void SelectTile(Vector3 cubePosition)
    {
        var tileInfo = screenManager.GetTileAtPosition(cubePosition);
        if (tileInfo != null)
        {
            selectedTile = tileInfo;
        }
    }

    void DeselectTile(Vector3 cubePosition)
    {
        selectedTile = null;
    }

    void OnGUI()
    {
        if (selectedTile != null)
        {

            GUI.Window(10000, new Rect(2, Screen.height - 202, 200, 200), DrawTileMenu, string.Empty);
        }
    }

    void DrawTileMenu(int windowID)
    {
        GUI.Label(new Rect(5, 5, 190, 25), "Tile Type: " + selectedTile["Name"]);

        int currentHeight = 30;
        var allKeys = selectedTile.internalInformation.Keys.ToList();
        foreach (var key in allKeys)
        {
            if (!key.Equals("Name"))
            {
                GUI.Label(new Rect(5, currentHeight + 5, 80, 25), key);
                selectedTile.internalInformation[key] = GUI.TextField(new Rect(90, currentHeight + 5, 100, 25), selectedTile[key]);
                currentHeight += 30;
            }
        }

        if (allKeys.Count == 1)
            GUI.Label(new Rect(5, currentHeight + 5, 190, 25), "No Configurable Properties");
    }

    void AddTile(Vector3 cubePosition)
    {
        var newTile = new TileType(currentTileType, cubePosition);
        screenManager.AddTile(newTile);
    }

    void RemoveTile(Vector3 cubePosition)
    {
        var newTile = new TileType(currentTileType, cubePosition);
        screenManager.RemoveTile(newTile);
    }

    public void SwitchTile(int value)
    {
        currentTileType = value - 1;

        var newCube = Instantiate(Global.BlockTypes[value - 1], cube.position, Quaternion.identity) as GameObject;
        newCube.transform.parent = this.transform;

        DestroyImmediate(cube.gameObject);
        cube = newCube.transform;
    }
}
