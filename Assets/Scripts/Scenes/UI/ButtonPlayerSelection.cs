using UnityEngine;

public class ButtonPlayerSelection : MonoBehaviour
{
    [SerializeField] int playerAmount;
    private PlayerAmountSelection playerAmountSelection;

    private void Awake()
    {
        playerAmountSelection = transform.parent.GetComponent<PlayerAmountSelection>();
    }

    public void SelectPlayer()
    {
        GameManager.Instance.SetPlayerAmount(playerAmount);
        SceneChanger.Instance.LoadScene("SampleScene");
        playerAmountSelection.Unsubscribe();                //Disables the Update method when selecting by clicking
    }
}
