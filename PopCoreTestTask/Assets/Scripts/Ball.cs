using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    [SerializeField]
    protected SpriteRenderer image = null;
    [SerializeField]
    protected TextMesh text = null;

    private CircleCollider2D collider = null;

    public virtual void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
    }

    public void SetInfo(BallSetting ballSetting)
    {
        image.color = ballSetting.Color;
        text.text = ballSetting.Value.ToString();
    }
}
