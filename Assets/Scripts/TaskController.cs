using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TaskController : MonoBehaviour
{
    [SerializeField] private List<Sprite> taskImages;
    [SerializeField] private List<Sprite> instructorImages;
    [SerializeField] private List<Image> uiTaskImages;
    [SerializeField] private List<GameObject> images;
    [SerializeField] private List<Sprite> instructionImages;
    [SerializeField] private Sprite burger_full_sprite;
    [SerializeField] private Sprite burger_noPatty_sprite;
    [SerializeField] private Sprite burger_noLettuce_sprite;
    [SerializeField] private Sprite burger_noCheese_sprite;
    [SerializeField] private Sprite burger_noTomato_sprite;
    [SerializeField] private Sprite potatoChips_sprite;
    [SerializeField] private Sprite salad_sprite;
    [SerializeField] private Sprite friedRice_sprite;
    [SerializeField] private Sprite porkLeg_sprite;
    [SerializeField] private Sprite beefsteak_sprite;
    [SerializeField] private List<TaskTime> taskTimes;

    private List<string> orderName;

    public List<float> time;
    public List<float> maxTime;
    private List<string> taskType;
    private List<TaskInstructor> taskInstructors;
    private float timeToNextTask;
    private Dictionary<string, Sprite> orderDictionary;
    private List<Dictionary<string, Sprite>> orderList;
    public static TaskController instance;

    private void Awake()
    {
        orderDictionary = new Dictionary<string, Sprite>();
        taskType = new List<string>();
        taskInstructors = new List<TaskInstructor>();
        orderList = new List<Dictionary<string, Sprite>>();
        time = new List<float>();
        maxTime = new List<float>();

        orderName = new List<string>();

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        foreach (var image in images)
        {
            image.SetActive(false);
        }
    }
    private void Start()
    {
        TurnOffTask();
        SetStartValue();
        SetDictionary();
    }

    private void Update()
    {
        SpawnTask();
        if (Input.GetKeyDown(KeyCode.B))
        {
            foreach (var order in orderList)
            {
                Debug.Log(order.Keys.First() + ": " + order.Values.First() + "\n");
            };
        }
    }

    List<TaskTime> listTask;
    private void SetStartValue()
    {
        timeToNextTask = 5f;
    }
    private void SetDictionary()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "level1")
        {
            orderDictionary.Add("burger_full", burger_full_sprite);
            orderDictionary.Add("burger_noLettuce", burger_noLettuce_sprite);
            orderDictionary.Add("burger_noCheese", burger_noCheese_sprite);
            orderDictionary.Add("burger_noTomato", burger_noTomato_sprite);
            orderDictionary.Add("potatoChips", potatoChips_sprite);
        }
        else if (SceneManager.GetActiveScene().name == "level2")
        {
            orderDictionary.Add("salad", salad_sprite);
            orderDictionary.Add("friedRice", friedRice_sprite);
            orderDictionary.Add("porkLeg", porkLeg_sprite);
            orderDictionary.Add("beefsteak", beefsteak_sprite);
        }
    }
    private void SpawnTask()
    {
        if (timeToNextTask > 0)
        {
            timeToNextTask -= Time.deltaTime;
        }
        else
        {
            if (orderList.Count < 6)
            {
                //SpawnTask
                var randomOrder = orderDictionary.ElementAt(UnityEngine.Random.Range(0, orderDictionary.Count));
                var newOrder = new Dictionary<string, Sprite> { { randomOrder.Key, randomOrder.Value } };
                orderList.Add(newOrder);
                orderName.Add(newOrder.Keys.First());

                int j = 0;
                for (int i = 0; i < taskTimes.Count; i++)
                {
                    if (!taskTimes[i].gameObject.activeSelf)
                    {
                        j = i;
                        break;
                    }
                }
                taskTimes[j].gameObject.SetActive(true);
                taskTimes[j].ClearOrder();
                taskTimes[j].AddOrder(newOrder);
                //taskTimes[j].gameObject.SetActive(false);

                //AddTaskImage(j);
                AddTaskInstructor();
                //reset time
                timeToNextTask = UnityEngine.Random.Range(15f, 25f);
            }
            else
            {
                timeToNextTask = 5f;
            }
        }
    }

    private void AddTaskImage(int j = -1)
    {
        if (j < 0)
        {
            taskImages.Add(orderList[orderList.Count - 1].Values.First());
            taskType.Add(orderList[orderList.Count - 1].Keys.First());
            //uiTaskImages[orderList.Count - 1].gameObject.SetActive(true);
            //images[orderList.Count - 1].gameObject.SetActive(true);
            uiTaskImages[orderList.Count - 1].enabled = true;
            uiTaskImages[orderList.Count - 1].GetComponent<Image>().sprite = taskImages[orderList.Count - 1];
            //add task instructor
        }
        else
        {
            taskImages.Add(orderList[orderList.Count - 1].Values.First());
            taskType.Add(orderList[orderList.Count - 1].Keys.First());
            uiTaskImages[j].gameObject.SetActive(true);
            //images[orderList.Count - 1].gameObject.SetActive(true);
            uiTaskImages[j].enabled = true;
            uiTaskImages[j].GetComponent<Image>().sprite = taskImages[orderList.Count - 1];
            //add task instructor
        }
    }
    private void SortTaskImage()
    {
        //taskImages.Clear();
        //taskType.Clear();
        //TurnOffTask();
        //foreach (var image in uiTaskImages)
        //{
        //    image.GetComponent<Image>().sprite = null;
        //    image.enabled = false;
        //}
        //for (int i = 0; i < orderList.Count; i++)
        //{
        //    taskImages.Add(orderList[i].Values.First());
        //    taskType.Add(orderList[i].Keys.First());
        //    images[i].gameObject.SetActive(true);
        //    uiTaskImages[i].gameObject.SetActive(true);
        //    uiTaskImages[i].enabled = true;
        //    uiTaskImages[i].GetComponent<Image>().sprite = taskImages[i];
        //}
    }
    private void TurnOffTask()
    {
        foreach (var image in uiTaskImages)
        {
            image.gameObject.SetActive(false);
        }
    }
    private void AddTaskInstructor()
    {
        for (int i = 0; i < uiTaskImages.Count; i++)
        {
            if (uiTaskImages[i].gameObject.activeSelf)
            {
                TaskInstructor taskInstructor = uiTaskImages[i].GetComponent<TaskInstructor>();
                if (taskInstructor != null)
                {
                    taskInstructor.SetType();
                }
            }
        }
    }

    public bool CheckOrderDone(string name, bool isDone = true)
    {
        bool check = false;
        foreach (var order in orderList)
        {
            Debug.Log($"name: {name}");
            Debug.Log(orderList.IndexOf(order) + ":order name: " + order.Keys.First());
            //if (order.Keys.First().StartsWith(name))
            if (name.StartsWith(order.Keys.First()))
            {
                Debug.Log("done");
                if (isDone)
                {
                    if (order.Keys.First().StartsWith("burger"))
                    {
                        GameController.instance.GetScore(30);
                    }
                    else if (order.Keys.First().StartsWith("potato"))
                    {
                        GameController.instance.GetScore(20);
                    }
                    else if (order.Keys.First().StartsWith("beefsteak"))
                    {
                        GameController.instance.GetScore(30);
                    }
                    else if (order.Keys.First().StartsWith("salad"))
                    {
                        GameController.instance.GetScore(30);
                    }
                    else if (order.Keys.First().StartsWith("porkLeg"))
                    {
                        GameController.instance.GetScore(20);
                    }
                    else if (order.Keys.First().StartsWith("friedRice"))
                    {
                        GameController.instance.GetScore(20);
                    }
                }
                RemoveOrder(order);
                RemoveTaskTime(order);
                check = true;
                break;
            }
        }
        return check;
    }
    private void RemoveOrder(Dictionary<string, Sprite> order)
    {
        if (orderList.Contains(order))
        {
            if (order.Any())
            {
                orderName.Remove(order.Keys.First());
                Debug.Log("removed");
            }
            orderList.Remove(order);
        }
    }
    private void RemoveTaskTime(Dictionary<string, Sprite> order)
    {
        TaskTime taskTime_ = null;
        float time = 100;
        foreach (var taskTime in taskTimes)
        {
            if (taskTime.order == order)
            {
                if (taskTime.time < time)
                {
                    taskTime_ = taskTime;
                    time = taskTime.time;
                }
            }
        }
        taskTime_.ClearOrder();
        taskTime_.gameObject.SetActive(false);
    }
    public void AddTime(float time_)
    {
        time.Add(time_);
    }
    public void AddMaxTime(float time_)
    {
        maxTime.Add(time_);
    }
    public void RemoveOrderTimeOut(Dictionary<string, Sprite> order)
    {
        foreach (var order_ in orderList)
        {
            if (order.Keys.Count > 0)
            {
                if (order_.Keys.First() == order.Keys.First())
                {
                    orderList.Remove(order_);

                    orderName.Remove(order_.Keys.First());
                    break;
                }
            }
        }
    }
}
