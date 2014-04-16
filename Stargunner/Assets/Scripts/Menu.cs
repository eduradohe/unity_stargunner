using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    private int buttonsAreaWidth = 200;
    private int buttonsAreaHeight = 50;

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 200), "Instructions:\nFire: Left Ctrl\nBomb: Space\nMove: Directional Keys");
        bool survival = GUI.Button(
                new Rect(Screen.width / 2 - buttonsAreaWidth / 2,
                         Screen.height / 2 - buttonsAreaHeight / 2,
                         buttonsAreaWidth,
                         buttonsAreaHeight), 
                "Survival");
        if (survival)
        {
            Application.LoadLevel(1);
        }
    }
}
