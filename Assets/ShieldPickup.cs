using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : MonoBehaviour
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

            GameObject collidedBoat = other.gameObject;
            BoatBehavior collidedBoatBehavior = collidedBoat.GetComponent<BoatBehavior>();
            collidedBoatBehavior.forceField.SetActive(true);
        Destroy(gameObject);
    }
}
