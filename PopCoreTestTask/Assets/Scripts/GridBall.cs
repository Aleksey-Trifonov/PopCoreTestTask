using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

[System.Serializable]
public struct GridData
{
    public int RowIndex;
    public int RowPosition;
}

public class GridBall : Ball
{
    public GridData GridData
    {
        get 
        {
            return gridData;
        }
    }

    private GridData gridData = new GridData();

    [SerializeField]
    private float neighboursSearchRadius = 0.5f;
    [SerializeField]
    private ScoreVisalizer scoreVisualizerPrefab = null;

    private List<GridBall> neighbours = new List<GridBall>();

    public void SetGridData(int rowIndex, int rowPosition)
    {
        gridData.RowIndex = rowIndex;
        gridData.RowPosition = rowPosition;
    }

    public void CollectNeighbours()
    {
        neighbours.Clear();
        var colliders = Physics2D.OverlapCircleAll(transform.position, neighboursSearchRadius, ballLayer);
        foreach (var collider in colliders)
        {
            if (collider.gameObject == gameObject)
            {
                continue;
            }

            neighbours.Add(collider.gameObject.GetComponent<GridBall>());
        }
    }

    public List<GridBall> GetMatchingNeighbours(int targetScore)
    {
        var matchingBalls = new List<GridBall>();

        foreach (var neighbourBall in neighbours)
        {
            if (neighbourBall != null && neighbourBall.isInGrid && neighbourBall.Score == targetScore)
            {
                matchingBalls.Add(neighbourBall);
            }
        }

        return matchingBalls;
    }

    public void ProcessMerge()
    {
        GameplayManager.Instance.ChangeComboCounter(true);
        GameplayManager.Instance.AddScore(score);
        var scoreVisualizer = Instantiate(scoreVisualizerPrefab, transform.position, Quaternion.identity);
        scoreVisualizer.PlayScoreEffects(score * GameplayManager.Instance.ComboCounter);
        CheckForNextMerge();
    }

    public void CheckForNextMerge()
    {
        var matchingBalls = GetMatchingNeighbours(score);
        if (matchingBalls.Count > 1)
        {
            foreach (var matchingBall in matchingBalls)
            {
                if (matchingBall.GetMatchingNeighbours(Mathf.Min(score * 2, GameplayManager.Instance.GameSettings.BallSettings.Max(s => s.Value))).Count != 0)
                {
                    MergeBalls(matchingBall);
                    return;
                }
            }

            MergeBalls(matchingBalls[0]);
        }
        else if (matchingBalls.Count == 1)
        {
            MergeBalls(matchingBalls[0]);
        }
        else
        {
            isInGrid = false;
            GameplayManager.Instance.ChangeComboCounter(false);
            GridController.Instance.SpawnNextRow();
            CheckBottomNeighbours();
            Destroy(gameObject);
        }
    }

    private void CheckIfIsolated()
    {
        var topNeighbours = neighbours.FindAll(n => n != null && n.isInGrid && n.GridData.RowIndex == gridData.RowIndex + 1);
        if (topNeighbours.Count == 0)
        {
            Fall();
            CheckBottomNeighbours();
        }
    }

    private void CheckBottomNeighbours()
    {
        var bottomNeighbours = neighbours.FindAll(n => n != null && n.isInGrid && n.GridData.RowIndex == gridData.RowIndex - 1);
        if (bottomNeighbours.Count > 0)
        {
            foreach (var bottomNeighbour in bottomNeighbours)
            {
                bottomNeighbour.CheckIfIsolated();
            }
        }
    }

    private void Fall()
    {
        isInGrid = false;
        circleCollider.enabled = false;
        transform.DOMoveY(-6f, Random.Range(1f, 2f)).OnComplete(() =>
        {
            GameplayManager.Instance.AddScore(score);
            var scoreVisualizer = Instantiate(scoreVisualizerPrefab, transform.position, Quaternion.identity);
            scoreVisualizer.PlayScoreEffects(score);
            Destroy(gameObject);
        });
    }

    private void OnDestroy()
    {
        if (GridController.Instance != null)
        {
            GridController.Instance.RemoveGridBall(this);
        }
    }
}
