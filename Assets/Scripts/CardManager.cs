using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour {

    // toto chce byt singleton design pattern
    public static CardManager instance;

    // nezobraz v instpectore
    [HideInInspector] public int card1, card2;

    // zoznam vsetkych picas
    public List<Sprite> cardSpriteList = new List<Sprite>();

    [SerializeField] private List<GameObject> cardButtonList = new List<GameObject>();

    private int numberOfHiddenButtons = 0;

    // prave odkryte karticky, maximalne dve
    [SerializeField] public List<GameObject> choosenButtonList = new List<GameObject>();

    [Header("obdlznikova hracia plocha X,Y")]
    [SerializeField] public int sizeX, sizeY;

    [Header("parent object pre Cards")]
    public Transform playGround;

    [Header("Score and Time")]
    public int score;
    public int elapsedTime;

    // kto je na tahu
    //    public int nextMove;

    // toto bude ButtonPrefab
    public GameObject cardPrefab;
    [Header("Particles")]
    public GameObject particle;

    void initPlayground()
    {
        int cardIndex = 0;
        List<int> indexes = new List<int>();
        for (int i = 0; i < sizeX * sizeY; i++)
        {
            indexes.Add(1 + (cardIndex++) / 2);  // dva rovnake indexy (0,1) -> 1, (2,3) -> 2
        }
        int shuffleCount = Random.Range(50, 150);
        while (shuffleCount > 0)
        {
            shuffleCount--;
            int i1 = Random.Range(0, indexes.Count);
            int i2 = Random.Range(0, indexes.Count);
            //Debug.Log("Swap " + i1 + ", " + i2);
            int tmp = indexes[i1];
            indexes[i1] = indexes[i2];
            indexes[i2] = tmp;
        }
        for (int i = 0; i < indexes.Count; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, playGround);
            PexesoCard card = newCard.GetComponent<PexesoCard>();
            //newCard.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //newCard.transform.parent = playGround;

            card.ID = indexes[i];
            //Debug.Log("Indexes " + card.ID);
            card.FrontSide = cardSpriteList[card.ID];   // 1,2,3....
            card.BackSide = cardSpriteList[0]; // P0;
            cardButtonList.Add(newCard);
            numberOfHiddenButtons++;
            //hiddenBottonList.Add(newCard);
        }
    }

    public IEnumerator CompareCards() {
        if (card1 == 0 || card2 == 0) {
            yield break;
        }
        yield return new WaitForSeconds(2.0f);
        if (card1 != card2) {
            Debug.Log("zle ");
            allCardsBack();
            ScoreManager.instance.updateNextPlayer();
            ScoreManager.instance.showPlayer();
        } else {
            Debug.Log("dobre ");
            foreach (GameObject b in choosenButtonList)
            {
                Instantiate(particle, b.transform.position + new Vector3(0, 0, -1), Quaternion.identity);
            }
            ScoreManager.instance.resetCountdown();
            ScoreManager.instance.updateScore();
            ScoreManager.instance.showScore();
            // stale ide ten, co bol na tahu...
            numberOfHiddenButtons -= 2;
            if (numberOfHiddenButtons == 0)
            {
                Debug.Log("WIN ");
            }
        }
        card1 = 0;
        card2 = 0;
        choosenButtonList.Clear();
    }

    private void win()
    {
        StopAllCoroutines();
    }

    private void allCardsBack()
    {
        foreach(GameObject card in choosenButtonList)
        {
            card.GetComponent<PexesoCard>().openCloseCard(false);
        }
    }
    // Awake() sa zavola pred Start()
    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        // get args
        sizeX = MySceneManager.getSizeX();
        sizeY = MySceneManager.getSizeY();
        Debug.Log("size " + sizeX);

        // resize playGround
        float width = playGround.GetComponent<RectTransform>().rect.width;
        Vector2 newSize = new Vector2(width / (sizeX), width / (sizeY));
        playGround.GetComponent<UnityEngine.UI.GridLayoutGroup>().cellSize = newSize;

        // init
        initPlayground();
    }
    public void BackToLevelsBtnPressed()
    {
        Debug.Log("Back to Levels");
        SceneManager.LoadScene("Levels");
    }
}
