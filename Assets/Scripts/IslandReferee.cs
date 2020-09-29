using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IslandReferee : MonoBehaviour
{
    public static int yellowBoatCount = 3;
    public static int redBoatCount = 3;
    public int[] boatCountTotal = new int[4];
    [SerializeField] private TextMesh statusText = default;
    [SerializeField] private TextMesh statusShadow = default;
    public enum BoatTeams {Red, Yellow, White, Orange};
    public Color[] boatShades = new Color[4];
    public static BoatTeams currentTurn = BoatTeams.Red;
    private Color redOfBoat = new Color(255.0f/255, 92.0f/255, 51.0f/255, 1.0f);
    private Color yellowOfBoat = new Color(255.0f/255, 188.0f/255, 47.0f/255, 1.0f);
    // Start is called before the first frame update
    void Start()
    {
        yellowBoatCount = 3;
        redBoatCount = 3;
        Invoke("BoatCountPrint", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(redBoatCount == 0)
        {
            //Debug.Log("Yellow  Wins!");
            statusText.text = "Yellow Wins!";
            statusText.color = yellowOfBoat;
            Invoke("GoToMainMenu", 5.0f);
        }
        else if(yellowBoatCount == 0)
        {
            //Debug.Log("Red Wins!");
            statusText.text = "Red Wins!";
            statusText.color = redOfBoat;
            Invoke("GoToMainMenu", 5.0f);
        }
        else if(currentTurn == BoatTeams.Red)
        {
            statusText.text = "Red's Turn!";
            statusText.color = redOfBoat;
        }
        else if (currentTurn == BoatTeams.Yellow)
        {
            statusText.text = "Yellow's Turn!";
            statusText.color = yellowOfBoat;
        }
        statusShadow.text = statusText.text;
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    void BoatCountPrint()
    {
        int loopCounter = 0;
        foreach (int count in boatCountTotal)
        {
            BoatTeams printshade = (BoatTeams)loopCounter;
            Debug.Log(printshade + " has " + boatCountTotal[loopCounter] + " boats on the team");
            loopCounter++;
        }
    }
}
