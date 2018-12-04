using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;
using System;

public class FirebaseConnect
{
    private DatabaseReference mDatabaseRef;
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    //private string sceneName;
    private PlayerData playerData = new PlayerData();

    public FirebaseConnect()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://handy-theory-148710.firebaseio.com");
        mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }
    //---------------- FB basics, dobre na pochopenie, ako tam nieco zapisat a precitat
    public void UpdateResultDictionary(Dictionary<string, string> ex, string pathToSharedData)
    {
        LeaderBoardEntry entry = new LeaderBoardEntry(ex);
        Dictionary<string, object> entryValues = entry.ToDictionary();
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates[pathToSharedData] = entryValues;
        mDatabaseRef.UpdateChildrenAsync(childUpdates);
    }
    public void ReadData(string key)
    {
        FirebaseDatabase.DefaultInstance
            .GetReference(key)
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    // Handle the error...
                    Debug.Log(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    // Do something with snapshot...
                    //Debug.Log("got results=" + snapshot.ToString());
                    Debug.Log("got results=" + snapshot.GetRawJsonValue());
                    // nikam to nejde, len na konzolu :(
                }
            });
    }
    // toto sa nezavolalo po zmene dat vo FB ???
    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Debug.Log ("value changed: " + args.Snapshot.GetRawJsonValue());
        //----- Do something with the data in args.Snapshot
    }
    // ------------- prevzate od Daniela ---------------------------------
    public void SaveGlobalDataAll(string name, string userId, Dictionary<string, string> classses, bool loggedUser)
    {
        GlobalData.playerData.Name = name;
        GlobalData.playerData.UserId = userId;
        GlobalData.playerData.Classes = classses;
        GlobalData.playerData.LoggedUser = loggedUser;
    }

    public void SaveGlobalDataUserId(string userId)
    {
        GlobalData.playerData.UserId = userId;
    }
    public void SaveGlobalDataLoggedUser(bool loggedUser)
    {
        GlobalData.playerData.LoggedUser = loggedUser;
    }

    public void SaveGlobalSelectedClass(string @class)
    {
        GlobalData.playerData.SelectedClass = @class;
    }

    #region Auth
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
                playerData.LoggedUser = false;
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                playerData.LoggedUser = true;
                playerData.UserId = auth.CurrentUser.UserId;
            }
        }
        SaveGlobalDataUserId(playerData.UserId);
        SaveGlobalDataLoggedUser(playerData.LoggedUser);
    }

    public void OnDestroy(string sceneName)
    {
    //    this.sceneName = sceneName;
        auth.StateChanged -= AuthStateChanged;
        auth.SignOut();
        auth = null;
        SaveGlobalDataAll(null, null, null, false);
        SaveGlobalSelectedClass(null);
        SceneManager.LoadScene(sceneName);
    }
    // nickName by asi malo by unique... asi v triede mozu byt dve marienky...
    // mne sa priezvisko nepacilo, tak tom to vyhadzal...
    public void RegistrationNewAccount(Student student, string sceneName = null)
    {
        //this.sceneName = sceneName;
        string hopefullyUnique = student.getUnique();
        Debug.LogError("RegistrationNewAccount:" + hopefullyUnique) ; 
        auth.CreateUserWithEmailAndPasswordAsync(hopefullyUnique, student.getPasswd()).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            if (task.IsCompleted)
            {
                Firebase.Auth.FirebaseUser newUser = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
                RegistrationSaveData(student, newUser.UserId);
                playerData.Name = student.getName();
                if (sceneName != null)
                {
                    SceneManager.LoadScene(sceneName);
                }
            }
        });
    }

    public string Login(Student student, string sceneName = null)
    {
        //this.sceneName = sceneName;
        string hopefullyUnique = student.getUnique();
        Debug.LogError("Login:" + hopefullyUnique);
        Firebase.Auth.Credential credential =
                    Firebase.Auth.EmailAuthProvider.GetCredential(hopefullyUnique, student.getPasswd());
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return "";
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return  "Nesprávne meno alebo heslo.";
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                            newUser.DisplayName, newUser.UserId);
            if (sceneName != null)
            {
                SceneManager.LoadScene("Loading");
            }
            return "";
        });
        return "";
    }
    #endregion

    #region InsertDatabase
    private void RegistrationSaveData(Student student, string userId)
    {
        mDatabaseRef.Child("USERS").Child(userId).SetRawJsonValueAsync(JsonUtility.ToJson(student));
    }
    public void setSelectedClass(string userId, string classID)
    {
        mDatabaseRef.Child("USERS").Child(userId).Child("selectClass").SetValueAsync(classID);
    }
    private void addStudentToClass(string userId, string hesloTriedy, string classID)
    {
        mDatabaseRef.Child("USERS").Child(userId).Child("MY_CLASS").Child(classID).SetValueAsync(hesloTriedy);
    }
    public void addSharedScreen(string screenId, SharedScreen screen)
    {
        mDatabaseRef.Child("SHARED_SCREEN").Child(screenId).SetRawJsonValueAsync(JsonUtility.ToJson(screen));
    }
    public void addPinnedToTable(string screenId, SharedScreen screen)
    {
        mDatabaseRef.Child("EXAMS_PINNED_TO_TABLE").Child(screenId).SetRawJsonValueAsync(JsonUtility.ToJson(screen));
    }
    public void inserMyIdToSharedScreen(string userId, string userName, string screenId)
    {
        mDatabaseRef.Child("SHARED_SCREEN").Child(screenId).Child("users_id").Child(userId).SetValueAsync(userName);
    }
    public void SendRequestWithShareScreen(string userId, string key, SharedScreenRequest value)
    {
        mDatabaseRef.Child("USERS").Child(userId).Child("waitForShare").Child(key).SetRawJsonValueAsync(JsonUtility.ToJson(value));
    }
    public void insertIntoOnlineStudent(string classId, string userId, string name)
    {
        mDatabaseRef.Child("CLASSES").Child(classId).Child("ONLINE_STUDENTS").Child(userId).SetValueAsync(name);
    }
    public void insertIntoStudentsInClass(string classId, string userId, string name)
    {
        mDatabaseRef.Child("CLASSES").Child(classId).Child("STUDENTS_IN_CLASS").Child(userId).SetValueAsync(name);
    }
    public void insertIntoTableViews(string tableId, string userId, string examsViews)
    {
        mDatabaseRef.Child("USERS").Child(userId).Child("TABLE_VIEWS").Child(tableId).SetValueAsync(examsViews);
    }
    public void zapisStavRozriesenejUlohy(string userId, string idSelectedExamOnBoard, string txtPocet)
    {
        mDatabaseRef.Child("/USERS").Child(userId).Child("SOLVE_EXAMS").Child(idSelectedExamOnBoard).SetValueAsync(txtPocet);
    }
    public void addPinExamToTable(string classId, string examId)
    {
        mDatabaseRef.Child("/TABLES").Child(classId).Child(examId).SetValueAsync("pin");
    }

    public void zapisDatumActualScreen(string path)
    {
        mDatabaseRef.Child(path).SetValueAsync(DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss"));
    }
    /*
    public void logovanieStatistik(string userId, string classId, string typLogu)
    {
        FirebaseDatabase.DefaultInstance.GetReference("/STATISTICS/" + classId + "/" + userId + "/" + typLogu)
                    .GetValueAsync().ContinueWith(task =>
                    {
                        if (task.IsCompleted)
                        {
                            DataSnapshot snapshot = task.Result;
                            var num = 0;
                            if (snapshot.Value == null)
                            {
                                num = 1;
                            }
                            else
                            {
                                if (int.TryParse(snapshot.Value.ToString(), out num))
                                {
                                    num++;
                                }
                            }
                            mDatabaseRef.Child("/STATISTICS").Child(classId).Child(userId).Child(typLogu).SetValueAsync(num);
                        }
                    });
    }
    */
    #endregion
    #region removeFromDB
    public void removeOfflineStudent(string classId, string userId)
    {
        FirebaseDatabase.DefaultInstance.GetReference("/CLASSES/" + classId + "/ONLINE_STUDENTS/").Child(userId).RemoveValueAsync();
    }
    #endregion
    #region LoadDataFromDB
    public void GetUserData(string userId, bool changeScene = false, string scene = "whatever")
    {

        if (playerData.LoggedUser)
        {
            FirebaseDatabase.DefaultInstance.GetReference("/USERS/" + userId + "/")
                        .GetValueAsync().ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                            {
                                Debug.Log("skontroluj pripojenie");
                                SceneManager.LoadScene("CheckConnection");
                            }
                            else if (task.IsCompleted)
                            {
                                DataSnapshot snapshot = task.Result;
                                playerData.Name = snapshot.Child("nickName").Value.ToString();
                                Dictionary<string, string> _classData = new Dictionary<string, string>();
                                foreach (var classData in snapshot.Child("MY_CLASS").Children.ToList())
                                {
                                    _classData[classData.Key] = classData.Value.ToString();
                                }
                                playerData.Classes = _classData;
                                playerData.UserId = userId;

                                if (changeScene)
                                {
                                    GlobalData.playerData = playerData;
                                    SaveGlobalDataAll(playerData.Name, userId, playerData.Classes, playerData.LoggedUser);
                                    SceneManager.LoadScene(scene);
                                }

                            }
                        });
        }
        else
        {
            SaveGlobalDataAll(null, null, null, false);
            SaveGlobalSelectedClass(null);
            SceneManager.LoadScene("Login");
        }
    }

    public Dictionary<string, string> getShareData(string path)
    {
        Dictionary<string, string> resultData = new Dictionary<string, string>();
        FirebaseDatabase.DefaultInstance.GetReference(path)
                    .GetValueAsync().ContinueWith(task => {
                        if (task.IsFaulted) { }
                        else if (task.IsCompleted)
                        {
                            DataSnapshot snapshot = task.Result;
                            foreach (var x in snapshot.Children)
                            {
                                resultData[x.Key] = x.Value.ToString();
                            }
                        }
                    });

        return resultData;
    }

    public void FindClass(string hesloTriedy, string studentID, string studentName)
    {
        FirebaseDatabase.DefaultInstance.GetReference("/CLASSES").OrderByChild("heslo").EqualTo(hesloTriedy)
                    .GetValueAsync().ContinueWith(task =>
                    {
                        if (task.IsFaulted) { }
                        else if (task.IsCompleted)
                        {
                            Debug.Log("FindClass");
                            DataSnapshot snapshot = task.Result;
                            bool exists = (snapshot.Value != null);
                            string classID = snapshot.Children.SingleOrDefault().Key;
                            string className = snapshot.Child(classID).Child("className").Value.ToString();
                            classExistCallback(hesloTriedy, classID, studentID, exists, className, studentName);
                        }
                    });
    }
    #endregion

    #region UpdateData
    public void UpdateResult(Dictionary<string, string> ex, string pathToSharedData)
    {
        LeaderBoardEntry entry = new LeaderBoardEntry(ex);
        Dictionary<string, object> entryValues = entry.ToDictionary();
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates[pathToSharedData] = entryValues;
        mDatabaseRef.UpdateChildrenAsync(childUpdates);
    }
    #endregion

    #region Callback
    private void classExistCallback(string hesloTriedy, string classID, string studentID, bool exists, string className, string studentName)
    {
        if (exists)
        {
            Debug.Log("class " + hesloTriedy + " exists!!!");
            insertIntoStudentsInClass(classID, studentID, studentName);
            insertIntoOnlineStudent(classID, studentID, studentName);
            setSelectedClass(studentID, classID);
            addStudentToClass(studentID, string.Format("{0} -> {1}", className, hesloTriedy), classID);
            insertIntoTableViews(classID, studentID, "0");
            GlobalData.playerData.Classes[classID] = string.Format("{0} -> {1}", className, hesloTriedy);
            SaveGlobalSelectedClass(classID);
            SceneManager.LoadScene("LoggedSelectLevel");

        }
        else
        {
            Debug.Log("class " + hesloTriedy + " not exists!!!");
        }
    }
    #endregion

}

