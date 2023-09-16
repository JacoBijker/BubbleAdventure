using UnityEngine;
using System.Collections;
using Assets.Scripts.Player;
using Assets.Scripts.Standalone;

public enum HUDMenus
{
    Normal,
    GameOver,
    Congratulations
}

public class GameHUD : MonoBehaviour
{
    private HUDMenus currentHUDMenu = HUDMenus.Normal;

    public GUIStyle fontStyle;
    public GUIStyle darkFontStyle;
    public Texture2D LifeBar;
    public Texture2D PointsBar;
    public GUIStyle LevelBar;
    public GUIStyle PausedMenu;

    public GUIStyle ResumeButton;
    public GUIStyle MainMenuButton;

    public Texture2D GrayedOutScreen;

    public GUIStyle GameOverMenu;
    public GUIStyle CongratulationsMenu;

    private PlayerAccessor playerAccessor;
    private GUIScaler guiScaler;
    private bool IsPaused
    {
        get
        {
            return Time.timeScale <= 0.01f;
        }
        set
        {
            if (value)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }
    }

    void Start()
    {
        guiScaler = new GUIScaler();   
    }

    void Awake()
    {
        playerAccessor = playerAccessor ?? new PlayerAccessor();
        IsPaused = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        guiScaler.Set(GUI.matrix);
        GUI.matrix = guiScaler.GetScaled();

        var x = 272;
        float y = 24;

        //var x = Screen.width / 2 - 479 / 2;
        //float y = Mathf.Max(0, Screen.height - 549);
        //y *= 0.25f;

        switch (currentHUDMenu)
        {
            case HUDMenus.GameOver:
                GUI.Window(5600, new Rect(x, y, 479, 549), GameOverWindow, "", GameOverMenu);
                break;
            case HUDMenus.Congratulations:
                GUI.Window(5600, new Rect(x, y, 479, 549), CongratulationsWindow, "", CongratulationsMenu);
                break;
            default:
                GUI.DrawTexture(new Rect(5, 9, 191, 57), LifeBar);
                GUI.DrawTexture(new Rect(201, 0, 196, 67), PointsBar);
                if (GUI.Button(new Rect(823, 0, 196, 68), string.Empty, LevelBar))
                    if (!IsPaused)
                        IsPaused = !IsPaused;

                GUI.Label(new Rect(80, 20, 50, 40), playerAccessor.Player.Lives.ToString(), fontStyle);
                GUI.Label(new Rect(281, 20, 50, 40), playerAccessor.Player.Score.ToString(), fontStyle);
                GUI.Label(new Rect(903, 21, 76, 56), playerAccessor.Player.Level.ToString(), fontStyle);

                if (IsPaused)
                {
                    var total = 1440f;
                    GUI.DrawTexture(new Rect(-208, -420, total, total), GrayedOutScreen);
                    GUI.Window(1338, new Rect(317, 127, 391, 177), PausedWindow, string.Empty, PausedMenu);
                }
                break;
        }
        GUI.matrix = guiScaler.GetOriginal();
    }

    void GameOverWindow(int windowID)
    {
        string text = string.Format("You've made it to level {0} \r\nwith a score of {1}", playerAccessor.Player.Level, playerAccessor.Player.Score);
        GUI.Label(new Rect(30, 120, 419, 469), text, darkFontStyle);

        var x = 479 / 2 - 69 / 2;
        var y = 549 - 71;
        if (GUI.Button(new Rect(x, y, 69, 71), string.Empty, MainMenuButton))
        {
            SceneInformationMedium.ClearTask();
            Application.LoadLevel("MainMenu");
        }
    }

    void CongratulationsWindow(int windowID)
    {
        string text = string.Format("You have successfully\r\ncompleted all {0} levels!\r\n\r\nYour total score is {1}", playerAccessor.Player.Level, playerAccessor.Player.Score);
        GUI.Label(new Rect(30, 120, 419, 469), text, darkFontStyle);

        var x = 479 / 2 - 69 / 2;
        var y = 549 - 71;
        if (GUI.Button(new Rect(x, y, 69, 71), string.Empty, MainMenuButton))
        {
            SceneInformationMedium.ClearTask();
            Application.LoadLevel("MainMenu");
        }
    }

    void PausedWindow(int windowID)
    {

        if (GUI.Button(new Rect(95, 80, 69, 71), string.Empty, MainMenuButton))
        {
            SceneInformationMedium.ClearTask();
            Application.LoadLevel("MainMenu");
        }

        if (GUI.Button(new Rect(225, 80, 69, 71), string.Empty, ResumeButton))
        {
            if (IsPaused)
                IsPaused = !IsPaused;
        }
    }

    public void SetMenu(HUDMenus newMenu)
    {
        this.currentHUDMenu = newMenu;
    }
}
