using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinInfo : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI winnerName, totalScore;

    


    public void ReturnToMenu()
    {
      SceneChanger.Instance.LoadScene("MenuScene");
    }

    public void SetWinnerNameAndScore(string name, int score)
    {
        winnerName.text = name;
        totalScore.text = score.ToString();
    }
}