//[System.Serializable]
public class LeaderBoardEntry
{
    public Dictionary<string, string> exam;

    public LeaderBoardEntry()
    {
    }

    public LeaderBoardEntry(Dictionary<string, string> exam)
    {
        this.exam = exam;
    }

    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        foreach (KeyValuePair<string, string> item in exam)
        {
            result[item.Key] = item.Value;
        }
        return result;
    }


}

public class SharedScreen
{
    public Dictionary<string, object> data = new Dictionary<string, object>();
    public bool screeen_locker;
    public string screen_name;
    public string admin_name;
    public string nazov_ulohy;
    public List<string> users_id = new List<string>();
    public List<int> pomSucet0and1 = new List<int>();
    public List<string> poliaKtoreSaNevykreslia = new List<string>();
    public List<string> poliaOznaceneDisable = new List<string>();

    public SharedScreen() { }
    public SharedScreen(Dictionary<string, object> data, bool screeen_locker, string screen_name, string admin_name, string nazov_ulohy, List<string> users_id, List<int> pomSucet0and1, List<string> poliaKtoreSaNevykreslia, List<string> poliaOznaceneDisable)
    {
        this.data = data;
        this.screeen_locker = screeen_locker;
        this.screen_name = screen_name;
        this.admin_name = admin_name;
        this.nazov_ulohy = nazov_ulohy;
        this.users_id = users_id;
        this.pomSucet0and1 = pomSucet0and1;
        this.poliaKtoreSaNevykreslia = poliaKtoreSaNevykreslia;
        this.poliaOznaceneDisable = poliaOznaceneDisable;
    }
}

public class SharedScreenRequest
{
    public string share_object;
    public string admin_name;

    public SharedScreenRequest() { }
    public SharedScreenRequest(string share_object, string admin_name)
    {
        this.share_object = share_object;
        this.admin_name = admin_name;
    }
}


