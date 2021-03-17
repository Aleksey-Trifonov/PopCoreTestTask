using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BallSetting
{
    public int Value;
    public Color Color;
}

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public List<BallSetting> BallSettings;

    public int LevelCompleteScore = 10000;
}
