using TMPro;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class joining_game_ui_manager : MonoBehaviour
{
    [SerializeField] private UIDocument menuDocument = null;
    [SerializeField] private UIDocument inLobbyDocument = null;
    private TextElement codeText = null;
    private TextElement infoText = null;
    private Button startButton = null;
    private Button createLobbyButton = null;
    private Button joinLobbyButton = null;
    private Button quickPlayButton = null;
    private Button backToMainMenu = null;
    private Button backToChooseLobby = null;
    private TextField joinTextField = null;

    private void Awake()
    {
        createLobbyButton = menuDocument.rootVisualElement.Q("CreateLobby") as Button;
        joinLobbyButton = menuDocument.rootVisualElement.Q("SubmitCode") as Button;
        quickPlayButton = menuDocument.rootVisualElement.Q("QuickPlay") as Button;
        joinTextField = menuDocument.rootVisualElement.Q("JoinCode") as TextField;
        codeText = inLobbyDocument.rootVisualElement.Q("Code") as TextElement;
        infoText = inLobbyDocument.rootVisualElement.Q("Info") as TextElement;
        startButton = inLobbyDocument.rootVisualElement.Q("Start") as Button;
        backToMainMenu = menuDocument.rootVisualElement.Q("Back") as Button;
        backToChooseLobby = inLobbyDocument.rootVisualElement.Q("Back") as Button;


        createLobbyButton.clicked += OnCreateLobbyClicked;
        joinLobbyButton.clicked += OnJoinLobbyClicked;
        startButton.clicked += StartGame;

        backToMainMenu.clicked += () =>
        {
            SceneManager.LoadScene("MainMenu");
        };

        backToChooseLobby.clicked += () =>
        {
            multiplayer_manager.Instance.LeaveLobbyAndRelay();
            inLobbyDocument.sortingOrder = -1;
            menuDocument.sortingOrder = 1;
        };
    }

    private async void OnCreateLobbyClicked()
    {
        string lobbyCode = await multiplayer_manager.Instance.CreateLobbyAndRelay("Game");
        HostStarted(lobbyCode);
    }

    private async void OnJoinLobbyClicked()
    {
        string lobbyCode = joinTextField.text;
        string result = await multiplayer_manager.Instance.JoinLobbyAndRelay(lobbyCode);
        if( result != null)
        {
            ClientStarted(result);
        }
        else
        {

        }
    }

    private void StartGame()
    {
        //Start game logic
    }



    private void HostStarted(string code)
    {
        menuDocument.sortingOrder = -1;
        inLobbyDocument.sortingOrder = 1;
        codeText.text = "CODE: " + code.ToUpper(); 
        infoText.text = "Start game when ready...";
    }

    public void ClientStarted(string code)
    {
        menuDocument.sortingOrder = -1;
        inLobbyDocument.sortingOrder = 1;
        codeText.text = "Code " + code;
        infoText.text = "Waiting for host...";
    }
}
