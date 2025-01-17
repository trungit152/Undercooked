using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Cookable : Ingredients
{
    private float cookTime = 0f;
    [SerializeField] private float cookDoneTime = 5f;
    [SerializeField] private float burntTime = 7f;
    [SerializeField] private Slider processBar;
    [SerializeField] private Slider processBar2;
    [SerializeField] private Sprite doneForm;
    [SerializeField] private Sprite burntForm;
    [SerializeField] private SpriteRenderer shadow;
    private SpriteRenderer spriteRenderer;
    private bool isCooking;
    private bool isBurnting;
    private SoundController soundController;
    private bool isTurnOnSound;
    private enum state
    {
        rare,
        done,
        burnt
    }
    private state curState;
    private void Awake()
    {
        soundController = GameObject.FindGameObjectWithTag("audio").GetComponent<SoundController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetStartValue();
    }
    private void Update()
    {
        StartCook();
    }

    private void SetStartValue()
    {
        isBurnting = false;
        isCooking = false;
        curState = state.rare;
        cookTime = 0;
        processBar.value = cookTime / cookDoneTime;
        isTurnOnSound = false;

        SetOriginal();
    }
    private void UpdatePrcessBar()
    {
        if (processBar != null && processBar.value < 1)
        {
            processBar.value = cookTime / cookDoneTime;
        }
    }
    private void UpdatePrcessBar2()
    {
        if (processBar2 != null && processBar2.value < 1)
        {
            processBar2.value = cookTime / burntTime;
        }
    }
    private void StartCook()
    {
        if (isCooking)
        {
            if (!isTurnOnSound)
            {
                isTurnOnSound = true;
                soundController.PlaySFX(soundController.fry);
            }
            if (cookTime >= cookDoneTime)
            {
                spriteRenderer.sprite = doneForm;
                shadow.sprite = doneForm;
                gameObject.name += "Done";
                if (gameObject.name.StartsWith("potato"))
                {
                    gameObject.name = "potatoChips";
                }
                TurnOffProcessBar();
                TurnOnProcessBar2();
                curState = state.done;
                isCooking = false;
                cookTime = 0f;
                isBurnting = true;
            }
            else
            {
                cookTime += Time.deltaTime;
                UpdatePrcessBar();
            }
        }
        else if(isBurnting)
        {
            if (!isTurnOnSound)
            {
                isTurnOnSound = true;
                soundController.PlaySFX(soundController.fry);
            }
            if (cookTime >= burntTime)
            {
                isBurnting = false;
                TurnOffProcessBar2();
                curState = state.burnt;
                spriteRenderer.sprite = burntForm;
                gameObject.name += "Burnt";
            }
            cookTime += Time.deltaTime;
            UpdatePrcessBar2();
        }
        else
        {
            if (isTurnOnSound)
            {
                isTurnOnSound = false;
                soundController.StopSFX();
            }
        }
    }
    public void TurnOnProcessBar()
    {
        processBar.gameObject.SetActive(true);
        spriteRenderer.sortingOrder += 1;
    }
    public void TurnOffProcessBar()
    {
        isCooking = false;
        processBar.gameObject.SetActive(false);
    }

    public void TurnOffProcessBar2()
    {
        isBurnting = false;
        processBar2.gameObject.SetActive(false);
    }

    public void TurnOnProcessBar2()
    {
        processBar2.gameObject.SetActive(true);
    }
    public void Cook()
    {
        if(curState == state.rare)
        {
            isCooking = true;
            TurnOnProcessBar();
        }
        else if(curState == state.done)
        {
            isBurnting=true;
            TurnOnProcessBar2();
        }
    }
}
