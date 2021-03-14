using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : Ball
{
    [SerializeField]
    private LineRenderer trajectory = null;

    private bool isInAimMode = false;

    public override void Awake()
    {
        base.Awake();

        trajectory = GetComponentInChildren<LineRenderer>();
    }

    private void Update()
    {
        if (isInAimMode)
        {
            CalculateTrajectory();
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            LaunchBall();
        }
#else

#endif

    }

    public void CalculateTrajectory()
    {
#if UNITY_EDITOR
        
#else

#endif
    }

    private void LaunchBall()
    {
        
    }
}
