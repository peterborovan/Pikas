using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour {

    // singleton
    public static ScoreManager instance;
    private int elapsedTime;
    public TextMeshProUGUI elapsedTimeText;
    public TextMeshProUGUI scoreText1;
    public TextMeshProUGUI scoreText2;
    public TextMeshProUGUI nextTurnText;
    private const int COUNTDOWN = 10; // 10 sec na tah
    private int countDownForOneMove = COUNTDOWN;

    void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start() {
        Debug.LogFormat("MainPlayer is {0}", MySceneManager.getMainPlayer().nick);
        Debug.LogFormat("SecondPlayer is {0}", MySceneManager.getSecondPlayer().nick);
        StartCoroutine(Timer());
        ScoreManager.instance.showPlayer();
        ScoreManager.instance.showScore();
    }

IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            elapsedTime++;
            countDownForOneMove--;
            int seconds = elapsedTime % 60;
            int minutes = (elapsedTime / 60) % 60;
            // not used... int hours = (elapsedTime / 60) / 60;
            string progress = "";
            if (countDownForOneMove < 0)
            {
                Debug.Log("timeout ");
                updateNextPlayer();
                showPlayer();
            }
            else
            {   // urcite to ma nejaky progress bar...
                for (int i = 0; i < countDownForOneMove; i++)
                    progress = progress + ".";
            }

            elapsedTimeText.text = "cas: " + //hours.ToString("D2") + ":" + 
                minutes.ToString("D2") + ":" + seconds.ToString("D2") + progress;
        }
    }
    public void updateScore()
    {
        Player p = MySceneManager.getMainPlayer();
        p.score++;
        MySceneManager.setMainPlayer(p);
    }

    public void showScore()
    {
        scoreText1.text = //"skore: " +
            MySceneManager.getMainPlayer().nick + ":" + MySceneManager.getMainPlayer().score;
        scoreText2.text = //"skore: " +
            MySceneManager.getSecondPlayer().nick + ":" + MySceneManager.getSecondPlayer().score;
    }
    public  void updateNextPlayer()
    {
        Debug.LogFormat("SWAP PLAYERS");
        Player tmp = MySceneManager.getMainPlayer();
        MySceneManager.setMainPlayer(MySceneManager.getSecondPlayer());
        MySceneManager.setSecondPlayer(tmp);
        resetCountdown();
        showScore();
    }
    public void resetCountdown()
    {
        countDownForOneMove = COUNTDOWN;
    }
    public void showPlayer()
    {
        // asi netreba...
        //nextTurnText.text = "teraz ide: " + MySceneManager.getMainPlayer().nick;
    }
}
