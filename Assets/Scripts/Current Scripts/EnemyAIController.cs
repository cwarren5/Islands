using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    IslandReferee localReferee;
    [SerializeField] private IslandReferee.BoatTeams boatColor = IslandReferee.BoatTeams.Red;
    public int totalShips = 0;
    public int allShipsMoved = 0;
    private bool delay = false;
    // Start is called before the first frame update
    void Start()
    {
        localReferee = FindObjectOfType<IslandReferee>();
        //localReferee.boatCountTotal[(int)boatColor] += 1;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("there are this many enemy boats:" + totalShips + " and this many moved:" + allShipsMoved);
        if (localReferee.currentTurn == boatColor && allShipsMoved == totalShips && !delay)
        {
            Invoke("DelayedTurn", .1f);
            allShipsMoved = 0;
            delay = true;
        }
    }

    private void DelayedTurn()
    {
        localReferee.GoToNextTurn();
        delay = false;
    }
}
