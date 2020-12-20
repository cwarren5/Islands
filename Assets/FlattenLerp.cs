using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlattenLerp : MonoBehaviour
{
    MapTransition mapTransitioner;
    [SerializeField] private float targetFlattenRate = 3;
    // Start is called before the first frame update
    void Start()
    {
        mapTransitioner = FindObjectOfType<MapTransition>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log(0.9-(Input.mousePosition.y / Screen.height));
            mapTransitioner.transitionSpeed = Mathf.Lerp(0, targetFlattenRate, (0.6f - (Input.mousePosition.y / Screen.height))*1.3f);
        }
    }
}
