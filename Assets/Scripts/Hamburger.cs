using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamburger : MonoBehaviour
{
    public GameObject top;
    public GameObject lettuce;
    public GameObject tomato;
    public GameObject cheese;
    public GameObject patty;
    public GameObject bot;
    private int curLayerID;
    private SpriteRenderer sr;
    public string type;
    public SpriteRenderer shadow;

    private void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();

        top = transform.GetChild(0).gameObject;
        lettuce = transform.GetChild(1).gameObject;
        tomato = transform.GetChild(2).gameObject;
        cheese = transform.GetChild(3).gameObject;
        patty = transform.GetChild(4).gameObject;
        bot = transform.GetChild(5).gameObject;
        shadow = transform.GetChild(6).gameObject.GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        curLayerID = sr.sortingOrder;

        top.SetActive(true);
        lettuce.SetActive(false);
        tomato.SetActive(false);
        cheese.SetActive(false);
        patty.SetActive(false);
        bot.SetActive(true);

        UpdateOrderLayerID();
    }

    private void Update()
    {
        if(curLayerID != sr.sortingOrder)
        {
            UpdateOrderLayerID();
            curLayerID=sr.sortingOrder;
        }
    }
    public string GetHamburgerName()
    {
        if(top.activeSelf && lettuce.activeSelf && tomato.activeSelf && cheese.activeSelf && patty.activeSelf && bot.activeSelf)
        {
            return "burger_full";
        }
        else if (top.activeSelf && !lettuce.activeSelf && tomato.activeSelf && cheese.activeSelf && patty.activeSelf && bot.activeSelf)
        {
            return "burger_noLettuce";
        }
        else if (top.activeSelf && lettuce.activeSelf && !tomato.activeSelf && cheese.activeSelf && patty.activeSelf && bot.activeSelf)
        {
            return "burger_noTomato";
        }
        else if (top.activeSelf && lettuce.activeSelf && tomato.activeSelf && !cheese.activeSelf && patty.activeSelf && bot.activeSelf)
        {
            return "burger_noCheese";
        }
        else if (top.activeSelf && lettuce.activeSelf && tomato.activeSelf && cheese.activeSelf && !patty.activeSelf && bot.activeSelf)
        {
            return "burger_noPatty";
        }
        else
        {
            return "unfinished";
        }
    } 
    public void UpdateOrderLayerID()
    {
        patty.GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder + 1;
        cheese.GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder + 2;
        tomato.GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder + 3;
        lettuce.GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder + 4;
        top.GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder + 5;
        shadow.sortingOrder = sr.sortingOrder - 1;
    }
}
