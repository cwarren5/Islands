using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelection : MonoBehaviour
{
    [SerializeField] private int correspondingScene = 2;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        rend.enabled = true;
    }

    private void OnMouseExit()
    {
        rend.enabled = false;
    }

    private void OnMouseDown()
    {
        SceneManager.LoadScene(correspondingScene);
    }
}
