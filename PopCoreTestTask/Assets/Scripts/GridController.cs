using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using System;
using Random = UnityEngine.Random;

public class GridController : MonoBehaviour
{
    public static GridController Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GridController>();
            }
            return instance;
        }
    }

    private static GridController instance = null;

    public event Action<List<GridBall>> EventInitalBallsSpawned = null;

    [SerializeField]
    private GridBall gridBallPrefab = null;
    [SerializeField]
    private Transform[] oddRowSpawnTransforms = null;
    [SerializeField]
    private Transform[] evenRowSpawnTransforms = null;
    [SerializeField]
    private Transform gridBallsParent = null;
    [SerializeField]
    private int ballsPerRow = 6;
    [SerializeField]
    private float rowDistance = 0.7f;

    private int rowSpawnIndex = 0;
    private List<GridBall> lastSpawnedRow = new List<GridBall>();

    public async void SpawnInitialRows()
    {
        var initialBalls = new List<GridBall>();
        for (int i = 0; i < GameplayManager.Instance.GameSettings.InitialRowCount; i++)
        {
            var isEvenRow = rowSpawnIndex % 2 == 0;
            for (int k = 0; k < ballsPerRow; k++)
            {
                var gridBall = Instantiate(gridBallPrefab, isEvenRow ? evenRowSpawnTransforms[k] : oddRowSpawnTransforms[k]);
                gridBall.transform.SetParent(gridBallsParent);
                gridBall.SetInfo(GameplayManager.Instance.GameSettings.BallSettings[Random.Range(0, GameplayManager.Instance.GameSettings.BallSettings.Count)]);
                gridBall.SetGridData(rowSpawnIndex, k);
                initialBalls.Add(gridBall);

                if (i == GameplayManager.Instance.GameSettings.InitialRowCount - 1)
                {
                    lastSpawnedRow.Add(gridBall);
                }
            }
            rowSpawnIndex++;

            if (i != GameplayManager.Instance.GameSettings.InitialRowCount - 1)
            {
                gridBallsParent.position -= new Vector3(0, rowDistance, 0);
            }
        }

        await Task.Delay(10);

        foreach (var ball in initialBalls)
        {
            ball.CollectNeighbours();
        }

        EventInitalBallsSpawned?.Invoke(initialBalls);
    }

    public void SpawnNextRow()
    {
        var newRow = new List<GridBall>();
        gridBallsParent.DOMoveY(gridBallsParent.position.y - rowDistance, 1f).OnComplete(() => 
        {
            //check if game over
            var isEvenRow = rowSpawnIndex % 2 == 0;
            for (int i = 0; i < ballsPerRow; i++)
            {
                var gridBall = Instantiate(gridBallPrefab, isEvenRow ? evenRowSpawnTransforms[i] : oddRowSpawnTransforms[i]);
                gridBall.transform.SetParent(gridBallsParent);
                gridBall.SetInfo(GameplayManager.Instance.GameSettings.BallSettings[Random.Range(0, GameplayManager.Instance.GameSettings.BallSettings.Count)]);
                gridBall.SetGridData(rowSpawnIndex, i);
                newRow.Add(gridBall);
            }
            rowSpawnIndex++;

            foreach (var ball in lastSpawnedRow)
            {
                if (ball != null)
                {
                    ball.CollectNeighbours();
                }
            }

            foreach (var ball in newRow)
            {
                ball.CollectNeighbours();
            }

            lastSpawnedRow = newRow;
        });
    }
}
