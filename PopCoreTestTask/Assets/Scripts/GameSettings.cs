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


    [SerializeField]
    private List<BallSetting> BallSettings;
}
