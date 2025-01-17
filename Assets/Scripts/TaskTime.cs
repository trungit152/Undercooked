using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TaskTime : MonoBehaviour
{
    public float time;
    private float maxTime;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject warning;
    [SerializeField] private  Image taskImage;
    public int id;
    public Dictionary<string, Sprite> order;
    public string name;
    private void Awake()
    {
        if (warning != null) warning.SetActive(false);
        if (slider != null) slider.gameObject.SetActive(true);
        order = new Dictionary<string, Sprite>();
    }
    private void Update()
    {
        time -= Time.deltaTime;
        if (time <= 7)
        {
            if (warning != null)
            {
                warning.SetActive(true);
            }
        }
        else
        {
            warning?.SetActive(false);
        }
        UpdateSlider();
        if (time <= 0)
        {
            TaskController.instance.RemoveOrderTimeOut(order);
            ClearOrder();
            gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(gameObject + ": " + order.Keys.First());
        }

    }

    public bool IsOverTime()
    {
        return time <= 0;
    }

    private void OnEnable()
    {
        time = Random.Range(45, 70);
        maxTime = time;
    }

    private void UpdateSlider()
    {
        slider.value = (maxTime - time) / maxTime;
    }
    public void AddOrder(Dictionary<string, Sprite> order)
    {
        this.order = order;
        name = order.Keys.First();
        taskImage.gameObject.SetActive(true);
        taskImage.enabled = true;
        taskImage.sprite = order.Values.First();
    }
    public void ClearOrder()
    {
        this.order.Clear();
        this.name = null;
    }
}
