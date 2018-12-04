using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using UnityEngine.SceneManagement;

// zdroj: lynda.com
public class PlayerBoardManager : MonoBehaviour
{
    public List<Player> playerList = new List<Player>();

    public GameObject rowPrefab;
    public GameObject scrollContainer;
    public GameObject profilePanel;

    Firebase.Auth.FirebaseAuth auth;

    void Awake()
    {
        Debug.LogFormat("PlayerBoardManager Awake()");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        playerList.Clear();

        DatabaseManager.sharedInstance.GetPlayers(result => {
            playerList = result;
            Debug.LogFormat("First player in the list: {0}", playerList[0].email);

        /// !!! InitialiseUI();  // toto tu nemoze byt, lebo GUI sa nesmie updatovat asynchronne, z ineho tasku
        //DoInMainThread.ExecuteOnMainThread.Enqueue({} =>InitialiseUI());
        });
        //Router.Players().OrderByChild("level");
        //Router.Players().LimitToFirst(10);
        //Router.Players().OrderByChild("level").LimitToLast(5);
        //InitialiseUI();
        profilePanel.GetComponent<ProfileConfig>().Config(auth.CurrentUser);

        //Router.Players ().ChildAdded += NewPlayerAdded;
    }
    /* toto nepomohlo, vykona sa to skor, ako sa dotiahne zoznam playerov 
    void Start()
    {
        Debug.LogFormat("PlayerBoardManager start()");
        InitialiseUI();
    }
    */
    public void InitialiseUI()
    {
        Debug.LogFormat("Initialise UI");
        foreach (Player player in playerList) {
            CreateRow(player);
        }
    }

    void CreateRow(Player player) {
        if (rowPrefab == null)   {
            Debug.LogFormat("rowPrefab is null");
        }
        //?? GameObject newRow = Instantiate(rowPrefab, scrollContainer.transform);

        GameObject newRow = Instantiate(rowPrefab) as GameObject;
        newRow.GetComponent<RowConfig>().Initialise(player);
        newRow.transform.SetParent(scrollContainer.transform, false);
    }


    void NewPlayerAdded(object sender, ChildChangedEventArgs args) {
        if (args.Snapshot.Value == null) {
            Debug.Log("Sorry, there was no data at that node.");
        } else {
            Debug.Log("New player has joined the game!");
        }
    }
    public void onBackBtnClick()
    {
        SceneManager.LoadScene("Login");
    }
}
