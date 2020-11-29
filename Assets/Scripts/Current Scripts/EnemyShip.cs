using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    [SerializeField] private GameObject destination = default;
    private Vector3 currentLocation = default;
    private bool movingOut = true;
    private float speed = 15.0f;
    // Start is called before the first frame update
    void Start()
    {
        currentLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingOut)
        {
            Vector3 target = destination.transform.position;
            Vector3 moveDirection = (target - transform.position).normalized;
            float singleStep = speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, moveDirection, singleStep, 0.0f);

            if (Vector3.Distance(target, transform.position) <= 1)
            {
                movingOut = !movingOut;
            }
            else
            { 
                transform.position = transform.position + moveDirection * speed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
        if (!movingOut)
        {
            Vector3 target = currentLocation;
            Vector3 moveDirection = (target - transform.position).normalized;
            float singleStep = speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, moveDirection, singleStep, 0.0f);

            if (Vector3.Distance(target, transform.position) <= 1)
            {
                movingOut = !movingOut;
            }
            else
            {
                transform.position = transform.position + moveDirection * speed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
    }
}
