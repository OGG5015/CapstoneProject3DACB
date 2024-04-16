using UnityEngine;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using Unity.Services.Core;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class AuthManager : MonoBehaviour
{
    public Text logTxt;

    [SerializeField] private TMP_InputField loginUsernameField = default;
    [SerializeField] private TMP_InputField loginPasswordField = default;
    [SerializeField] private TMP_InputField signupUsernameField = default;
    [SerializeField] private TMP_InputField signupPasswordField = default;

    async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    public async void SignInAnon()
    {
        await signInAnonymous();

    }

    public async void SignIn()
    {
        string username = loginUsernameField.text;
        string password = loginPasswordField.text;
        await SignInWithUsernamePasswordAsync(username, password);
    }

    public async void Signup()
    {
        string newUserUsername = signupUsernameField.text;
        string newUserPassword = signupPasswordField.text;
        await SignUpWithUsernamePassword(newUserUsername, newUserPassword);
    }

    public void SignOut()
    {
        AuthenticationService.Instance.SignOut();
        logTxt.text = "Logged out";
    }

    async Task signInAnonymous()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            print("Sign in successful");
            print("Player ID: " + AuthenticationService.Instance.PlayerId);
            logTxt.text = "Player ID: " + AuthenticationService.Instance.PlayerId;

            
        }
        catch (AuthenticationException e)
        {
            print("Sign in failed");
            Debug.LogException(e);
        }

    }

    async Task SignInWithUsernamePasswordAsync(string username, string password)
    {

        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            
            print(AuthenticationService.Instance.PlayerName);
            Debug.Log("SignIn is successful.");
            
            logTxt.text = "Player ID: " + AuthenticationService.Instance.PlayerId;
            Debug.Log("Player ID: " + AuthenticationService.Instance.PlayerId);
            
            if(AuthenticationService.Instance.PlayerName == null)
            {
                _ = AuthenticationService.Instance.UpdatePlayerNameAsync(username);
                Debug.Log("Player name updated -> " + AuthenticationService.Instance.PlayerName);
            }

            SceneManager.LoadScene("Game_Options");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        
    }
    async Task SignUpWithUsernamePassword(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            Debug.Log("SignUp is successful.");
            _ = AuthenticationService.Instance.UpdatePlayerNameAsync(username);
            
            print("Player ID: " + AuthenticationService.Instance.PlayerId);
            logTxt.text = "Player ID: " + AuthenticationService.Instance.PlayerId;
            //print("Player Name: " + AuthenticationService.Instance.PlayerName);
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
}
