using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum MenuButtonLayout { Horizontal, Vertical };
public enum MenuSizingType { Percentage, Pixels };
public enum MenuHorizontalPosition { Left, Center, Right };
public enum MenuVerticalPosition { Top, Center, Bottom };

public class MasterMenu
{
    private static int IDCounter;

    private int ID;
    private MenuHorizontalPosition horizontalPosition;
    private MenuVerticalPosition verticalPosition;
    private MenuButtonLayout buttonLayout;
    private MenuSizingType sizingType;
    private Vector2 menuSize;
    private Vector2 menuPadding;
    private Vector2 buttonSize;
    private Vector2 buttonPadding;
    private Dictionary<string, Action> buttons;

    private Vector2 buttonPixelsPadding;
    private Vector2 buttonPixelsSize;
    private Vector2 menuPixelsPadding;
    private Vector2 menuPixelsSize;
    private Vector2 topLeftPosition;
    private Rect windowRectangle;

    private bool showMenuWindow;
    private Vector2 buttonSizeIncrement;

    public MasterMenu(MenuHorizontalPosition horizontalPosition, MenuVerticalPosition verticalPosition, MenuButtonLayout buttonLayout, MenuSizingType sizingType, Vector2 menuSize, Vector2 menuPadding, Vector2 buttonSize, Vector2 buttonPadding, bool showMenuWindow)
    {
        this.horizontalPosition = horizontalPosition;
        this.verticalPosition = verticalPosition;
        this.buttonLayout = buttonLayout;
        this.sizingType = sizingType;
        this.menuSize = menuSize;
        this.menuPadding = menuPadding;
        this.buttonSize = buttonSize;
        this.buttonPadding = buttonPadding;
        this.showMenuWindow = showMenuWindow;

        ID = IDCounter++;
    }

    public void Initialize(Dictionary<string, Action> buttons)
    {
        this.buttons = buttons;
        CalculateMenuSizes();
    }

    public void CalculateMenuSizes()
    {
        if (sizingType == MenuSizingType.Pixels)
        {
            menuPixelsSize = menuSize;
            menuPixelsPadding = menuPadding;
        }
        else
        {
            float singleXPercentage = Screen.width / 100f;
            float singleYPercentage = Screen.height / 100f;

            menuPixelsSize = new Vector2(menuSize.x * singleXPercentage, menuSize.y * singleYPercentage);
            menuPixelsPadding = new Vector2(menuPixelsPadding.x * singleXPercentage, menuPixelsPadding.y * singleYPercentage);
        }

        float menuLeftX = 0;
        switch (horizontalPosition)
        {
            case MenuHorizontalPosition.Left:
                menuLeftX = menuPixelsPadding.x;
                break;
            case MenuHorizontalPosition.Center:
                menuLeftX = Screen.width / 2f - menuPixelsSize.x / 2f;
                break;
            case MenuHorizontalPosition.Right:
                menuLeftX = Screen.width - menuPixelsSize.x - menuPixelsPadding.x;
                break;
        }

        float menuTopX = 0;
        switch (verticalPosition)
        {
            case MenuVerticalPosition.Top:
                menuTopX = menuPixelsPadding.y;
                break;
            case MenuVerticalPosition.Center:
                menuTopX = Screen.height / 2f - menuPixelsSize.y / 2f;
                break;
            case MenuVerticalPosition.Bottom:
                menuTopX = Screen.height - menuPixelsSize.y - menuPixelsPadding.y;
                break;
        }

        windowRectangle = new Rect(menuLeftX, menuTopX, menuPixelsSize.x, menuPixelsSize.y);

        if (buttonSize != Vector2.zero) //size is set
        {
            buttonPixelsSize = buttonSize;
            buttonPixelsPadding = buttonPadding;
        }
        else //Calculate size from available space.
        {
            float singleButtonXPercentage = menuPixelsSize.x / 100f;
            float singleButtonYPercentage = menuPixelsSize.y / 100f;

            buttonPixelsPadding = new Vector2(buttonPadding.x * singleButtonXPercentage, buttonPadding.y * singleButtonYPercentage);

            int paddingsX = 2;
            int paddingsY = 2;
            if (buttonLayout == MenuButtonLayout.Horizontal)
                paddingsX += buttons.Count - 1;
            else
                paddingsY += buttons.Count - 1;

            var remainingXSpace = menuPixelsSize.x - (buttonPixelsPadding.x * paddingsX);
            var remainingYSpace = menuPixelsSize.y - (buttonPixelsPadding.y * paddingsY);

            if (buttonLayout == MenuButtonLayout.Horizontal)
                buttonPixelsSize = new Vector2(remainingXSpace / buttons.Count, remainingYSpace);
            else
                buttonPixelsSize = new Vector2(remainingXSpace, remainingYSpace / buttons.Count);
        }

        if (buttonLayout == MenuButtonLayout.Horizontal)
            buttonSizeIncrement = new Vector2(buttonPixelsPadding.x + buttonPixelsSize.x, 0);
        else
            buttonSizeIncrement = new Vector2(0, buttonPixelsPadding.y + buttonPixelsSize.y);
    }

    public void OnGUI()
    {
        if (showMenuWindow)
            GUI.Window(ID, windowRectangle, OnWindow, "");
        else
            OnButtons(windowRectangle);
    }

    void OnButtons(Rect Offset)
    {
        var currentPosition = new Rect(Offset.x + buttonPixelsPadding.x, Offset.y + buttonPixelsPadding.y, buttonPixelsSize.x, buttonPixelsSize.y);

        foreach (var btn in buttons)
        {
            if (GUI.Button(currentPosition, btn.Key.ToString()))
                btn.Value.Invoke();

            currentPosition.x += buttonSizeIncrement.x;
            currentPosition.y += buttonSizeIncrement.y;
        }
    }

    void OnWindow(int windowID)
    {
        OnButtons(new Rect(0,0,0,0));
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
