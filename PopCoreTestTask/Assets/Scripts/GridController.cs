using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private List<GridBall> activeGridBalls = new List<GridBall>();

    public void SpawnInitialRows()
    {
        for (int i = 0; i < GameplayManager.Instance.GameSettings.InitialRowCount; i++)
        {
            var isEvenRow = rowSpawnIndex % 2 == 0;
            for (int k = 0; k < ballsPerRow; k++)
            {
                var gridBall = Instantiate(gridBallPrefab, isEvenRow ? evenRowSpawnTransforms[k] : oddRowSpawnTransforms[k]);
                gridBall.transform.SetParent(gridBallsParent);
                gridBall.SetInfo(GameplayManager.Instance.GameSettings.BallSettings[Random.Range(0, GameplayManager.Instance.GameSettings.BallSettings.Count)]);
                //write logic. Maybe variable with row index
                //gridBall.SetNeighbours();
            }
            rowSpawnIndex++;

            if (i != GameplayManager.Instance.GameSettings.InitialRowCount - 1)
            {
                gridBallsParent.position -= new Vector3(0, rowDistance, 0);
            }
        }
    }

    public void SpawnNextRow()
    {
        rowSpawnIndex++;
        gridBallsParent.position -= new Vector3(0, rowDistance, 0);
        var isOddRow = rowSpawnIndex % 2 == 0;
    }
}
