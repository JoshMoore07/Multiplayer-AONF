using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class main_menu_manager : MonoBehaviour
{
    [SerializeField] unity_service_manager serviceManager = null;

    [SerializeField] private UIDocument mainMenuDocument = null;
    [SerializeField] private UIDocument authenticateDocument = null;
    [SerializeField] private UIDocument signInDocument = null;
    private Button playButton = null;
    private Button settingsButton = null;
    private Button signOutButton = null;
    private Button signInButton = null;
    private Button signUpButton = null;
    private Button submitSignIn = null;
    private TextField usernameSignIn = null;
    private TextField passwordSignIn = null;
    private TextElement nameText = null;
    private TextElement passwordTip = null;


    private void Awake()
    {
        if (mainMenuDocument != null)
        {
            playButton = mainMenuDocument.rootVisualElement.Q("Play") as Button;
            settingsButton = mainMenuDocument.rootVisualElement.Q("Settings") as Button;
            signOutButton = mainMenuDocument.rootVisualElement.Q("SignOut") as Button;
            nameText = mainMenuDocument.rootVisualElement.Q("PlayerName") as TextElement;

            playButton.clicked += PressedPlay;
            settingsButton.clicked += PressedSettings;
            signOutButton.clicked += PressedSignOut;
        }

        if (authenticateDocument != null)
        {
            signInButton = authenticateDocument.rootVisualElement.Q("SignIn") as Button;
            signUpButton = authenticateDocument.rootVisualElement.Q("SignUp") as Button;

            signInButton.clicked += PressedSignIn;
            signUpButton.clicked += PressedSignUp;
        }

        if (signInDocument != null)
        {
            usernameSignIn = signInDocument.rootVisualElement.Q("Username") as TextField;
            passwordSignIn = signInDocument.rootVisualElement.Q("Password") as TextField;
            submitSignIn = signInDocument.rootVisualElement.Q("Submit") as Button;
            passwordTip = signInDocument.rootVisualElement.Q("PasswordTip") as TextElement;

            submitSignIn.clicked += PressedSubmit;
        }

    }

    private void OnDisable()
    {
        playButton.clicked -= PressedPlay;
        settingsButton.clicked -= PressedSettings;
        signOutButton.clicked -= PressedSignOut;
        signInButton.clicked -= PressedSignIn;
        signUpButton.clicked -= PressedSignUp;
        submitSignIn.clicked -= PressedSubmit;
}

    private void PressedPlay()
    {
        SceneManager.LoadScene("JoinGame");
    }

    private void PressedSettings()
    {
        Debug.Log("Pressed Settings");
    }

    private void PressedSignOut()
    {
        Debug.Log("Pressed Sign Out");
        serviceManager.SignOut();
        LoadAuthenticateDocument();
    }

    private void PressedSignIn()
    {
        authenticateDocument.sortingOrder = -1;
        signInDocument.sortingOrder = 1;
        submitSignIn.text = "SIGN IN";
        passwordTip.visible = false;
    }

    private void PressedSignUp()
    {
        authenticateDocument.sortingOrder = -1;
        signInDocument.sortingOrder = 1;
        submitSignIn.text = "SIGN UP";
        passwordTip.visible = true;
    }

    private void PressedSubmit()
    {
        if(submitSignIn.text == "SIGN IN")
        {
            serviceManager.SignIn(usernameSignIn.text, passwordSignIn.text);
        }
        else
        {
            serviceManager.SignUp(usernameSignIn.text, passwordSignIn.text);
        }
    }


    public void LoadAuthenticateDocument()
    {
        authenticateDocument.sortingOrder = 1;
    }

    public void HideAuthenticateDocument()
    {
        signInDocument.sortingOrder = -1;
    }

    public void HideSignIn()
    {
        signInDocument.sortingOrder = -1;
    }


    public void UpdateNameText(string name)
    {
        nameText.text = name;
    }
    
}
