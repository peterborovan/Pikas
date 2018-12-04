using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// zdroj: Lynda.com
public class RowConfig : MonoBehaviour {
    public Text score;
    public Text email;
    public Text level;
    public Image profilePic;
    public List<Sprite> imagesList;

    public void Initialise(Player player) {
        Debug.LogFormat("RowConfig Initialise");

        this.score.text = player.score.ToString();
        this.email.text = player.email;
        this.level.text = player.level.ToString();

        this.profilePic.sprite = imagesList[Random.Range(0, 3)]; // 0-Janko, 1-Marienka, 2-Alien
    }
}