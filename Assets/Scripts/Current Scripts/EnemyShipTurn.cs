using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipTurn : MonoBehaviour
{
    [SerializeField] private GameObject destination = default;
    [SerializeField] private GameObject activeGuns = default;
    private Vector3 currentLocation = default;
    private bool movingOut = true;
    private float speed = 15.0f;
    private bool itsMoving = false;

    IslandReferee localReferee;
    [SerializeField] private IslandReferee.BoatTeams boatColor = default;
    // Start is called before the first frame update
    void Start()
    {
        currentLocation = transform.position;
        localReferee = FindObjectOfType<IslandReferee>();
        localReferee.boatCountTotal[(int)boatColor] += 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (localReferee.currentTurn == boatColor)
        {
            itsMoving = true;
            activeGuns.SetActive(true);
        }
        else
        {
            itsMoving = false;
            activeGuns.SetActive(false);
        }

        if (itsMoving)
        {
            if (movingOut)
            {
                Vector3 target = destination.transform.position;
                Vector3 moveDirection = (target - transform.position).normalized;
                float singleStep = speed * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, moveDirection, singleStep, 0.0f);

                if (Vector3.Distance(target, transform.position) <= 1)
                {
                    movingOut = !movingOut;
                    localReferee.GoToNextTurn();
                }
                else
                {
                    transform.position = transform.position + moveDirection * speed * Time.deltaTime;
                    transform.rotation = Quaternion.LookRotation(newDirection);
                }
            }
            if (!movingOut)
            {
                Vector3 target = currentLocation;
                Vector3 moveDirection = (target - transform.position).normalized;
                float singleStep = speed * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, moveDirection, singleStep, 0.0f);

                if (Vector3.Distance(target, transform.position) <= 1)
                {
                    movingOut = !movingOut;
                    localReferee.GoToNextTurn();
                }
                else
                {
                    transform.position = transform.position + moveDirection * speed * Time.deltaTime;
                    transform.rotation = Quaternion.LookRotation(newDirection);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemybomb")
        {
            InitiateSelfDestruct();
        }
    }

    private void InitiateSelfDestruct()
    {
        //var tempExplodingTarget = Instantiate(explodingParticlesPrefab, transform.position, Quaternion.identity);
        //var tempParticleRenderer = tempExplodingTarget.GetComponent<Renderer>();
        //tempParticleRenderer.material.SetColor("_Color", localReferee.boatShades[(int)boatColor]);
        localReferee.boatCountTotal[(int)boatColor] -= 1;
        Destroy(gameObject);
    }
}
