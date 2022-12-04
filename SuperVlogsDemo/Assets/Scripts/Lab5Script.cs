using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lab5Script : MonoBehaviour
{
    public bool isMovingInCycle = true;
    public Transform[] points;
    

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        for (int a = 0; a < points.Length; a++)
        {
            if (!isMovingInCycle && (a == points.Length - 1 || a == points.Length - 2 || a == 0))
            {
                continue;
            }

            ShowSpline(a);
        }
    }

    int ControlPositions(int controlledPosition)
    {
        if (controlledPosition < 0)
        {
            controlledPosition = points.Length - 1;
        }
        if (controlledPosition > points.Length - 1)
        {
            controlledPosition = 0;
        }
        else if (controlledPosition > points.Length)
        {
            controlledPosition = 1;
        }
        

        return controlledPosition;
    }

    public static Vector3 CatMullEquation(float currentPosition, Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4)
    {
        Vector3 q = 2f * point2;
        Vector3 w = point3 - point1;
        Vector3 e = 2f * point1 - 5f * point2 + 4f * point3 - point4;
        Vector3 r = -point1 + 3f * point2 - 3f * point3 + point4;

        Vector3 equation = 0.5f * (q + (w * currentPosition) + (e * currentPosition * currentPosition) 
            + (r * currentPosition * currentPosition * currentPosition));

        return equation;
    }

    void ShowSpline(int pointPosition)
    {
        float splineResolution = 0.1f;
        int cycleCount = Mathf.FloorToInt(1f / splineResolution);
        Vector3 point1 = points[ControlPositions(pointPosition - 1)].position;
        Vector3 previousPosition = point1;
        Vector3 point2 = points[pointPosition].position;
        Vector3 point3 = points[ControlPositions(pointPosition + 1)].position;
        Vector3 point4 = points[ControlPositions(pointPosition + 2)].position;

        for (int a = 1; a <= cycleCount; a++)
        {
            float currentPosition = a * splineResolution;
            Vector3 nextPosition = CatMullEquation(currentPosition, point1, point2, point3, point4);
            Gizmos.DrawLine(previousPosition, nextPosition);
            previousPosition = nextPosition;
        }
    }
}
