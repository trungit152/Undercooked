using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sliceable : Ingredients
{
    [SerializeField] private float health = 5, maxHealth = 5;
    [SerializeField] private Slider slider;
    [SerializeField] private Sprite slicedForm;
    [SerializeField] private SpriteRenderer shadowSprite;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        TurnOffHealthBar();
        UpdateHealthBar();
        SetOriginal();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log(gameObject.name + ": " + gameObject.GetComponent<Ingredients>().isOriginal);
        }
    }
    private void UpdateHealthBar()
    {
        slider.value = health / maxHealth;
    }
    public void Slice()
    {
        health -= 1;
        UpdateHealthBar();
        SliceDone();
    }
    public void TurnOnHealthBar()
    {
        slider.gameObject.SetActive(true);
    }
    public void TurnOffHealthBar()
    {
        slider.gameObject.SetActive(false);
    }
    private void SliceDone()
    {
        if(health == 0)
        {
            slider.gameObject.SetActive(false);
            spriteRenderer.sprite = slicedForm;
            shadowSprite.sprite = slicedForm;
            gameObject.name = gameObject.name + "Sliced";

            //CheckForPotato
            if (gameObject.name.StartsWith("potato") && gameObject.name.EndsWith("Sliced"))
            {
                gameObject.GetComponent<Sliceable>().enabled = false;
                gameObject.GetComponent<Cookable>().enabled = true;
            }
        }
    }
}
