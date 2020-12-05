using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnDisplay : MonoBehaviour
{
    IslandReferee localReferee;
    [SerializeField] private GameObject missionariesTurn = default;
    [SerializeField] private GameObject vigilantiesTurn = default;
    // Start is called before the first frame update
    void Start()
    {
        localReferee = FindObjectOfType<IslandReferee>();
        missionariesTurn.SetActive(false);
        vigilantiesTurn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(localReferee.currentTurn == (IslandReferee.BoatTeams)1)
        {
            missionariesTurn.SetActive(true);
            vigilantiesTurn.SetActive(false);
        }
        if(localReferee.currentTurn == (IslandReferee.BoatTeams)0)
        {
            vigilantiesTurn.SetActive(true);
            missionariesTurn.SetActive(false);
        }
    }
}
