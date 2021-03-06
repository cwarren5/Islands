﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    IslandReferee localReferee;

    [SerializeField] private float lineFidelity = .25f;
    [SerializeField] private GameObject bombX = default;
    [SerializeField] private GameObject mineM = default;
    [SerializeField] private GameObject bombIcon = default;
    private GameObject activeBombX = default;
    private GameObject activeMineM = default;

    private LineRenderer boatPath;
    private int localVisibleLength;
    private bool pressed = false;
    public bool hasBomb = false;
    public bool droppedMine = false;
    public List<Vector3> points = new List<Vector3>();
    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { };
    private Vector3 mOffset;
    private float mZCoord;
    public bool running = false;
    public int bombPosition = default;
    public int minePosition = default;

    // Start is called before the first frame update
    void Start()
    {
        boatPath = GetComponent<LineRenderer>();
        boatPath.enabled = false;
        localReferee = FindObjectOfType<IslandReferee>();
        bombIcon = GameObject.FindGameObjectWithTag("bombIcon");
        localVisibleLength = localReferee.visibleLineLength;
    }

    // Update is called once per frame
    void Update()
    {
        WeaponsCheck();
    }

    //Have boat turn collider off when not its turn
    void OnMouseDown()
    {
        CreateNewBoatPath();
        //localReferee.nightEnv.SetActive(true);
        localReferee.dayEnv.SetActive(false);
        pressed = true;
        hasBomb = true;
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
        //localReferee.nightEnv.SetActive(false);
        localReferee.dayEnv.SetActive(true);
        boatPath.enabled = false;
        Destroy(activeBombX);
        Destroy(activeMineM);
    }



    private void DrawBoatPath()
    {
        Vector3 mousePoint = GetMouseAsWorldPoint() + mOffset;
        if (DistanceToLastPoint(mousePoint) > lineFidelity)
        {
            points.Add(mousePoint);
            if(points.Count <= localVisibleLength)
            {
                boatPath.positionCount = points.Count;
                boatPath.SetPositions(points.ToArray());
            }
            else
            {
                Vector3[] newPositions = new Vector3[localVisibleLength];
                for (int i = 0; i < localVisibleLength; i++)
                {
                    newPositions[i] = points[points.Count - localVisibleLength + i];
                }
                boatPath.SetPositions(newPositions);
            }
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
        if (!points.Any()) {return Mathf.Infinity;}
        else{return Vector3.Distance(points.Last(), point);}
    }

    private void WeaponsCheck()
    {
        if (Input.GetKeyDown("b") && pressed && hasBomb)
        {
            activeBombX = Instantiate(bombX, points[points.Count - 1], Quaternion.identity);
            bombPosition = points.Count - 1;
            hasBomb = false;
            bombIcon.SetActive(false);
        }
        if (Input.GetKeyDown("m") && pressed && !localReferee.usedMine[(int)localReferee.currentTurn])
        {
            activeMineM = Instantiate(mineM, points[points.Count - 1], Quaternion.identity);
            minePosition = points.Count - 1;
            droppedMine = true;
            //localReferee.usedMine[0] = true;
            //bombIcon.SetActive(false);
        }
    }
}
