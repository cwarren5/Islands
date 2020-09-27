using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private SpriteRenderer islandsLogo = default;
    [SerializeField] private float length = 6.0f;
    private float timer = 0.0f;
    private bool logoStart = false;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartLogoIntro", 2.0f);
        Invoke("LoadMainMenu", 10.0f);
        islandsLogo.color = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(logoStart && timer < 1.0f)
        timer += Time.deltaTime / length;
        float inOut = Mathf.Sin(timer * Mathf.PI);
        float clampInOut = Mathf.Clamp(inOut * 2, 0, 1.0f);
        islandsLogo.color = new Color(1f, 1f, 1f, clampInOut);
    }

    private void StartLogoIntro()
    {
        logoStart = true;
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(1);
    }
}
