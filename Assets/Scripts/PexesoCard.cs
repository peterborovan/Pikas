using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PexesoCard : MonoBehaviour {
    public int ID;
    public Sprite FrontSide;
    public Sprite BackSide;
    //private bool hidden = true;
    private Button button;
    private Image image;
    private float flipAmount = 1;
    public float flipSpeed = 5;

    // Use this for initialization
    void Start () {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
	}
	public void ClickOnCard()
    {
        if (CardManager.instance.card1 == 0)
        {
            CardManager.instance.card1 = ID;
            CardManager.instance.choosenButtonList.Add(this.gameObject);
            openCloseCard(true);
            button.interactable = false;
        }
        else if (CardManager.instance.card2 == 0)
        {
            CardManager.instance.card2 = ID;
            CardManager.instance.choosenButtonList.Add(this.gameObject);
            openCloseCard(true);
            button.interactable = false;
            StartCoroutine(CardManager.instance.CompareCards());
        }

    }
    public void openCloseCard(bool open)
    {
        StartCoroutine(flipCard(open));
        //hidden = !open;
        button.interactable = !open;
    }
    private IEnumerator flipCard(bool open)
    {
        while (flipAmount > 0)
        {
            //Debug.Log("flip down " + flipAmount);
            flipAmount -= Time.deltaTime * flipSpeed;
            flipAmount = Mathf.Clamp01(flipAmount);
            transform.localScale = new Vector3(flipAmount, flipAmount, transform.localScale.z);
            yield return null;
        }
        if (open)
        {
            image.sprite = FrontSide;
        } else
        {
            image.sprite = BackSide;
        }
        while (flipAmount < 1)
        {
            //Debug.Log("flip up " + flipAmount);
            flipAmount += Time.deltaTime * flipSpeed;
            flipAmount = Mathf.Clamp01(flipAmount);
            transform.localScale = new Vector3(flipAmount, flipAmount, transform.localScale.z);
            yield return null;
        }
        //button.interactable = true;
    }
    // Update is called once per frame
    void Update () {
		
	}
}
