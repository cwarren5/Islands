using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IslandReferee : MonoBehaviour
{
    [SerializeField] private TextMesh statusText = default;
    [SerializeField] private TextMesh statusShadow = default;
    [SerializeField] public GameObject nightEnv = default;
    [SerializeField] public GameObject dayEnv = default;

    public enum BoatTeams {Red, Yellow, White, Orange};
    public BoatTeams currentTurn = BoatTeams.Red;
    public Color[] boatShades = new Color[4];
    public int[] boatCountTotal = new int[4];
    public int totalTeams = 0;
    private bool winnerDeclaired = false;
 
    //Sets up game after all boats have time to report back
    void Start()
    {
        Invoke("GameSetup", 0.1f);
        winnerDeclaired = false;
    }

    void Update()
    {

    }

    // for use by other scripts
    private void GoToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    // checks to see if someone won the game every time a ship is destroyed
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

    //
    public void AnnounceWinner(BoatTeams winningTeam)
    {
        if (!winnerDeclaired) { 
            winnerDeclaired = true;
            Debug.Log("The winning team is " + winningTeam);
            statusText.text = winningTeam + " Wins!";
            statusText.color = boatShades[(int)winningTeam];
            statusShadow.text = statusText.text;
            Invoke("GoToMainMenu", 5.0f);
        }
    }

    public void GoToNextTurn()
    {
        int nextTurn = (int)currentTurn;
        bool foundNextTurn = false;
        while (!foundNextTurn)
        {
            nextTurn += 1;
            if (nextTurn == totalTeams)
            {
                nextTurn = 0;
            }
            if (boatCountTotal[nextTurn] != 0)
            {
                foundNextTurn = true;
            }
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

    void GameSetup()
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
        int randomTurn = Random.Range(0, totalTeams);
        currentTurn = (BoatTeams)randomTurn;
        AnnounceTurn();
    }
}
