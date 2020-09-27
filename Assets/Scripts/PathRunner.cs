using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathRunner : MonoBehaviour
{
    private LineRenderer boatPath;
    private List<Vector3> points = new List<Vector3>();
    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { };
    private Vector3 mOffset;
    private float mZCoord;
    private bool running = false;
    private int runPosition = 0;
    private Rigidbody boatRigid;
    [SerializeField] private float speed = 1.0f;
    void Start()
    {
        boatPath = GetComponent<LineRenderer>();
        boatRigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (running)
        {
            if (runPosition + 1 <= points.Count) { 
                Vector3 target = points[runPosition];
                Debug.Log(target);
                Vector3 moveDirection = (target - transform.position).normalized;
                //transform.position = points[runPosition];
                if (Vector3.Distance(target, transform.position) <= 1)
                {
                    runPosition++;
                }
                else
                {
                    transform.position = transform.position + moveDirection * speed * Time.deltaTime;
                }
            }
            else
            {
                running = false;
                runPosition = 0;
            }
        }
    }

    void OnMouseDown()
    {
        points.Clear();
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    void OnMouseDrag()
    {
        //Debug.Log(GetMouseAsWorldPoint() + mOffset);
        Vector3 mousePoint = GetMouseAsWorldPoint() + mOffset;
        if(DistanceToLastPoint(mousePoint) > .25f)
        {
            points.Add(mousePoint);
            boatPath.positionCount = points.Count;
            boatPath.SetPositions(points.ToArray());
            //transform.position = mousePoint;
        }
    }

    void OnMouseUp()
    {
        running = true;
        runPosition = 0;
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
