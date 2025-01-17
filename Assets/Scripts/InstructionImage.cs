using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionImage : MonoBehaviour
{
    [SerializeField] private Image image;
    public static InstructionImage instance;
    [SerializeField] private List<Sprite> spritesLv1;
    [SerializeField] private List<Sprite> spritesLv2;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void ChangeImage(string type)
    {
        if(type == "burger_full")
        {
            Debug.Log("full");
            image.sprite = spritesLv1[0];
        }
        else if (type == "burger_noPatty")
        {
            Debug.Log("patty");
            image.sprite = spritesLv1[1];
        }
        else if (type == "burger_noLettuce")
        {
            Debug.Log("lettuce");
            image.sprite = spritesLv1[2];
        }
        else if (type == "burger_noCheese")
        {
            Debug.Log("cheese");
            image.sprite = spritesLv1[3];
        }
        else if (type == "burger_noTomato")
        {
            Debug.Log("tomato");
            image.sprite = spritesLv1[4];
        }
        else if(type.StartsWith("potatoChips")) 
        {
            Debug.Log("potato");
            image.sprite = spritesLv1[5];
        }
        else if (type.StartsWith("salad"))
        {
            Debug.Log("porkLeg");
            image.sprite = spritesLv2[0];
        }
        else if (type.StartsWith("friedRice"))
        {
            Debug.Log("friedRice");
            image.sprite = spritesLv2[1];
        }
        else if (type.StartsWith("porkLeg"))
        {
            Debug.Log("porkLeg");
            image.sprite = spritesLv2[2];
        }
        else if (type.StartsWith("beefsteak"))
        {
            Debug.Log("beefsteak");
            image.sprite = spritesLv2[3];
        }
        else
        {
            Debug.Log($"khong co {type}");
        }
    }
    
}
