using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

public class LevelEditorMenu : MonoBehaviour
{
    public static Dictionary<int, KeyCode> IntToKeyCodeMapping = new Dictionary<int, KeyCode>()
    {
        {1, KeyCode.Alpha1},
        {2, KeyCode.Alpha2},
        {3, KeyCode.Alpha3},
        {4, KeyCode.Alpha4},
        {5, KeyCode.Alpha5},
        {6, KeyCode.Alpha6},
        {7, KeyCode.Alpha7},
        {8, KeyCode.Alpha8},
        {9, KeyCode.Alpha9}
    };

    Dictionary<KeyCode, Vector3> KeyboardScrolling = new Dictionary<KeyCode, Vector3>()
    { 
        {KeyCode.W, new Vector3(0,0.5f,0) },
        {KeyCode.A, new Vector3(-0.5f,0,0) },
        {KeyCode.S, new Vector3(0,-0.5f,0) },
        {KeyCode.D, new Vector3(0.5f,0,0) }
    };

    private MasterMenu topButtonBars;
    private Dictionary<string, Action> buttons;

    public GameObject CubePlacer;
    public GameObject LevelManager;
    private ScreenManager screenManager;
    private string levelName;
    private LevelTypes levelType;

    private string notificationString = string.Empty;
    private string NotificationString
    {
        get
        {
            return notificationString;
        }
        set
        {
            notificationString = value;
            notificationTimer = 0;
        }
    }
    private float notificationTimer;
    private float notificationsTime = 5f;

    // Use this for initialization
    void Start()
    {
        topButtonBars = new MasterMenu(MenuHorizontalPosition.Left,
                                       MenuVerticalPosition.Top,
                                       MenuButtonLayout.Horizontal,
                                       MenuSizingType.Percentage,
                                       new Vector2(100, 10),
                                       new Vector2(3, 3),
                                       new Vector2(45, 45),
                                       new Vector2(3, 3),
                                       false);

        buttons = new Dictionary<string, Action>() 
        { 
            {"1", new Action(Select_Dirt) },
            {"2", new Action(Select_Tunnel) },
            {"3", new Action(Select_Spawn) },
            {"4", new Action(Select_EnemySpawn) },
            {"5", new Action(Select_ItemSpawner) },
            {"6", new Action(Select_Border) },
            {"7", new Action(Select_Spikes) },
            {"8", new Action(Select_SpikesTop) }
        };

        topButtonBars.Initialize(buttons);

        screenManager = LevelManager.GetComponent<ScreenManager>();

        levelName = "Default.map";
    }

    private bool showFiles = false;
    private FileInfo[] files;
    void OnGUI()
    {
        topButtonBars.OnGUI();

        levelName = GUI.TextField(new Rect(Screen.width - 185, 5, 135, 30), levelName);
        if (GUI.Button(new Rect(Screen.width - 45, 5, 40, 30), "..."))
        {
            showFiles = !showFiles;

            if (showFiles)
            {
                var info = new DirectoryInfo(".");
                files = info.GetFiles("*.map");
            }
        }

        if (showFiles && files != null)
        {
            GUI.Window(90, new Rect(Screen.width - 185, 40, 180, Screen.height - 45), filesWindow, string.Empty);
        }

        GUI.TextArea(new Rect(Screen.width - 185 - 70, 5, 65, 30), levelType.ToString());
        if (GUI.Button(new Rect(Screen.width - 185 - 105, 5, 30, 30), "S"))
            Change_TileType();

        if (notificationTimer < notificationsTime)
        {
            string toDisplay = notificationTimer < notificationsTime ? notificationString : string.Empty;
            GUI.TextArea(new Rect(5, Screen.height - 30, 200, 25), toDisplay);
        }
    }

    void filesWindow(int windowID)
    {
        int currentTop = 5;
        foreach (var file in files)
        {
            if (GUI.Button(new Rect(5, currentTop, 170, 30), file.Name))
            {
                levelName = file.Name;
            }
            currentTop += 35;
        }
    }

    // Update is called once per frame
    void Update()
    {
        notificationTimer += Time.deltaTime;

        for (int i = 0; i < buttons.Keys.Count; i++)
            if (Input.GetKeyUp(IntToKeyCodeMapping[i + 1]))
                buttons[(i + 1).ToString()].Invoke();

        foreach (var key in KeyboardScrolling.Keys)
        {
            if (Input.GetKey(key))
                Camera.main.transform.position += KeyboardScrolling[key];
        }

        if (Input.GetKey(KeyCode.Home))
            Camera.main.transform.position = new Vector3(0, 0, -10);

        if (Input.GetKey(KeyCode.F5))
            SaveLevel();
        else if (Input.GetKey(KeyCode.F7))
            LoadLevel();        
    }

    private void LoadLevel()
    {
        screenManager.ClearLevel();
        screenManager.LoadLevel(levelName, new int[] { });

        LevelAccessor la = new LevelAccessor();
        levelType = la.Level.LevelType;
        NotificationString = string.Format("Loaded '{0}'", levelName);
    }

    private void SaveLevel()
    {
        screenManager.SaveLevel(levelName);
        NotificationString = string.Format("Saved '{0}'", levelName);
    }


    void Select_Dirt()
    {
        CubePlacer.SendMessage("SwitchTile", 1);
    }

    void Select_Tunnel()
    {
        CubePlacer.SendMessage("SwitchTile", 6);
    }

    void Select_Spawn()
    {
        CubePlacer.SendMessage("SwitchTile", 7);
    }

    void Select_EnemySpawn()
    {
        CubePlacer.SendMessage("SwitchTile", 8);
    }

    void Select_Border()
    {
        CubePlacer.SendMessage("SwitchTile", 9);
    }

    void Select_ItemSpawner()
    {
        CubePlacer.SendMessage("SwitchTile", 10);
    }

    void Select_Spikes()
    {
        CubePlacer.SendMessage("SwitchTile", 11);
    }
    
    void Select_SpikesTop()
    {
        CubePlacer.SendMessage("SwitchTile", 12);
    }
    void Change_TileType()
    {
        switch(levelType)
        {
            case LevelTypes.Grass:
                levelType= LevelTypes.Snow;
                break;
            case LevelTypes.Snow:
                levelType = LevelTypes.Stone;
                break;
            case LevelTypes.Stone:
                levelType = LevelTypes.Grass;
                break;
        }
        LevelAccessor la = new LevelAccessor();
        la.Level.LevelType = levelType;
    }
}
