using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsBttns : MonoBehaviour {
    public Sprite mainPlayer = null;
    public Sprite secondPlayer = null;

    public void openLevel(int sizeX, int sizeY)
    {
        // zatial neviem, co s tym parametrom....
        //SceneManager.LoadScene("Playground");
        MySceneManager.LoadScene("Playground", sizeX, sizeY);
    }
    public void Start()
    {
        Debug.LogFormat("#1");
        if (MySceneManager.getMainPlayer() != null) {
            Debug.LogFormat("#2");
            string icon = MySceneManager.getMainPlayer().icon;
            if (icon != null)
            {
                Debug.LogFormat("#3");
                Sprite sprite = Resources.Load<Sprite>(icon);
                if (sprite != null)
                {
                    Debug.LogFormat("#4");
                    mainPlayer = sprite;
                    Debug.LogFormat("Main player sprite {0}", sprite);
                }
            }
        }
    }
    public void Level4()
    {
        Debug.Log("Hra 2x2");
        openLevel(2,2);
    }
    public void Level6()
    {
        Debug.Log("Hra 4x4");
        openLevel(4,4);
    }
    public void Level8()
    {
        Debug.Log("Hra 6x6");
        openLevel(6,6);
    }
    public void Level10()
    {
        Debug.Log("Hra 8x8");
        openLevel(8,8);
    }
    public void BackToIntroBtnPressed()
    {
        Debug.Log("Bact to Levels");
        SceneManager.LoadScene("Intro");
    }
}
