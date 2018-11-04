using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;

public class IntroButtons : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        /*
       Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith( task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp, i.e.
                //   app = Firebase.FirebaseApp.DefaultInstance;
                // where app is a Firebase.FirebaseApp property of your application class.

                // Set a flag here indicating that Firebase is ready to use by your
                // application.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
        */

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void JankoBtnPressed()
    {
        Debug.Log("Janko chce hrat");
        SceneManager.LoadScene("Levels");
    }
    public void MarienkaBtnPressed()
    {
        Debug.Log("Marienka chce hrat");
        SceneManager.LoadScene("Levels");
    }
    public void ExitBtnPressed()
    {
        Debug.Log("Exit z hry");
        Application.Quit(); //this will quit our game. Note this will only work after building the game
    }
    public void HelpBtnPressed()
    {
        Debug.Log("Help yourself");
        SceneManager.LoadScene("Ledd");
    }
    public void BackToIntroBtnPressed()
    {
        Debug.Log("Bact to Intro");
        SceneManager.LoadScene("Intro");
    }
}
