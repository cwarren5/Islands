using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoatPather : MonoBehaviour
{
    IslandReferee localRef;
    
    //Effects and Objects
    [SerializeField] private GameObject boatRender = default;
    [SerializeField] private GameObject nightShade = default;
    [SerializeField] private GameObject explodingParticles = default;
    [SerializeField] private GameObject bombX = default;
    [SerializeField] private GameObject bombBlast = default;
    [SerializeField] private GameObject bombInventory = default;
    [SerializeField] private GameObject dayTimeGeo = default;

    //Bomb Variables
    private bool pressed = false;
    private bool hasBomb = false;
    private int bombPosition = default;
    private bool dead = false;

    //Path Settings
    [SerializeField] private float speed = 15.0f;
    [SerializeField] private float lineFidelity = .25f;
    [SerializeField] private IslandReferee.BoatTeams boatColor = default;

    //Path Variables
    private LineRenderer boatPath;
    private List<Vector3> points = new List<Vector3>();
    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { };
    private Vector3 mOffset;
    private float mZCoord;
    private bool running = false;
    private int runPosition = 0;
    private Rigidbody boatRigid;


    
    void Start()
    {
        boatPath = GetComponent<LineRenderer>();
        boatRigid = GetComponent<Rigidbody>();
        nightShade.SetActive(false);
        bombX.SetActive(false);
        boatPath.enabled = false;
        localRef = FindObjectOfType<IslandReferee>();
        localRef.boatCountTotal[(int)boatColor] += 1;
    }

    void Update()
    {
        if (running)
        {
            PlayBoatAlongPath();
            LayBomb();
        }
        WeaponsCheck();
    }

    //Mouse Actions when player clicks a boat
    void OnMouseDown()
    {
        if (boatColor == IslandReferee.currentTurn)
        {      
            CreateNewBoatPath();
            nightShade.SetActive(true);
            dayTimeGeo.SetActive(false);
            pressed = true;
            hasBomb = true;
            bombInventory.SetActive(true);
            boatPath.enabled = true;
            boatRender.SetActive(false);
        }
    }

    void OnMouseDrag()
    {
        if (boatColor == IslandReferee.currentTurn)
        {
            DrawBoatPath();
        }
    }

    void OnMouseUp()
    {
        if (boatColor == IslandReferee.currentTurn)
        {
            running = true;
            runPosition = 0;
            pressed = false;
            nightShade.SetActive(false);
            dayTimeGeo.SetActive(true);
            boatPath.enabled = false;
            boatRender.SetActive(true);
            bombX.SetActive(false);
        }
    }

    //Collisions

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "island")
        {
            explodingParticles.SetActive(true);
            boatRender.SetActive(false);
            dead = true;
            InitiateNextPlayerTurn();
            Invoke("DestroyBoat", 1.0f);
        }

        if (other.tag == "enemybomb" && !running)
        {
            explodingParticles.SetActive(true);
            boatRender.SetActive(false);
            dead = true;
            Invoke("DestroyBoat" , 1.0f);
        }
    }

    private void DestroyBoat()
    {
        if (boatColor == IslandReferee.BoatTeams.Red)
        {
            IslandReferee.redBoatCount -= 1;
        }
        if (boatColor == IslandReferee.BoatTeams.Yellow)
        {
            IslandReferee.yellowBoatCount -= 1;
        }
        Destroy(gameObject);
    }

    //Methods

    private void InitiateNextPlayerTurn()
    {
        int nextTurn = (int)IslandReferee.currentTurn;
        nextTurn += 1;
        int totalEnumItems = IslandReferee.BoatTeams.GetNames(typeof(IslandReferee.BoatTeams)).Length;
        if (nextTurn == totalEnumItems)
        {
            nextTurn = 0;
        }
        IslandReferee.currentTurn = (IslandReferee.BoatTeams)nextTurn;
    }

    private void WeaponsCheck()
    {
        if (Input.GetKeyDown("b") && pressed && hasBomb)
        {
            bombX.SetActive(true);
            bombX.transform.position = points[points.Count-1];
            bombPosition = points.Count - 1;
        }
        if (Input.GetKeyUp("b"))
        {
            //bomb.SetActive(false);
            hasBomb = false;
            bombInventory.SetActive(false);
        }
    }

    private void LayBomb()
    { 
        if(bombPosition == runPosition && !hasBomb && !dead)
        {
            bombBlast.SetActive(true);
            bombBlast.transform.position = points[bombPosition];
        }
    }

    private void PlayBoatAlongPath()
    {
        if (runPosition + 1 <= points.Count)
        {
            Vector3 target = points[runPosition];
            Vector3 moveDirection = (target - transform.position).normalized;
            float singleStep = speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(boatRender.transform.forward, moveDirection, singleStep, 0.0f);


            if (Vector3.Distance(target, transform.position) <= 1)
            {
                runPosition++;
            }
            else
            {
                transform.position = transform.position + moveDirection * speed * Time.deltaTime;
                boatRender.transform.position = transform.position + moveDirection * speed * Time.deltaTime;
                boatRender.transform.rotation = Quaternion.LookRotation(newDirection);

            }
        }
        else
        {
            if (!dead)
            {
                InitiateNextPlayerTurn();
            }
            running = false;
            runPosition = 0;
        }
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

    //Variable Returns
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
