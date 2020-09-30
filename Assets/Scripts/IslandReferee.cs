using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IslandReferee : MonoBehaviour
{
    //public static int yellowBoatCount = 3;
    //public static int redBoatCount = 3;
    public int[] boatCountTotal = new int[4];
    [SerializeField] private TextMesh statusText = default;
    [SerializeField] private TextMesh statusShadow = default;
    public enum BoatTeams {Red, Yellow, White, Orange};
    public Color[] boatShades = new Color[4];
    public BoatTeams currentTurn = BoatTeams.Red;
    public int totalTeams = 0;
    //private Color redOfBoat = new Color(255.0f/255, 92.0f/255, 51.0f/255, 1.0f);
    //private Color yellowOfBoat = new Color(255.0f/255, 188.0f/255, 47.0f/255, 1.0f);
    // Start is called before the first frame update
    void Start()
    {
        Invoke("BoatCountPrintDebug", 0.1f);
        AnnounceTurn();
    }

    // Update is called once per frame
    void Update()
    {
        /*
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
        */
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void CheckForWinner()
    {
        int teamsStillInGame = 0;
        BoatTeams potentialWinner = BoatTeams.Red;
        for(int i = 0; i < totalTeams; i++)
        {
            if (boatCountTotal[i] > 0)
            {
                teamsStillInGame++;
                potentialWinner = (BoatTeams)i;
            }
        }
        Debug.Log("there are " + teamsStillInGame + " teams still in the game");
        if (teamsStillInGame < 2)
        {
            AnnounceWinner(potentialWinner);
        }
    }
    public void AnnounceWinner (BoatTeams winningTeam)
    {
        Debug.Log("The winning team is " + winningTeam);
        statusText.text = winningTeam + " Wins!";
        statusText.color = boatShades[(int)winningTeam];
        statusShadow.text = statusText.text;
        Invoke("GoToMainMenu", 5.0f);
    }

    public void GoToNextTurn()
    {
        int nextTurn = (int)currentTurn;
        nextTurn += 1;
        if (nextTurn == totalTeams)
        {
            nextTurn = 0;
        }
        currentTurn = (BoatTeams)nextTurn;
        AnnounceTurn();
    }

    void AnnounceTurn()
    {
        Debug.Log("It's currently " + currentTurn + "'s Turn");
        statusText.text = currentTurn + "'s Turn";
        statusText.color = boatShades[(int)currentTurn];
        statusShadow.text = statusText.text;
    }

    void BoatCountPrintDebug()
    {
        int loopCounter = 0;
        foreach (int count in boatCountTotal)
        {
            BoatTeams printshade = (BoatTeams)loopCounter;
            Debug.Log(printshade + " has " + boatCountTotal[loopCounter] + " boats on the team");
            if(boatCountTotal[loopCounter] > 0)
            {
                totalTeams++;
            }
            loopCounter++;
        }
        Debug.Log("The total number of teams is " + totalTeams);
    }
}
