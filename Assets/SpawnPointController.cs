using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    IslandReferee localReferee;

    private Transform[] spawnPoints;
    private Vector3[] spawnPositions;
    [SerializeField] private GameObject pickUp = default;
    [SerializeField] private int turnsBetweenPickups = 5;
    [SerializeField] private int pickupDuration = 4;
    private GameObject placedPickup = default;
    private int currentTurn = 0;
    private int nextPickupTurn = 0;
    private int pickupExpiration = 0;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        spawnPositions = new Vector3[spawnPoints.Length - 1];
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPositions[i] = spawnPoints[i + 1].position;
            Debug.Log("All the spawn positions are: " + spawnPositions[i]);
        }
        localReferee = FindObjectOfType<IslandReferee>();

    }

    // Update is called once per frame
    void Update()
    {
        if(currentTurn != localReferee.totalTurns)
        {
            CheckToPlacePickup();
            currentTurn = localReferee.totalTurns;
        }
    }

    private void CheckToPlacePickup()
    {
        if(currentTurn == nextPickupTurn)
        {
            int randomSpot = Random.Range(0, spawnPositions.Length);
            placedPickup = Instantiate(pickUp, spawnPositions[randomSpot], Quaternion.identity);
            placedPickup.transform.SetParent(gameObject.transform.parent);
            nextPickupTurn += turnsBetweenPickups;
            pickupExpiration = currentTurn + pickupDuration;
        }
        if(currentTurn == pickupExpiration)
        {
            Destroy(placedPickup);
        }
    }
}
