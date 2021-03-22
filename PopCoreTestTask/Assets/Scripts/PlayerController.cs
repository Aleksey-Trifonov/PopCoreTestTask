using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

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

    private void OnEnable()
    {
        GridController.Instance.EventInitalBallsSpawned += SetupPlayerBalls;
        GridController.Instance.EventNextRowSpawned += SetNextBall;
        GridController.Instance.EventGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        if (GridController.Instance != null)
        {
            GridController.Instance.EventInitalBallsSpawned -= SetupPlayerBalls;
            GridController.Instance.EventNextRowSpawned -= SetNextBall;
            GridController.Instance.EventGameOver -= OnGameOver;
        }
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
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

    private void SetupPlayerBalls(List<GridBall> initialBalls)
    {
        activePlayerBall = Instantiate(playerBallPrefab, activeBallPosition);
        var firstRowBalls = initialBalls.FindAll(b => b.GridData.RowIndex == 0);
        var randomFirstBal = firstRowBalls[Random.Range(0, firstRowBalls.Count)];
        firstRowBalls.Remove(randomFirstBal);
        var startingBallSettings = GameplayManager.Instance.GameSettings.BallSettings.Find(s => s.Value == randomFirstBal.Score);
        activePlayerBall.SetInfo(startingBallSettings);

        SpawnStandByBall(firstRowBalls);
        isInAimMode = true;
    }

    private void SpawnStandByBall(List<GridBall> possibleStandbyBalls = null)
    {
        standbyPlayerBall = Instantiate(playerBallPrefab, standbyBallPosition);
        standbyPlayerBall.transform.localScale = Vector3.zero;
        standbyPlayerBall.transform.DOScale(standbyBallSize, newBallAppearTimer);
        if (possibleStandbyBalls == null)
        {
            possibleStandbyBalls = GridController.Instance.GetPossibleBalls();
            if(possibleStandbyBalls.Count > 1 && possibleStandbyBalls.Find(b => b.Score == activePlayerBall.Score))
            {
                possibleStandbyBalls.Remove(possibleStandbyBalls.Find(b => b.Score == activePlayerBall.Score));
            }
        }
        var randomBallValue = possibleStandbyBalls[Random.Range(0, possibleStandbyBalls.Count)].Score;
        var standbyBallSettings = GameplayManager.Instance.GameSettings.BallSettings.Find(s => s.Value == randomBallValue);
        standbyPlayerBall.SetInfo(standbyBallSettings);
    }

    private void SetNextBall()
    {
        standbyPlayerBall.transform.DOMove(activeBallPosition.position, newBallAppearTimer).
            OnStart(() =>
            {
                standbyPlayerBall.transform.DOScale(new Vector3(activeBallSize, activeBallSize, activeBallSize), newBallAppearTimer);
            }).
            OnComplete(() =>
            {
                activePlayerBall = standbyPlayerBall;
                activePlayerBall.transform.SetParent(activeBallPosition);
                SpawnStandByBall();
                isInAimMode = true;
            });
    }

    private void OnGameOver()
    {
        isInAimMode = false;
    }
}
