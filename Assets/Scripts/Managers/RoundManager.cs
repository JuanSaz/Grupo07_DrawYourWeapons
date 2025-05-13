using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private List<Player> players;
    private bool roundEnded = false;

    private void Start()
    {
        //players = FindObjectsOfType<Player>().ToList(); 
        players = LevelManager.Instance.ActivePlayers;
    }

    private void Update()
    {
        if (roundEnded) return;

        int aliveCount = 0;
        Player lastAlive = null;

        foreach (var player in players)
        {
            if (player.IsAlive())
            {
                aliveCount++;
                lastAlive = player;
            }
        }

        if (aliveCount == 1)
        {
            roundEnded = true;
            lastAlive.AddPoint();
            GetPointsFromSurvivour(lastAlive);

            StartCoroutine(RestartRoundAfterDelay(2f)); 
        }
    }

    private IEnumerator RestartRoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var player in players)
        {
            player.ResetPlayer();
        }

        roundEnded = false;
    }

    public void GetPointsFromSurvivour(Player survivour)
    {
        if(survivour.Score == 3)
        {
           LevelManager.Instance.SetWinner(survivour);
            
        }
    }
    
}
