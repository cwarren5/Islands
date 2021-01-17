using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBehavior : MonoBehaviour
{
    Collider mineCollider = default;
    IslandReferee localReferee;
    private int turnsOnSpawn = 0;

    // Start is called before the first frame update
    void Start()
    {
        localReferee = FindObjectOfType<IslandReferee>();
        turnsOnSpawn = localReferee.totalTurns;
        mineCollider = GetComponent<Collider>();
        mineCollider.enabled = false;
        Invoke("MineDelay", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(localReferee.totalTurns - turnsOnSpawn > localReferee.mineTurnDuration)
        {
            Destroy(gameObject);
        }
    }

    private void MineDelay()
    {
        mineCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "island")
        {
            Destroy(gameObject);
        }
    }
}
