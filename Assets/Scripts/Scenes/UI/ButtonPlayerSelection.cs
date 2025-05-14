using UnityEngine;

public class ButtonPlayerSelection : MonoBehaviour
{
    [SerializeField] int playerAmount;

    public void SelectPlayer()
    {
        GameManager.Instance.SetPlayerAmount(playerAmount);
        SceneChanger.Instance.LoadScene("SampleScene");
    }
}
