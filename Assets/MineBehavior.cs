using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBehavior : MonoBehaviour
{
    Collider collider = default;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        collider.enabled = false;
        Invoke("MineDelay", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void MineDelay()
    {
        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "island")
        {
            Destroy(gameObject);
        }
    }
}
