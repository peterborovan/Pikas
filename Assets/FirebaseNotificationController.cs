using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FirebaseNotificationController : MonoBehaviour
{
    /*
     pri database som si 2x narazil nos.
     1. pre prve pokusy treba do rules databazy nastavit
     {
       "rules": {
            ".read": true,
            ".write": true
        }
     } - to znamena, ze ziadna autentifikacia
     Autentifikaciu budem riesit, ak nieco pobezi...

     2. Defaultne mi Firebase ponukal databazu Cloud Firebase BETA - next generation...
     asi je to cesta, ale neprekonal som permission denied. Ani ked som dobre nastavil rules
     bez autentifikacie...
     */
     
    public void Start()
    {
        //FirebaseVerifyDependencies();
        FirebaseRegisterPushNotifs();
    }

public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
        SSTools.ShowMessage("Received Registration Token: " + token.Token, SSTools.Position.bottom, SSTools.Time.oneSecond);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
        SSTools.ShowMessage("Received a new message from: " + e.Message.From, SSTools.Position.bottom, SSTools.Time.oneSecond);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // nefunguje, neviem preco
    public void FirebaseRegisterPushNotifs()
    {
        SSTools.ShowMessage("Register Firebase", SSTools.Position.bottom, SSTools.Time.oneSecond);
        UnityEngine.Debug.Log("Register Firebase for Push Notfication");
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }
    public void FirebaseVerifyDependencies()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp, i.e.
                //   app = Firebase.FirebaseApp.DefaultInstance;
                // where app is a Firebase.FirebaseApp property of your application class.

                // Set a flag here indicating that Firebase is ready to use by your
                // application.
                SSTools.ShowMessage("Firebase is ready", SSTools.Position.bottom, SSTools.Time.oneSecond);
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
                SSTools.ShowMessage("Firebase Unity SDK is not safe", SSTools.Position.bottom, SSTools.Time.oneSecond);
            }
        });
    }
}
