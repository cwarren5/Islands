using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuItem : MonoBehaviour
{
    [SerializeField] private int correspondingScene = 2;
    [SerializeField] private float hoverPushDistance = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        transform.localPosition = transform.localPosition + Vector3.down*hoverPushDistance;
    }

    private void OnMouseExit()
    {
        transform.localPosition = transform.localPosition + Vector3.up*hoverPushDistance;
    }

    private void OnMouseDown()
    {
        SceneManager.LoadScene(correspondingScene);
    }
}
