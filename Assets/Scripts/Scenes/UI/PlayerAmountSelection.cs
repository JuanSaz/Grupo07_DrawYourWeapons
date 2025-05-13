using UnityEngine;

public class PlayerAmountSelection : MonoBehaviour, IUpdatable
{
    private void Awake()
    {
        //UpdateManager.Instance.Subscribe(this);
    }

    public void UpdateMe(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))  //Si presion� el 2 en numpad o normal
        {
            GameManager.Instance.SetPlayerAmount(2);

            UpdateManager.Instance.Unsubscribe(this);
            SceneChanger.Instance.LoadScene("SampleScene");
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))  //Si presion� el 3 en numpad o normal
        {
            GameManager.Instance.SetPlayerAmount(3);

            UpdateManager.Instance.Unsubscribe(this);
            SceneChanger.Instance.LoadScene("SampleScene");
           
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))   //Si presion� el 4 en numpad o normal
        {
            GameManager.Instance.SetPlayerAmount(4);

            UpdateManager.Instance.Unsubscribe(this);
            SceneChanger.Instance.LoadScene("SampleScene");
           
        }

    }

    public void Subscribe()
    {
        //UpdateManager.Instance.Unsubscribe(this);
        UpdateManager.Instance.Subscribe(this);
    }
}
