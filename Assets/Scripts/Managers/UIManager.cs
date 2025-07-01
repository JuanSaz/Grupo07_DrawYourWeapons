using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum E_UIScreens
{
    MainMenuUI,
    SelectionScreenUI,
    HUDUI,
    WinUI,
    LooseUI,
    SpecialRoundUI,
    SettingsUI,
    ConfirmExitUI,
    CreditsUI,
    LoadingUI,
    CountDownUI,
}

public enum E_ButtonActions
{
    Play,
    Settings,
    Exit,
    BackToMenu,
    TwoPlayers,
    ThreePlayers,
    FourPlayers,
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject[] buttons;
    private Button[] buttonsComponent = new Button[4];
    private GameObject[] texts = new GameObject[4];
    private TextMeshProUGUI[] textsComponent = new TextMeshProUGUI[4];

    [SerializeField] private GameObject[] layoutGroup;

    private GameObject background;
    private Image currentBackground;

    [SerializeField] private Sprite[] backgroundImages = new Sprite[3];

    [Header("MainMenuUI")]
    [SerializeField] private SO_Button[] MainMenuUIButtonsSO = new SO_Button[4];

    [Header("SelectionScreenUI")]
    [SerializeField] private SO_Button[] SelectionScreenUIButtonsSO = new SO_Button[4];

    [Header("HUDUI")]
    [SerializeField] private SO_Button[] HUDUIButtonsSO = new SO_Button[4];

    private E_UIScreens currentUI = E_UIScreens.MainMenuUI;

    public UnityEvent<PlayerEntity> onScoreChanged;

    void Awake()
    {   
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttonsComponent[i] = buttons[i].GetComponent<Button>();
            texts[i] = buttons[i].transform.GetChild(0).gameObject;
            textsComponent[i] = texts[i].GetComponent<TextMeshProUGUI>();
        }

        background = transform.GetChild(0).gameObject;
        currentBackground = background.GetComponent<Image>();

        ChangeUIScreen(E_UIScreens.MainMenuUI);

        onScoreChanged.AddListener(AddPointToHUD);
    }

    private void OnInteraction(Button button, SO_Button SO_button)
    {
        switch (SO_button.actionToExecute)
        {
            case E_ButtonActions.Play:
                ChangeUIScreen(E_UIScreens.SelectionScreenUI);
            break;

            case E_ButtonActions.Settings:
            
            break;

            case E_ButtonActions.Exit:
                ExitGame();
                break;

            case E_ButtonActions.BackToMenu:
                ChangeUIScreen(E_UIScreens.MainMenuUI);
                break;

            case E_ButtonActions.TwoPlayers:
                SelectPlayer(2);
                ChangeUIScreen(E_UIScreens.HUDUI);
                break;

            case E_ButtonActions.ThreePlayers:
                SelectPlayer(3);
                ChangeUIScreen(E_UIScreens.HUDUI);
                break;

            case E_ButtonActions.FourPlayers:
                SelectPlayer(4);
                ChangeUIScreen(E_UIScreens.HUDUI);
                break;
        }
    }

    private void SelectPlayer(int playerAmount)
    {
        GameManager.Instance.SetPlayerAmount(playerAmount);
        SceneChanger.Instance.LoadScene("Testing");
    }

    private void AddPointToHUD(PlayerEntity player)
    {
        MyBehaviorType playerNumber = player.playerBehavior.type;
        switch (playerNumber)
        {
            case MyBehaviorType.Player1:
                textsComponent[0].text = HUDUIButtonsSO[0].text + player.Score.ToString();

                break;
            case MyBehaviorType.Player2:
                textsComponent[1].text = HUDUIButtonsSO[1].text + player.Score.ToString();

                break;
            case MyBehaviorType.Player3:
                textsComponent[2].text = HUDUIButtonsSO[2].text + player.Score.ToString();

                break;
            case MyBehaviorType.Player4:
                textsComponent[3].text = HUDUIButtonsSO[3].text + player.Score.ToString();

                break;
        }
    }

    public void ExitGame()
    {
        SceneChanger.Instance.QuitGame();
    }

    private void ChangeUIScreen(E_UIScreens newScreen)
    {
        currentUI = newScreen;
        switch (currentUI)
        {
            case E_UIScreens.MainMenuUI:
                currentBackground.enabled = true;
                currentBackground.sprite = backgroundImages[0];
                for (int i = 0; i < buttons.Length; i++)
                {
                    var index = i;
                    buttonsComponent[i].onClick.RemoveAllListeners();
                    buttons[i].transform.SetParent(layoutGroup[0].transform);
                    textsComponent[i].text = MainMenuUIButtonsSO[i].text;
                    textsComponent[i].color = MainMenuUIButtonsSO[i].textColor;
                    buttonsComponent[i].interactable = MainMenuUIButtonsSO[i].isInteractable;
                    if (buttonsComponent[i].interactable)
                    {
                        buttonsComponent[i].onClick.AddListener(() => OnInteraction(buttonsComponent[index], MainMenuUIButtonsSO[index]));
                    }
                    buttonsComponent[i].enabled = MainMenuUIButtonsSO[i].isActive;
                }
                break;

            case E_UIScreens.SelectionScreenUI:
                currentBackground.enabled = true;
                currentBackground.sprite = backgroundImages[1];
                for (int i = 0; i < buttons.Length; i++)
                {
                    var index = i;
                    buttonsComponent[i].onClick.RemoveAllListeners();
                    buttons[i].transform.SetParent(layoutGroup[1].transform);
                    textsComponent[i].text = SelectionScreenUIButtonsSO[i].text;
                    textsComponent[i].color = SelectionScreenUIButtonsSO[i].textColor;
                    buttonsComponent[i].interactable = SelectionScreenUIButtonsSO[i].isInteractable;
                    if (buttonsComponent[i].interactable)
                    {
                        buttonsComponent[i].onClick.AddListener(() => OnInteraction(buttonsComponent[index], SelectionScreenUIButtonsSO[index]));
                    }
                    buttonsComponent[i].enabled = SelectionScreenUIButtonsSO[i].isActive;
                }
                break;

            case E_UIScreens.HUDUI:
                currentBackground.sprite = backgroundImages[2];
                currentBackground.enabled = false;
                for (int i = 0; i < buttons.Length; i++)
                {
                    var index = i;
                    buttonsComponent[i].onClick.RemoveAllListeners();
                    buttons[i].transform.SetParent(layoutGroup[2].transform);
                    textsComponent[i].text = HUDUIButtonsSO[i].text;
                    textsComponent[i].color = HUDUIButtonsSO[i].textColor;
                    buttonsComponent[i].interactable = HUDUIButtonsSO[i].isInteractable;
                    if (buttonsComponent[i].interactable)
                    {
                        buttonsComponent[i].onClick.AddListener(() => OnInteraction(buttonsComponent[index], HUDUIButtonsSO[index]));
                    }
                    buttonsComponent[i].enabled = HUDUIButtonsSO[i].isActive;
                }
                break;
        }
    }






}
