using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerBall playerBallPrefab = null;
    [SerializeField]
    private Transform standbyBallPosition = null;
    [SerializeField]
    private Transform activeBallPosition = null;

    [Space(10)]
    [Header("Tween settings")]
    [SerializeField]
    private float newBallAppearTimer = 1f;
    [SerializeField]
    private float standbyBallSize = 0.15f;
    [SerializeField]
    private float activeBallSize = 0.2f;

    private PlayerBall activePlayerBall = null;
    private PlayerBall standbyPlayerBall = null;
    private bool isInAimMode = false;

    private void Start()
    {
        SetupPlayerBalls();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (isInAimMode) 
        {
            var inputPosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            activePlayerBall.CalculateTrajectory(inputPosition - (Vector2)activePlayerBall.transform.position);
        }

        if (Input.GetMouseButtonDown(0) && isInAimMode)
        {
            isInAimMode = false;
            activePlayerBall.Launch();
        }
#else
        //mobile input goes here
#endif
    }

    private void SetupPlayerBalls()
    {
        activePlayerBall = Instantiate(playerBallPrefab, activeBallPosition);
        activePlayerBall.SetInfo(GameplayManager.Instance.GameSettings.BallSettings[Random.Range(0, GameplayManager.Instance.GameSettings.BallSettings.Count)]);
        activePlayerBall.EventPlayerBallDestroyed += SetNextBall;

        SpawnStandByBall();
        isInAimMode = true;
    }

    private void SpawnStandByBall()
    {
        standbyPlayerBall = Instantiate(playerBallPrefab, standbyBallPosition);
        standbyPlayerBall.transform.localScale = Vector3.zero;
        standbyPlayerBall.transform.DOScale(standbyBallSize, newBallAppearTimer);
        standbyPlayerBall.SetInfo(GameplayManager.Instance.GameSettings.BallSettings[Random.Range(0, GameplayManager.Instance.GameSettings.BallSettings.Count)]);
    }

    private void SetNextBall()
    {
        activePlayerBall.EventPlayerBallDestroyed -= SetNextBall;
        standbyPlayerBall.transform.DOMove(activeBallPosition.position, newBallAppearTimer).
            OnStart(() =>
            {
                standbyPlayerBall.transform.DOScale(new Vector3(activeBallSize, activeBallSize, activeBallSize), newBallAppearTimer);
            }).
            OnComplete(() =>
            {
                activePlayerBall = standbyPlayerBall;
                activePlayerBall.transform.SetParent(activeBallPosition);
                activePlayerBall.EventPlayerBallDestroyed += SetNextBall;
                SpawnStandByBall();
                isInAimMode = true; //todo change
            });
    }
}
