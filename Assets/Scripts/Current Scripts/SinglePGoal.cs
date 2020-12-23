using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePGoal : MonoBehaviour
{
    IslandReferee localReferee;
    [SerializeField] private IslandReferee.BoatTeams boatColor = default;
    [SerializeField] private bool sendToNextGoal = false;
    [SerializeField] private GameObject nextGoal = default;
    private bool shipLanded = false;
    // Start is called before the first frame update
    void Start()
    {
        localReferee = FindObjectOfType<IslandReferee>();
        localReferee.boatCountTotal[0] += 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(localReferee.currentTurn == boatColor)
        {
            //localReferee.GoToNextTurn();
            if (shipLanded)
            {
                localReferee.boatCountTotal[0] -= 1;
                shipLanded = false;
                if (sendToNextGoal)
                {
                    nextGoal.SetActive(true);
                    Destroy(gameObject);
                }
                else
                {
                    localReferee.nextScene++;
                    localReferee.CheckForWinner();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "enemyship" && other.tag != "island")
        {
            shipLanded = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        shipLanded = false;
    }
}
