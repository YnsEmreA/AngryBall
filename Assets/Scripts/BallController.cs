using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float power = 3f;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private LineRenderer _lineRenderer;
    private Vector2 dragStartPos;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 _velocity = (dragEndPos - dragStartPos) * power;

            Vector2[] trajectory = Plot(_rigidbody2D, (Vector2)transform.position, _velocity, 500);
            _lineRenderer.positionCount = trajectory.Length;

            Vector3[] positions = new Vector3[trajectory.Length];

            for (int i = 0; i < trajectory.Length; i++)
            {
                positions[i] = trajectory[i];
            }
            
            _lineRenderer.SetPositions(positions);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 _velocity = (dragEndPos - dragStartPos) * power;

            _rigidbody2D.velocity = _velocity;
        }
    }

    public Vector2[] Plot(Rigidbody2D rb, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * rb.gravityScale * timestep * timestep;

        float drag = 1f - timestep * rb.drag;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }

        return results;
    }
}
