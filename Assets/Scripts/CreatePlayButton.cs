using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayButton : MonoBehaviour
{
    [SerializeField] private GameObject PlayMode;
    [SerializeField] private GameObject CreateMode;
    private MeshRenderer thisRenderer;
    // Start is called before the first frame update
    void Start()
    {
        thisRenderer = GetComponent<MeshRenderer>();
        thisRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseUp()
    {
        PlayMode.SetActive(true);
        CreateMode.SetActive(false);
    }

    private void OnMouseEnter()
    {
        thisRenderer.enabled = true;
    }

    private void OnMouseExit()
    {
        thisRenderer.enabled = false;
    }
}
