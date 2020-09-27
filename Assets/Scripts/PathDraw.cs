using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathDraw : MonoBehaviour
{
    private LineRenderer boatPath;
    private List<Vector3> points = new List<Vector3>();
    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { };
    private Vector3 mOffset;
    private float mZCoord;
    void Start()
    {
        boatPath = GetComponent<LineRenderer>();
    }

    void Update()
    {
        
    }

    void OnMouseDown()
    {
        points.Clear();
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    private void OnMouseDrag()
    {
        //Debug.Log(GetMouseAsWorldPoint() + mOffset);
        Vector3 mousePoint = GetMouseAsWorldPoint() + mOffset;
        if(DistanceToLastPoint(mousePoint) > 1f)
        {
            points.Add(mousePoint);
            boatPath.positionCount = points.Count;
            boatPath.SetPositions(points.ToArray());
        }
    }

    private Vector3 GetMouseAsWorldPoint()

    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private float DistanceToLastPoint(Vector3 point)
    {
        if (!points.Any())
        {
            return Mathf.Infinity;
        }
        else
        {
            return Vector3.Distance(points.Last(), point);
        }
    }
}
