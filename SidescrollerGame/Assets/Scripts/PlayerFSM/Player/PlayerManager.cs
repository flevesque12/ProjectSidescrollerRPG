using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //singleton
    public static PlayerManager instance;
    public Player player;

    private void Awake()
    {
        instance = this;
    }
}
