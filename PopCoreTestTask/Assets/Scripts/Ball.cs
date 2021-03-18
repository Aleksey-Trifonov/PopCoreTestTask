using UnityEngine;
using DG.Tweening;

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

    private int score = 0;

    [SerializeField]
    protected SpriteRenderer image = null;
    [SerializeField]
    protected TextMesh text = null;
    [SerializeField]
    protected CircleCollider2D circleCollider = null;

    public void SetInfo(BallSetting ballSetting)
    {
        image.color = ballSetting.Color;
        text.text = ballSetting.Value.ToString();
    }

    protected void Merge()
    {
        
    }
}
