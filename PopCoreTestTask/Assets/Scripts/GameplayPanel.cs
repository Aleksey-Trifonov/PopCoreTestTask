using DG.Tweening;
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
    private Color notificationEndColor = Color.clear;

    [Space(10)] [Header("Tween settings")]
    [SerializeField]
    private float notificationTime = 0.5f;
    [SerializeField]
    private float notificationScale = 1.3f;

    private Tween notificationTween = null;

    private void Awake()
    {
        currentScore.text = "0";
        currentMultiplier.text = "X1";
        progressSlider.value = 0f;
    }

    private void OnEnable()
    {
        GameplayManager.Instance.EventComboCounterChanged += OnComboCounterChanged;
        GameplayManager.Instance.EventScoreChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        GameplayManager.Instance.EventComboCounterChanged -= OnComboCounterChanged;
        GameplayManager.Instance.EventScoreChanged -= OnScoreChanged;
    }

    private void OnComboCounterChanged(int comboCounter)
    {
        if (notificationTween != null)
        {
            notificationTween.Kill();
            notification.color = Color.clear;
            notification.transform.localScale = Vector3.one;
            notificationTween = null;
        }

        var comboText = $"X{(comboCounter > 1 ? comboCounter : 1)}";
        currentMultiplier.text = comboText;
        if (comboCounter > 1)
        {
            notification.text = comboText;
            notificationTween = (notification.DOColor(notificationEndColor, notificationTime).OnStart(() =>
            {
                notification.transform.DOScale(notificationScale, notificationTime);
            })).OnComplete(() =>
            {
                notification.color = Color.clear;
                notification.transform.localScale = Vector3.one;
                notificationTween = null;
            });
        }
    }

    private void OnScoreChanged(int score)
    {
        currentScore.text = StringUtils.GetConvertedValueString(score);
        progressSlider.value = score / GameplayManager.Instance.GameSettings.LevelCompleteScore;
    }
}
