using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBehavior : MonoBehaviour
{
    IslandReferee localReferee;

    [SerializeField] private GameObject explodingParticles = default;
    [SerializeField] private GameObject bombBlast = default;
    [SerializeField] private float speed = 15.0f;
    [SerializeField] private IslandReferee.BoatTeams boatColor = default;

    private bool running = false;
    private bool dead = false;
    private int runPosition = 0;

    void Start()
    {
        localReferee = FindObjectOfType<IslandReferee>();
        localReferee.boatCountTotal[(int)boatColor] += 1;
    }

    void Update()
    {
        if (running)
        {
            PlayBoatAlongPath();
            LayBomb();
        }
    }

    private void PlayBoatAlongPath()
    {
        if (runPosition + 1 <= 2/*points.Count*/)
        {
            Vector3 target = /*points[runPosition]*/ Vector3.forward;
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
            running = false;
            runPosition = 0;
        }
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
