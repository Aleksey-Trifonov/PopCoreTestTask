using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using System.Linq;

public class PlayerBall : Ball
{
    [SerializeField]
    private LineRenderer trajectory = null;
    [SerializeField]
    private float travelTime = 1.5f;
    [SerializeField]
    private float collisionSearchradius = 0.7f;
    [SerializeField]
    private float pushDuration = 0.2f;
    [SerializeField]
    private float pushDistance = 0.3f;

    private List<Vector3> launchPath = new List<Vector3>();
    private Tween movementTween = null;

    public void CalculateTrajectory(Vector2 direction)
    {
        var trajectoryPositions = new List<Vector3>
        {
            transform.position
        };

        var initialHit = Physics2D.Raycast(transform.position, direction.normalized, 10f);
        if (initialHit)
        {
            trajectoryPositions.Add(initialHit.point);

            if (initialHit.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                var reflectDirection = Vector2.Reflect(direction.normalized, initialHit.normal);
                var reflectedHit = Physics2D.Raycast(initialHit.point, reflectDirection.normalized, 10f);
                if (reflectedHit)
                {
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
            OnComplete(() => 
            {
                movementTween = null;
                CheckIfCollidedWithMatchingBall();
            });
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            if (movementTween != null)
            {
                movementTween.Kill();
                movementTween = null;
                CheckIfCollidedWithMatchingBall();
            }
        }
    }

    private async void CheckIfCollidedWithMatchingBall()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, collisionSearchradius, ballLayer);
        var matchingBalls = new List<GridBall>();
        foreach (var collider in colliders)
        {
            if (collider.gameObject == gameObject)
            {
                continue;
            }

            var direction = collider.transform.position - transform.position;
            collider.transform.DOMove(collider.transform.position + direction.normalized * pushDistance, pushDuration/2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Linear);

            var gridBall = collider.GetComponent<GridBall>();
            if (gridBall.Score == score)
            {
                matchingBalls.Add(gridBall);
            }
        }

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
            await Task.Delay((int)(pushDuration * 1000));
            GameplayManager.Instance.ChangeComboCounter(false);
            GridController.Instance.SpawnNextRow();

            Destroy(gameObject);
        }
    }
}
