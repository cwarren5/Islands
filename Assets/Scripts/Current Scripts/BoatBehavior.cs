using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBehavior : MonoBehaviour
{
    IslandReferee localReferee;

    [SerializeField] private GameObject explodingParticlesPrefab = default;
    [SerializeField] private GameObject bombBlast = default;
    [SerializeField] private GameObject boatPatherPrefab = default;
    [SerializeField] private GameObject turnHighlighter = default;
    private GameObject bombIcon = default;
    private GameObject myBoatPath = default;
    [SerializeField] private float speed = 15.0f;
    [SerializeField] private IslandReferee.BoatTeams boatColor = default;

    private bool dead = false;
    private int runPosition = 0;

    PathCreator pathScript;

    void Start()
    {
        localReferee = FindObjectOfType<IslandReferee>();
        localReferee.boatCountTotal[(int)boatColor] += 1;
        myBoatPath = Instantiate(boatPatherPrefab, transform.position, Quaternion.identity);
        pathScript = myBoatPath.GetComponent<PathCreator>();
        turnHighlighter.SetActive(false);
        bombIcon = GameObject.FindGameObjectWithTag("bombIcon");
    }

    void Update()
    {
        if (pathScript.running)
        {
            PlayBoatAlongPath();
            LayBomb();
        }
        if(localReferee.currentTurn != boatColor)
        {
            myBoatPath.SetActive(false);
            turnHighlighter.SetActive(false);
        }
        else
        {
            turnHighlighter.SetActive(true);
            myBoatPath.SetActive(true);
        }
    }

    private void PlayBoatAlongPath()
    {
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
        if (other.tag == "island")
        {
            InitiateNextPlayerTurn();
            InitiateSelfDestruct();            
        }

        if (other.tag == "enemybomb" && !pathScript.running)
        {
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
}
