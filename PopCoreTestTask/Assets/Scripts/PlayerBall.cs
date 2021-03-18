using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerBall : Ball
{
    public event Action EventPlayerBallDestroyed = null;

    [SerializeField]
    private LineRenderer trajectory = null;
    [SerializeField]
    private float travelTime = 1.5f;

    private List<Vector3> launchPath = new List<Vector3>();
    private Tween movementTween = null;

    public void CalculateTrajectory(Vector2 direction)
    {
        var trajectoryPositions = new List<Vector3>
        {
            transform.position
        };

        var initialHit = Physics2D.Raycast(transform.position, direction.normalized, 10f);
        //Debug.DrawRay(transform.position, direction.normalized * 10f, Color.red);
        //Debug.Log(initialHit.collider.gameObject.name);
        if (initialHit)
        {
            trajectoryPositions.Add(initialHit.point);

            if (initialHit.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                var reflectDirection = Vector2.Reflect(direction.normalized, initialHit.normal);
                var reflectedHit = Physics2D.Raycast(initialHit.point, reflectDirection.normalized, 10f);
                //Debug.DrawRay(initialHit.point, reflectDirection.normalized * 10f, Color.red);
                if (reflectedHit)
                {
                    //Debug.Log(reflectedHit.collider.gameObject.name);
                    trajectoryPositions.Add(reflectedHit.point);
                }
            }
        }

        trajectory.positionCount = trajectoryPositions.Count;
        for (int i = 0; i < trajectory.positionCount; i++)
        {
            trajectory.SetPosition(i, trajectoryPositions[i]);
        }

        launchPath = trajectoryPositions;
    }

    public void Launch()
    {
        circleCollider.enabled = true;
        trajectory.positionCount = 0;
        movementTween = transform.DOPath(launchPath.ToArray(), travelTime, PathType.Linear, PathMode.TopDown2D).SetEase(Ease.Linear).
            OnComplete(() => CheckIfMatchingBall());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.GetMask("Ball"))
        {
            
        }
    }

    private void CheckIfMatchingBall()
    {
        //check for grid balls
        EventPlayerBallDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
