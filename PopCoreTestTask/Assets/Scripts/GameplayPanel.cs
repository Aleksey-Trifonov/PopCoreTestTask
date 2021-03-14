using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentScore = null;
    [SerializeField]
    private TextMeshProUGUI currentMultiplier = null;
    [SerializeField]
    private TextMeshProUGUI notification = null;
    [SerializeField]
    private Slider progressSlider = null;
    [SerializeField]
    private TextMeshProUGUI currentLevelValue = null;
    [SerializeField]
    private TextMeshProUGUI nextLevelValue = null;

    private void Awake()
    {
        
    }

    private void OnDisable()
    {
        
    }
}
