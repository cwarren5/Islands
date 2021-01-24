﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBehavior : MonoBehaviour
{
    IslandReferee localReferee;

    [SerializeField] private GameObject explodingParticlesPrefab = default;
    [SerializeField] private GameObject bombBlast = default;
    [SerializeField] private GameObject mine = default;
    [SerializeField] private GameObject boatPatherPrefab = default;
    [SerializeField] private GameObject turnHighlighter = default;
    [SerializeField] private GameObject anchorIcon = default;
    [SerializeField] public GameObject forceField = default;
    private GameObject bombIcon = default;
    //private GameObject mineIcon = default;
    //private GameObject[] minerIcons = default;
    private GameObject myBoatPath = default;
    [SerializeField] private float speed = 15.0f;
    [SerializeField] private IslandReferee.BoatTeams boatColor = default;

    private bool dead = false;
    private int runPosition = 0;
    private bool beached = false;
    private int timeOut = 0;
    private bool turnMonitor = true;

    PathCreator pathScript;

    void Start()
    {
        localReferee = FindObjectOfType<IslandReferee>();
        localReferee.boatCountTotal[(int)boatColor] += 1;
        myBoatPath = Instantiate(boatPatherPrefab, transform.position, Quaternion.identity);
        pathScript = myBoatPath.GetComponent<PathCreator>();
        turnHighlighter.SetActive(false);
        bombIcon = GameObject.FindGameObjectWithTag("bombIcon");
        //mineIcon = GameObject.FindGameObjectWithTag("mineIcon");
        anchorIcon.SetActive(false);
        if (localReferee.startWithForcefields)
        {
            forceField.SetActive(true);
        }
        
    }

    void Update()
    {
        if (pathScript.running)
        {
            PlayBoatAlongPath();
            LayBomb();
            LayMine();
        }
        if (localReferee.currentTurn == boatColor)
        {
            if (!beached || timeOut > localReferee.beachedTurnsNumber * 2 - 1)
            {
                turnHighlighter.SetActive(true);
                myBoatPath.SetActive(true);
                anchorIcon.SetActive(false);
            }
            if (!turnMonitor && beached)
            {
                timeOut++;
                turnMonitor = true;
            }
        }
        else
        {           
            myBoatPath.SetActive(false);
            turnHighlighter.SetActive(false);

            if (turnMonitor && beached)
            {
                timeOut++;
                turnMonitor = false;
                anchorIcon.SetActive(true);

            }
        }


    }

    private void PlayBoatAlongPath()
    {
        beached = false;
        if (runPosition + 1 <= pathScript.points.Count)
        {
            Vector3 target = pathScript.points[runPosition];
            Vector3 moveDirection = (target - transform.position).normalized;
            float singleStep = speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, moveDirection, singleStep, 0.0f);


            if (Vector3.Distance(target, transform.position) <= 1)
            {
                runPosition++;
            }
            else
            {
                transform.position = transform.position + moveDirection * speed * Time.deltaTime;
                transform.position = transform.position + moveDirection * speed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(newDirection);

            }
        }
        else
        {
            pathScript.running = false;
            InitiateNextPlayerTurn();
            runPosition = 0;
            myBoatPath.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I GOT HIT");
        if(other.tag == "sandIsland")
        {
            if (!beached)
            {
                anchorIcon.SetActive(true);
                beached = true;
                timeOut = 0;
                pathScript.running = false;
                InitiateNextPlayerTurn();
                runPosition = 0;
                myBoatPath.transform.position = transform.position;
            }
        }
        if (other.tag == "island")
        {       
                InitiateNextPlayerTurn();
                InitiateSelfDestruct();        
        }

        if (other.tag == "enemybomb" && !pathScript.running)
        {
            Debug.Log("I Got Hit");
            if (forceField.activeSelf)
            {
                forceField.SetActive(false);
            }
            else
            {
                InitiateSelfDestruct();
            }
        }

        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "sharkWaters" && !pathScript.running)
        {
            Debug.Log("In Shark Waters");
            InitiateSelfDestruct();
        }
    }

    private void InitiateSelfDestruct()
    {
        var tempExplodingTarget = Instantiate(explodingParticlesPrefab, transform.position, Quaternion.identity);
        var tempParticleRenderer = tempExplodingTarget.GetComponent<Renderer>();
        tempParticleRenderer.material.SetColor("_Color", localReferee.boatShades[(int)boatColor]);
        localReferee.boatCountTotal[(int)boatColor] -= 1;
        localReferee.CheckForWinner();
        Destroy(myBoatPath);
        Destroy(gameObject);
    }
    private void InitiateNextPlayerTurn()
    {
        bombIcon.SetActive(true);
        localReferee.GoToNextTurn();
        
    }
    private void LayBomb()
    {
        if (pathScript.bombPosition == runPosition && !pathScript.hasBomb)
        {
            /*bombBlast.SetActive(true);
            bombBlast.transform.position = points[bombPosition];*/
            Instantiate(bombBlast, transform.position, Quaternion.identity);
            pathScript.hasBomb = true;
        }
    }

    private void LayMine()
    {
        if (pathScript.minePosition == runPosition && !localReferee.usedMine[(int)boatColor] && pathScript.droppedMine)
        {
            GameObject myBomb = Instantiate(mine, transform.position, Quaternion.identity);
            myBomb.transform.SetParent(gameObject.transform.parent);
            localReferee.usedMine[(int)boatColor] = true;
            localReferee.teamMines[(int)boatColor]--;
            localReferee.UpdateMineDisplay();
            pathScript.droppedMine = false;
            //mineIcon.SetActive(false);
        }
    }
}
