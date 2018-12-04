using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player {
    public string nick;
    public string email;
    public int score;
    public int level;
    public string icon;


    public Player(string nick, string email, int score, int level, string icon)
    {
        Debug.LogFormat("new player constructor: {0}", email);
        this.nick = nick;
        this.email = email;
        this.score = score;
        this.level = level;
        this.icon = icon;
    }
    private static string janko = "Janko";
    private static string jankoEmail = "janko.odbabicky@yahoo.com";
   // private static string jankoPass = "Unity123";
    private static string jankoIcon = "avatar_boy_man_max_icon_256";
    public static Player getJanko()
    {
        return new Player(janko, jankoEmail, 0, 1, jankoIcon);
    }
    private static string marienka = "Marienka";
    private static string marienkaEmail = "marienka.odbabicky@yahoo.com";
   // private static string marienkaPass = "Unity123";
    private static string marienkaIcon = "avatar_girl_person_user_woman_icon_256";
    public static Player getMarienka()
    {
        return new Player(marienka, marienkaEmail, 0, 1, marienkaIcon);
    }
    public Player(IDictionary<string, object> dict)
    {
        this.nick = dict["nick"].ToString();
        this.email = dict["email"].ToString();
        Debug.LogFormat("new player constructor: {0}", this.email);
        this.score = Convert.ToInt32(dict["score"]);
        this.level = Convert.ToInt32(dict["level"]);
        //this.icon = Convert.ToInt32(dict["icon"]);
        this.icon = dict["icon"].ToString();
    }
}