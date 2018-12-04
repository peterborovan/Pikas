using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// toto neviem zatial urobit lepsie...
public static class MySceneManager {
    private static int sizeX = 4, sizeY = 4; // default
    [Header("Players")]
    private static Player mainPlayer = null;
    private static Player secondPlayer = null;

    public static void LoadScene(string sceneName, int _sizeX, int _sizeY)
    {
        sizeX = _sizeX;
        sizeY = _sizeY;
        SceneManager.LoadScene(sceneName);
    }
    public static void setMainPlayer(Player p)
    {
        mainPlayer = p;
    }
    public static Player getMainPlayer()
    {
        return mainPlayer;
    }
    public static void setSecondPlayer(Player p)
    {
        secondPlayer = p;
    }
    public static Player getSecondPlayer()
    {
        return secondPlayer;
    }

    public static int getSizeX()
    {
        return sizeX;
    }
    public static int getSizeY()
    {
        return sizeY;
    }
}
