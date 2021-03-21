using UnityEngine;
using DG.Tweening;
using System.Linq;
using System.Collections.Generic;

public class Ball : MonoBehaviour
{
    public int Score
    {
        get 
        {
            return score;
        }
        set
        {
            if (score != value)
            {
                score = value;
            }
        }
    }

    protected int score = 0;

    [SerializeField]
    protected SpriteRenderer image = null;
    [SerializeField]
    protected TextMesh text = null;
    [SerializeField]
    protected CircleCollider2D circleCollider = null;

    protected int ballLayer = 0;

    private void Awake()
    {
        ballLayer = LayerMask.GetMask("Ball");
    }

    public void SetInfo(BallSetting ballSetting)
    {
        image.color = ballSetting.Color;
        text.text = ballSetting.Value.ToString();
        score = ballSetting.Value;
    }

    protected void MergeBalls(GridBall mergeBall)
    {
        transform.DOMove(mergeBall.transform.position, 0.5f).OnComplete(() =>
        {
            var newData = GameplayManager.Instance.GameSettings.BallSettings.FindAll(d => d.Value <= score * 2).OrderByDescending(d => d.Value).First();
            mergeBall.SetInfo(newData);
            //add score
            //spawn particles
            mergeBall.CheckForNextMerge();
            Destroy(gameObject);
        });
    }
}
