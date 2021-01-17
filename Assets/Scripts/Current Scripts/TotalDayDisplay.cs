using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TotalDayDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DelayDisplay", 5.0f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DelayDisplay()
    {
        SceneManager.LoadScene(0);
    }
}
