using UnityEngine;
using Unity.Services.Core;
using System;
using Unity.Mathematics;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;


public class unity_service_manager : MonoBehaviour
{
    [SerializeField] private main_menu_manager mainMenuManager = null;


    private async void Start()
    {
        await InitializeAndLogIn();
    }


    private async Task InitializeAndLogIn()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch(Exception e)
        {
           Debug.Log(e.Message); 
        }

        if (AuthenticationService.Instance.SessionTokenExists && !AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            mainMenuManager.UpdateNameText(AuthenticationService.Instance.PlayerName);
        }
        else if (AuthenticationService.Instance.IsSignedIn)
        {
            mainMenuManager.UpdateNameText(AuthenticationService.Instance.PlayerName);
            return;
        }
        else
        {
            mainMenuManager.LoadAuthenticateDocument();
        }

    }

   



    public async void SignIn(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            mainMenuManager.UpdateNameText(AuthenticationService.Instance.PlayerName);
            mainMenuManager.HideSignIn();
        }
        catch (AuthenticationException ex)
        {
            Debug.Log(ex.Message);
        }
        catch (RequestFailedException e)
        {
            Debug.Log(e.Message);
        }
        
    }

    public async void SignUp(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
            mainMenuManager.UpdateNameText(AuthenticationService.Instance.PlayerName);
            mainMenuManager.HideSignIn();
        }
        catch (AuthenticationException ex)
        {

            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {

            Debug.LogException(ex);

        }
    }

    public void SignOut()
    {
        try
        {
            AuthenticationService.Instance.SignOut();
            AuthenticationService.Instance.ClearSessionToken();
        }
        catch(AuthenticationException e)
        {
            Debug.Log(e.Message);
        }

        mainMenuManager.UpdateNameText("");
    }

}
