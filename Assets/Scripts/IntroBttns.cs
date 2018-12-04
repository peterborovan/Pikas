using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;

public class IntroBttns : MonoBehaviour {
    public AuthManager authManager;

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
    public void JankoBtnPressed()
    {
        Debug.Log("Janko chce hrat s Marienkou");
        Player janko = Player.getJanko();
        authManager.LoginExistingUser(janko.email, "Unity123");
        MySceneManager.setMainPlayer(janko);
        MySceneManager.setSecondPlayer(Player.getMarienka());
        SSTools.ShowMessage("Janko chce hrat s Marienkou", SSTools.Position.bottom, SSTools.Time.oneSecond);
        SceneManager.LoadScene("Levels");
        //SceneManager.LoadScene("Player List");
    }
    public void MarienkaBtnPressed()
    {
        Debug.Log("Marienka chce hrat s Jankom");
        Player marienka = Player.getMarienka();
        authManager.LoginExistingUser(marienka.email, "Unity123");
        MySceneManager.setMainPlayer(marienka);
        MySceneManager.setSecondPlayer(Player.getJanko());
        SSTools.ShowMessage("Marienka chce hrat s Jankom", SSTools.Position.bottom, SSTools.Time.oneSecond);
        SceneManager.LoadScene("Levels");
        //SceneManager.LoadScene("Player List");
    }
    public void AlienBtnPressed()
    {
        Debug.Log("Mimozemstan chce hrat");
        SSTools.ShowMessage("Mimozemstan chce hrat", SSTools.Position.bottom, SSTools.Time.oneSecond);
        SceneManager.LoadScene("Login");
    }
    public void ExitBtnPressed()
    {
        Debug.Log("Exit z hry");
        Application.Quit(); //this will quit our game. Note this will only work after building the game
    }
    public void HelpBtnPressed()
    {
        Debug.Log("Help yourself");
        SSTools.ShowMessage("Help yourself", SSTools.Position.bottom, SSTools.Time.oneSecond);
    }

    // zbytocne
    public void zapisDoFB() { 
        // prvy pokus zapisat nieco a precitat z FB
        FirebaseConnect db = new FirebaseConnect();
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("key1", "Janko");
        data.Add("key2", "Marienka");
        db.UpdateResultDictionary(data, "/path");
        db.ReadData("/path");

        ///////////////////-----------------
        //VirtualClass vcl = new VirtualClass("Babi�kin� mazl��ci");  kedze z toho je email, tak to neslo
        VirtualClass vcl = new VirtualClass("BabickiniMazlicci");
        Student studentJanko = new Student("Janko", "borovansky@gmail.com", vcl, "123");
        Student studentkaMarienka = new Student("Marienka", "borovansky@yahoo.com", vcl, "456");
        db.RegistrationNewAccount(studentJanko);
        db.RegistrationNewAccount(studentkaMarienka);
        db.Login(studentJanko);
        db.Login(studentkaMarienka);
    }
    public void BackToIntroBtnPressed()
    {
        Debug.Log("Back to Intro");
        SceneManager.LoadScene("Intro");
    }
}
