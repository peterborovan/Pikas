using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;

// zdroj: Lynda.com
public class AuthManager : MonoBehaviour
{
    // Firebase API variables
    Firebase.Auth.FirebaseAuth auth;
    // Delegates
    public delegate IEnumerator AuthCallback(Task<Firebase.Auth.FirebaseUser> task, string operation);
    public event AuthCallback authCallback;

    void Awake()
    {
        Debug.LogFormat("authManager Awake()");
        auth = FirebaseAuth.DefaultInstance;
        if (auth == null) {
            Debug.LogFormat("auth is null");
        } else {
            Debug.LogFormat("auth is NOT null");
        }
    }

    public void SignUpNewUser(string email, string password)
    {
        Debug.LogFormat("SignUpNewUser({0},{1})", email, password);
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            DoInMainThread.ExecuteOnMainThread.Enqueue(() => {
                StartCoroutine(authCallback(task, "sign_up")); } );
        });
    }
    public void LoginExistingUser(string email, string password)
    {
        Debug.LogFormat("LoginUser({0},{1})", email, password);
        // niecomu nerozumiem....
        // nechapem, preco tu nemozem odpalit corutinu, zjavne to chce v jednom threade.
        // ale dovod nechapem
        // toto je chyba
        // Constructors and field initializers will be executed from the loading thread when loading a scene.
        // Don't use this function in the constructor or field initializers, instead move initialization code to the Awake or Start function.
        // uz chapem... v neMainThreade nemozem robit wait, public static to tiez neslo...
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            DoInMainThread.ExecuteOnMainThread.Enqueue(() => {
                StartCoroutine(authCallback(task, "login"));
            });
        });
    }
}