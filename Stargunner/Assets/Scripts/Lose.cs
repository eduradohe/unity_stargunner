using UnityEngine;
using System.Collections;

public class Lose : MonoBehaviour {

    private int messageWidth = 200;
    private int messageHeight = 20;

    private int buttonsAreaWidth = 200;
    private int buttonsAreaHeight = 100;

    void OnGUI()
    {
        GUIStyle centeredStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
        centeredStyle.alignment = TextAnchor.UpperCenter;
        GUI.Label(
                new Rect(Screen.width / 2 - messageWidth / 2,
                         Screen.height / 4 - messageHeight / 2,
                         messageWidth,
                         messageHeight), 
                "Earth is doomed...",
                centeredStyle);
        bool mainMenu = GUI.Button(
                new Rect(Screen.width / 2 - buttonsAreaWidth / 2,
                         Screen.height / 2 - buttonsAreaHeight / 4,
                         buttonsAreaWidth,
                         buttonsAreaHeight / 2), 
                "Main Menu");
        bool tryAgain = GUI.Button(
                new Rect(Screen.width / 2 - buttonsAreaWidth / 2,
                         Screen.height / 2 + buttonsAreaHeight / 4,
                         buttonsAreaWidth,
                         buttonsAreaHeight / 2),
                "Try Again");

        if (tryAgain)
        {
            Application.LoadLevel(1);
        }
        else if (mainMenu)
        {
            Application.LoadLevel(0);
        }
    }
}
