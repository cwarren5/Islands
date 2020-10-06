using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    IslandReferee localReferee;

    [SerializeField] private float lineFidelity = .25f;

    private LineRenderer boatPath;
    private bool pressed = false;
    private bool hasBomb = false;
    public List<Vector3> points = new List<Vector3>();
    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { };
    private Vector3 mOffset;
    private float mZCoord;
    public bool running = false;
    // Start is called before the first frame update
    void Start()
    {
        boatPath = GetComponent<LineRenderer>();
        localReferee = FindObjectOfType<IslandReferee>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Have boat turn collider off when not its turn
    void OnMouseDown()
    {
        CreateNewBoatPath();
        localReferee.nightEnv.SetActive(true);
        localReferee.dayEnv.SetActive(false);
        pressed = true;
        hasBomb = true;
        //bombInventory.SetActive(true);
        boatPath.enabled = true;    
    }

    void OnMouseDrag()
    {
        DrawBoatPath();
    }

    void OnMouseUp()
    {
        running = true;
        //runPosition = 0;
        pressed = false;
        localReferee.nightEnv.SetActive(false);
        localReferee.dayEnv.SetActive(true);
        boatPath.enabled = false;
        //bombX.SetActive(false);
    }



    private void DrawBoatPath()
    {
        Vector3 mousePoint = GetMouseAsWorldPoint() + mOffset;
        if (DistanceToLastPoint(mousePoint) > lineFidelity)
        {
            points.Add(mousePoint);
            boatPath.positionCount = points.Count;
            boatPath.SetPositions(points.ToArray());
        }
    }

    private void CreateNewBoatPath()
    {
        points.Clear();
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
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
