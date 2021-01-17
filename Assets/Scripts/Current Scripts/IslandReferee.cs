using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IslandReferee : MonoBehaviour
{
    //[SerializeField] private TextMesh statusText = default;
    //[SerializeField] private TextMesh statusShadow = default;
    //[SerializeField] public GameObject nightEnv = default;
    [Header("Scene Attributes")]
    [SerializeField] public GameObject dayEnv = default;
    [SerializeField] public int nextScene = 1;

    [Header("Game Rules")]

    [SerializeField] public int beachedTurnsNumber = 1;
    [SerializeField] public int startingMineNumber = 1;
    [SerializeField] public int mineTurnDuration = 3;


    [HideInInspector] public enum BoatTeams {Red, Yellow, White, Black};
    [HideInInspector] public BoatTeams currentTurn = BoatTeams.Red;
    [HideInInspector] public BoatTeams winnerDeclaration = default;
    [HideInInspector] public Color[] boatShades = new Color[4];
    [HideInInspector] public int[] boatCountTotal = new int[4];
    [HideInInspector] public int totalTeams = 4;
    [HideInInspector] public bool winnerDeclaired = false;
    [HideInInspector] public bool[] usedMine = new bool[4];
    public GameObject[] minerIcons;
    [HideInInspector] public int totalTurns = 0;
    public int[] teamMines = new int[4];

    //Sets up game after all boats have time to report back
    void Start()
    {
        Invoke("GameSetup", 0.01f);
        winnerDeclaired = false;
        totalTeams = 4;
        for (int i = 0; i < teamMines.Length; i++)
        {
            teamMines[i] = startingMineNumber;
        }

        
        minerIcons = GameObject.FindGameObjectsWithTag("mineIcon");      
    }

    void Update()
    {

    }

    // for use by other scripts
    private void GoToScene()
    {
        SceneManager.LoadScene(nextScene);
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
        PrintGameStatus();
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
            //statusText.text = winningTeam + " Wins!";
            //statusText.color = boatShades[(int)winningTeam];
            //statusShadow.text = statusText.text;
            winnerDeclaration = winningTeam;
            Invoke("GoToScene", 2.0f);
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
        totalTurns++;
        Debug.Log("Turns This Game : " + totalTurns);
        UpdateMineDisplay();
        ReloadMines();
        //AnnounceTurn();
    }

    /*void AnnounceTurn()
    {
        Debug.Log("It's currently " + currentTurn + "'s Turn");
        statusText.text = currentTurn + "'s Turn";
        statusText.color = boatShades[(int)currentTurn];
        statusShadow.text = statusText.text;
    }*/

    public void UpdateMineDisplay()
    {
        if (minerIcons.Length > 0)
        {
            for (int i = 0; i < minerIcons.Length; i++)
            {
                minerIcons[i].SetActive(false);
            }
            for (int x = 0; x < teamMines[(int)currentTurn]; x++)
            {
                Debug.Log("this is the problem - " + teamMines[(int)currentTurn]);
                minerIcons[x].SetActive(true);
            }
        }
        
    }

    void ReloadMines()
    {
        if(teamMines[(int)currentTurn] > 0)
        {
            usedMine[(int)currentTurn] = false;
        }
    }

    void GameSetup()
    {
        PrintGameStatus();
        Debug.Log("The total number of teams is " + totalTeams);
        bool foundAStartingTurn = false;
        while (!foundAStartingTurn)
        {
            int randomTurn = Random.Range(0, totalTeams);
            currentTurn = (BoatTeams)randomTurn;
            if (boatCountTotal[(int)currentTurn] > 0)
            {
                foundAStartingTurn = true;
            }
        }
        UpdateMineDisplay();
        //AnnounceTurn();
    }

    void PrintGameStatus()
    {
        Debug.Log("The Current Status Of The Game Is - ");
        int loopCounter = 0;
        foreach (int count in boatCountTotal)
        {
            BoatTeams printshade = (BoatTeams)loopCounter;
            Debug.Log(printshade + " has " + boatCountTotal[loopCounter] + " boats on the team");
            /*if (boatCountTotal[loopCounter] > 0)
            {
                totalTeams++;
            }*/
            loopCounter++;
        }
    }
}
