using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinePickup : MonoBehaviour
{
    // Start is called before the first frame update
    IslandReferee localReferee;
    void Start()
    {
        localReferee = FindObjectOfType<IslandReferee>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (localReferee.teamMines[(int)localReferee.currentTurn] < localReferee.minerIcons.Length)
        {
            localReferee.teamMines[(int)localReferee.currentTurn]++;
            localReferee.UpdateMineDisplay();
        }
        Destroy(gameObject);
    }
}
