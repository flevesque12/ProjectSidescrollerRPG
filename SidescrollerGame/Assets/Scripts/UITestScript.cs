using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UITestScript : MonoBehaviour
{
    private void OnGUI() {
        bool isClicked = GUI.Button(new Rect(0f,0f,100f,100f),"Button");
    }
}
