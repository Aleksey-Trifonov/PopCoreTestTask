using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreVisalizer : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem mergeEffectParticles = null;
    [SerializeField]
    private AudioSource mergeEffectAudio = null;
    [SerializeField]
    private TextMesh notificationText = null;

    [Space(10)]
    [Header("Score tween settings")]
    [SerializeField]
    private float distance = 0.3f;
    [SerializeField]
    private float duration = 0.5f;

    public void PlayScoreEffects(int score)
    {
        mergeEffectParticles.Play();
        mergeEffectAudio.Play();

        notificationText.text = score.ToString();
        notificationText.transform.DOMoveY(transform.position.y + distance, duration).OnComplete(() => 
        {
            Destroy(gameObject);
        });
    }
}
