using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDisplay : MonoBehaviour
{
    IslandReferee localReferee;
    [SerializeField] private GameObject missionaryWinS = default;
    [SerializeField] private GameObject vigilanteWinS = default;
    // Start is called before the first frame update
    void Start()
    {
        localReferee = FindObjectOfType<IslandReferee>();
    }

    // Update is called once per frame
    void Update()
    {
        if (localReferee.winnerDeclaired)
        {
            if((int)localReferee.winnerDeclaration == 1)
            {
                missionaryWinS.SetActive(true);
            }
            if ((int)localReferee.winnerDeclaration == 0)
            {
                vigilanteWinS.SetActive(true);
            }
        }
    }
}
