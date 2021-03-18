using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBall : Ball
{
    private List<GridBall> topNeighbours = new List<GridBall>();
    private List<GridBall> bottomNeighbours = new List<GridBall>();

    public void SetNeighbours(List<GridBall> topNeighboursToAssign, List<GridBall> bottomNeighboursToAssign)
    {
        topNeighbours = topNeighboursToAssign;
        bottomNeighbours = bottomNeighboursToAssign;
    }
}
