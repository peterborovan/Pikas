using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FacePickerManager : MonoBehaviour {

    private static string selectedFace = "Alien";

    [SerializeField] public int sizeX, sizeY;
    public List<Sprite> faceSpriteList = new List<Sprite>();

    [Header("parent object pre Cards")]
    public Transform playGround;
    // toto bude ButtonPrefab
    public GameObject cardPrefab;

    private void Awake()
    {
        initTableau();
    }
    public void Start()
    {
        
    }
    void initTableau()
    {
        Debug.LogFormat("initTableau");
        List<int> indexes = new List<int>();
        for (int i = 0; i < sizeX * sizeY; i++) {
            GameObject newCard = Instantiate(cardPrefab, playGround);
            FaceCard card = newCard.GetComponent<FaceCard>();
            card.FaceFileName = faceSpriteList[i].name;
            Sprite texture = Resources.Load<Sprite>(faceSpriteList[i].name);
            if (texture != null) {
                Debug.LogFormat("initTableau {0}", faceSpriteList[i].name);
                Image img = card.GetComponent<Image>();
                img.sprite = texture;
            }
        }
    }

    public static string getSelectedFace()
    {
        return selectedFace;
    }
    public static void setSelectedFace(string face)
    {
        Debug.LogFormat("SelectedFace={0}", face);
        selectedFace = face;
        SceneManager.LoadScene("Player List");
    }
    public void OnBackButtonClick()
    {
        SceneManager.LoadScene("Login");
    }
}
