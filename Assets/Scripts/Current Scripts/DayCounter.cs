using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCounter : MonoBehaviour
{
    IslandReferee localReferee;
    static int daysAtSea = 0;
    private IslandReferee.BoatTeams boatMonitor = IslandReferee.BoatTeams.Red;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMesh>().text = "" + daysAtSea;
        localReferee = FindObjectOfType<IslandReferee>();
    }

    // Update is called once per frame
    void Update()
    {
        if (localReferee != null)
        {
            if (localReferee.currentTurn != boatMonitor)
            {
                if (localReferee.currentTurn == IslandReferee.BoatTeams.Red)
                {
                    daysAtSea++;
                    Debug.Log(daysAtSea);
                    GetComponent<TextMesh>().text = "" + daysAtSea;
                }
                boatMonitor = localReferee.currentTurn;
            }
        }
    }
}
