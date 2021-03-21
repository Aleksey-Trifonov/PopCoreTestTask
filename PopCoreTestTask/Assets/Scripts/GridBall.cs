using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //private List<GridBall> topNeighbours = new List<GridBall>();
    //private List<GridBall> bottomNeighbours = new List<GridBall>();
    private List<GridBall> neighbours = new List<GridBall>();
    private GridData gridData = new GridData();

    //public void SetNeighbours(List<GridBall> topNeighboursToAssign, List<GridBall> bottomNeighboursToAssign)
    //{
    //    topNeighbours = topNeighboursToAssign;
    //    bottomNeighbours = bottomNeighboursToAssign;
    //}

    public void SetGridData(int rowIndex, int rowPosition)
    {
        gridData.RowIndex = rowIndex;
        gridData.RowPosition = rowPosition;
    }

    public void CollectNeighbours()
    {
        neighbours.Clear();
        var colliders = Physics2D.OverlapCircleAll((Vector2)transform.position, 0.5f, LayerMask.GetMask("Ball"));
        //Debug.DrawLine(transform.position, transform.position + new Vector3(0, 0.5f, 0), Color.red, 5f);
        foreach (var collider in colliders)
        {
            if (collider.gameObject == gameObject)
            {
                continue;
            }

            neighbours.Add(collider.gameObject.GetComponent<GridBall>());
        }
    }
}
