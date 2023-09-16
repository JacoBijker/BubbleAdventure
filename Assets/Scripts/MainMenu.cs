using UnityEngine;
using System.Collections;
using Assets.Scripts.Standalone;

public class MainMenu : MonoBehaviour
{
    public enum OpenMenu
    {
        Main,
        Credits,
        Play
    }


    public GUIStyle MainMenuStyle;
    public GUIStyle CreditsMenuStyle;

    public GUIStyle PlayButtonStyle;
    public GUIStyle CreditsButtonStyle;
    public GUIStyle QuitButtonStyle;

    public GUIStyle RoundButtonBack;

    private OpenMenu currentMenu = OpenMenu.Main;

    private GUIScaler scaler;
    // Use this for initialization
    void Start()
    {
        scaler = new GUIScaler();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        scaler.Set(GUI.matrix);
        GUI.matrix = scaler.GetScaled();
        
        var x = 272;
        float y = 24;

        switch (currentMenu)
        {
            case OpenMenu.Main:
                GUI.Window(5600, new Rect(x, y, 479, 549), MainMenuWindow, "", MainMenuStyle);
                break;
            case OpenMenu.Credits:
                GUI.Window(5601, new Rect(x, y, 479, 549), CreditsMenu, "", CreditsMenuStyle);
                break;
        }

        GUI.matrix = scaler.GetOriginal();
    }

    void CreditsMenu(int windowID)
    {
        var x = 479 / 2 - 69 / 2;
        var y = 549 - 71;
        if (GUI.Button(new Rect(x, y, 69, 71), string.Empty, RoundButtonBack))
            currentMenu = OpenMenu.Main;
    }

    void MainMenuWindow(int windowID)
    {
        float x = 135f;
        float y = 180;
        if (GUI.Button(new Rect(x, y, 210, 69), string.Empty, PlayButtonStyle))
        {
            SceneInformationMedium.SetCampaign("Default.cmp");
            Application.LoadLevel("GameScene");
        }

        if (GUI.Button(new Rect(x, y + 80, 210, 69), string.Empty, CreditsButtonStyle))
            currentMenu = OpenMenu.Credits;

        if (GUI.Button(new Rect(x, y + 160, 210, 69), string.Empty, QuitButtonStyle))
            Application.Quit();
    }
}
