using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBehavior : MonoBehaviour
{
    IslandReferee localReferee;

    [SerializeField] private GameObject explodingParticlesPrefab = default;
    [SerializeField] private GameObject bombBlast = default;
    [SerializeField] private GameObject boatPatherPrefab = default;
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
    }

    void Update()
    {
        if (pathScript.running)
        {
            PlayBoatAlongPath();
            LayBomb();
        }
        if(localReferee.currentTurn != boatColor){myBoatPath.SetActive(false);}
        else{ myBoatPath.SetActive(true);}
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
            if (!dead)
            {
                //InitiateNextPlayerTurn();
            }
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
            var tempExplodingTarget = Instantiate(explodingParticlesPrefab, transform.position, Quaternion.identity);
            var tempParticleRenderer = tempExplodingTarget.GetComponent<Renderer>();
            tempParticleRenderer.material.SetColor("_Color", localReferee.boatShades[(int)boatColor]);
            InitiateNextPlayerTurn();
            //dead = true;
            DestroyBoatAssembly();
            //Invoke("DestroyBoat", 1.0f);
            
        }

        /*if (other.tag == "enemybomb" && !running)
        {
            explodingParticles.SetActive(true);
            boatRender.SetActive(false);
            dead = true;
            Invoke("DestroyBoat", 1.0f);
        }*/
    }

    private void DestroyBoatAssembly()
    {
        localReferee.boatCountTotal[(int)boatColor] -= 1;
        localReferee.CheckForWinner();
        Destroy(myBoatPath);
        Destroy(gameObject);
    }
    private void InitiateNextPlayerTurn()
    {
        localReferee.GoToNextTurn();
    }
    private void LayBomb()
    {
        /*if (bombPosition == runPosition && !hasBomb && !dead)
        {
            bombBlast.SetActive(true);
            bombBlast.transform.position = points[bombPosition];
        }*/
    }
}
