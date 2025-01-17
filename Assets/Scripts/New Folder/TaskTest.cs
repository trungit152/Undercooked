using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskTest : MonoBehaviour
{
    public int id;
    public string taskName;
    public Image img;

    public void Init(int id, string name, Sprite sprite)
    {
        this.id = id;
        this.taskName = name;
        img.sprite = sprite;
    }
}
