using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using TMPro;

// zdroj: Lynda.com
public class ProfileConfig : MonoBehaviour {
    public TextMeshProUGUI emailString;
    public Image profilePic;

    public void Config(Firebase.Auth.FirebaseUser user) {
        this.emailString.text = string.Format("ty si: {0}", user.Email);
    }
}