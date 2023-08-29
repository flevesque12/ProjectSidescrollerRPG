using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //singleton
    public Player player;
    
    private static PlayerManager _instance;
    
    public static PlayerManager Instance
    {
        get {
            //if there is no instance create one
            if(_instance == null)
            {
                //check if an instance already exists in the scene
                _instance = FindObjectOfType<PlayerManager>();

                //if not create a new gameobjects and add thje mmanager component
                if(_instance == null)
                {
                    GameObject singletonObject = new GameObject("PlayerManager");
                    _instance = singletonObject.AddComponent<PlayerManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        //ensure there's only one instance of PlayerManager
        if(_instance!= null && _instance != this) {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
