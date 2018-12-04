using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

// zdroj: Lynda.com
public class Router : MonoBehaviour {
    private static DatabaseReference baseRef = FirebaseDatabase.DefaultInstance.RootReference;

    public static DatabaseReference Players()
    {
        //return FirebaseDatabase.DefaultInstance.GetReference("players");
        return baseRef.Child("players");
        //return baseRef.Child ("players").OrderByValue("score").LimitToFirst(100);
    }

    public static DatabaseReference PlayerWithUID(string uid)
    {
        return baseRef.Child("players").Child(uid);
    }
}