using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;

// zdroj: Lynda.com
public class DatabaseManager : MonoBehaviour
{

    public static DatabaseManager sharedInstance = null;

    /// <summary>
    /// Awake this instance and initialize sharedInstance for Singleton pattern
    /// </summary>
    void Awake() {
        Debug.LogFormat("DatabaseManager Awake()");

        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else if (sharedInstance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        // firebase console, login: borovansky@gmail.com, ..., project MyProject
        // authentifications: email (login, pass)
        // default user borovansky@gmail.com/Unity123
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://handy-theory-148710.firebaseio.com");

        Debug.Log(Router.Players());
    }

    public void CreateNewPlayer(Player player, string uid)
    {
        string playerJSON = JsonUtility.ToJson(player);
        Router.PlayerWithUID(uid).SetRawJsonValueAsync(playerJSON);
    }
    public void GetPlayers(Action<List<Player>> completionBlock) {
        Debug.LogFormat("GetPlayers");
        List<Player> tmpList = new List<Player>();
        Router.Players().GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("error loading players");
            }
            else
            {
                Debug.LogFormat("new players");
                DataSnapshot players = task.Result;
                //Debug.LogFormat("new players: {0}", players.Children.ToString());
                foreach (DataSnapshot playerNode in players.Children)
                {
                    Debug.LogFormat("new player in db");
                    var playerDict = (IDictionary<string, object>)playerNode.Value;
                    Player newPlayer = new Player(playerDict);
                    tmpList.Add(newPlayer);
                }
                completionBlock(tmpList);
            }
        });
    }
}