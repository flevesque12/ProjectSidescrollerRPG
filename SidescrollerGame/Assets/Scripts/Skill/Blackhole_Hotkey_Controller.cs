using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_Hotkey_Controller : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private KeyCode hotkey;
    private TextMeshProUGUI text;

    private Transform enemy;
    private Blackhole_Skill_Controller blackhole;

    public void SetupHotkey(KeyCode _newHotkey, Transform _enemy, Blackhole_Skill_Controller _blackhole)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        enemy = _enemy;
        blackhole = _blackhole;

        hotkey = _newHotkey;
        text.text = _newHotkey.ToString();
    }

    void Update()
    {
        if(Input.GetKeyDown(hotkey))
        {
            blackhole.AddEnemyToList(enemy);

            text.color = Color.clear;
            spriteRenderer.color = Color.clear;
        }
        
    }
}
