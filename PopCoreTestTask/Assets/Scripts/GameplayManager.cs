using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameplayManager>();
            }
            return instance;
        }
    }

    private static GameplayManager instance = null;

    public GameSettings GameSettings
    {
        get
        {
            return gameSettings;
        }
    }

    [SerializeField]
    private GameSettings gameSettings = null;

    private void Start()
    {
        GridController.Instance.SpawnInitialRows();
    }
}
