using System;
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

    public int ComboCounter
    {
        get
        {
            return comboCounter;
        }
    }

    public event Action<int> EventScoreChanged = null;
    public event Action<int> EventComboCounterChanged = null;

    [SerializeField]
    private GameSettings gameSettings = null;

    private int score = 0;
    private int comboCounter = 0;

    private void Start()
    {
        GridController.Instance.SpawnInitialRows();
    }

    public void AddScore(int scoreToAdd)
    {
        if (comboCounter > 1)
        {
            scoreToAdd *= comboCounter;
        }
        score += scoreToAdd;
        EventScoreChanged?.Invoke(score);
    }

    public void ChangeComboCounter(bool isCombo)
    {
        if (isCombo)
        {
            comboCounter++;
        }
        else
        {
            comboCounter = 0;
        }
        EventComboCounterChanged?.Invoke(comboCounter);
    }
}
