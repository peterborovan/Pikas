using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using TMPro;

// zdroj: lynda.com
public class FormManager : MonoBehaviour {
    public TMP_InputField /*InputField*/ emailInput;
    public TMP_InputField /*InputField*/ passwordInput;
    public Button signUpButton;
    public Button loginButton;
    public TextMeshProUGUI statusText;
    public AuthManager authManager;

    //public static FormManager sharedInstance = null;

    void Awake() {
        Debug.LogFormat("FormManager Awake()");
        /*
        if (sharedInstance == null) {
            sharedInstance = this;
        } else if (sharedInstance != this) {
            Destroy(gameObject);
        }
        */
        // na debugovanie je to dobre zakomentovat, nech sa clovek neotravuje s emailom...
        ToggleButtonStates(false);

        // Auth delegate subscriptions
        if (authManager != null) {
            authManager.authCallback += HandleAuthCallback;
        }
        else {
            Debug.LogFormat("authManager==null");
        }
    }

    // Validates the email input
    public void ValidateEmail() {
        Debug.Log("Email Validator");
        string email = emailInput.text;
        var regexPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        if (email != "" && Regex.IsMatch(email, regexPattern)) {
            ToggleButtonStates(true);
        } else {
            ToggleButtonStates(false);
        }
    }

    // Firebase methods
    public void OnSignUp() {
        Debug.Log("Sign Up");
        authManager.SignUpNewUser(emailInput.text, passwordInput.text);
    }
    public void LoginUser(string user, string pass)
    {
        Debug.Log("Login");
        authManager.LoginExistingUser(user, pass);
    }
    public void OnLogin() {
        LoginUser(
            //"borovansky@gmail.com", "Unity123"
            emailInput.text, passwordInput.text
            );
    }
    public void OnBackBtn()
    {
        Debug.Log("MainMenu");
        SceneManager.LoadScene("Intro");
    }

    IEnumerator HandleAuthCallback(Task<Firebase.Auth.FirebaseUser> task, string operation)
    {
        Debug.LogFormat("authManager HandleAuthCallback()");
        if (task.IsFaulted || task.IsCanceled)
        {
            UpdateStatus("Chyba pri vytvoreni konta: " + task.Exception);
        }
        else if (task.IsCompleted) {
            if (operation == "sign_up") {
                Firebase.Auth.FirebaseUser newPlayer = task.Result;
                Debug.LogFormat("Vitaj dnu {0}!", newPlayer.Email);
                int score = 0;
                int level = 1;
                string icon = "Alien";
                Player player = new Player("Alien", newPlayer.Email, score, level, icon);
                MySceneManager.setMainPlayer(player);
                DatabaseManager.sharedInstance.CreateNewPlayer(player, newPlayer.UserId);
                UpdateStatus("...vyber si fotku...");
                yield return new WaitForSeconds(2.0f);
                SceneManager.LoadScene("FacePicker");
            } else {
                Firebase.Auth.FirebaseUser oldPlayer = task.Result;
                int score = 0;
                int level = 1;
                string icon = "Alien";
                MySceneManager.setMainPlayer(new Player("Alien", oldPlayer.Email, score, level, icon));
                UpdateStatus("...vyber si supera...");
                yield return new WaitForSeconds(2.0f);
                SceneManager.LoadScene("Player List");
            }
        }
    }

    void OnDestroy() {
        if (authManager != null) {
            authManager.authCallback -= HandleAuthCallback;
        } else {
            Debug.LogFormat("authManager==null");
        }
    }

    // Utilities
    private void ToggleButtonStates(bool toState) {
        signUpButton.interactable = toState;
        loginButton.interactable = toState;
    }

    private void UpdateStatus(string message) {
        statusText.text = message;
    }
}