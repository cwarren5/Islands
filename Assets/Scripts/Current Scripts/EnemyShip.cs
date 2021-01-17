using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    IslandReferee localReferee;
    EnemyAIController localGeneral;
    [SerializeField] private GameObject[] waypoints = default;
    [SerializeField] private int startingWaypoint = 0;
    [SerializeField] private GameObject bombVolume = default;
    [SerializeField] private GameObject explodingParticlesPrefab = default;
    private int nextWaypointNumber = 0;
    private Vector3 currentLocation = default;
    private Vector3 nextLocation = default;
    private bool movingOut = true;
    private float speed = 22.0f;
    // Start is called before the first frame update
    void Start()
    {

        localReferee = FindObjectOfType<IslandReferee>();
        localGeneral = FindObjectOfType<EnemyAIController>();
        bombVolume.SetActive(false);
        localGeneral.totalShips++;
        transform.position = waypoints[startingWaypoint].transform.position;
        nextWaypointNumber = startingWaypoint + 1;
        if (nextWaypointNumber == waypoints.Length)
        {
            nextWaypointNumber = 0;
        }
        nextLocation = waypoints[nextWaypointNumber].transform.position;     
    }

    // Update is called once per frame
    void Update()
    {
        bombVolume.SetActive(false);
        if (localReferee.currentTurn == (IslandReferee.BoatTeams)0)
        {
            Vector3 target = nextLocation;
            Vector3 moveDirection = (target - transform.position).normalized;
            float singleStep = speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, moveDirection, singleStep, 0.0f);

            if (Vector3.Distance(target, transform.position) <= 1)
            {
                nextWaypointNumber++;
                if (nextWaypointNumber == waypoints.Length)
                {
                    nextWaypointNumber = 0;
                }
                nextLocation = waypoints[nextWaypointNumber].transform.position;
                localGeneral.allShipsMoved++;
                bombVolume.SetActive(false);
            }
            else
            {
                transform.position = transform.position + moveDirection * speed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(newDirection);
                bombVolume.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemybomb")
        {
            var tempExplodingTarget = Instantiate(explodingParticlesPrefab, transform.position, Quaternion.identity);
            var tempParticleRenderer = tempExplodingTarget.GetComponent<Renderer>();
            tempParticleRenderer.material.SetColor("_Color", localReferee.boatShades[0]);
            localGeneral.totalShips--;
            Destroy(gameObject);
        }
    }
}
