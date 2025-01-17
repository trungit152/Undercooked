using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskInstructor : MonoBehaviour
{
    [SerializeField] private GameObject instructorPanel;
    [SerializeField] private GameObject blurPanel;
    [SerializeField] private TaskTime taskTime;
    private string type;
    
    private void Start()
    {
        if (blurPanel.activeSelf)
        {
            blurPanel.SetActive(false);
            instructorPanel.SetActive(false);
        }
    }
    public void ShowInstruction()
    {
        blurPanel.SetActive(true);
        if (InstructionImage.instance != null)
        {
            InstructionImage.instance.ChangeImage(type);
        }
        FeelingTool.instance.ZoomIn(instructorPanel.transform);
    }
    public void CloseInstruction()
    {
        blurPanel.SetActive(false);
        FeelingTool.instance.ZoomOut(instructorPanel.transform);
    }
    public void SetType(string type_ = null)
    {
        Debug.Log(taskTime.name);   
        if (type_ != null)
        {
            type = type_;
        }
        else
        {
            type = taskTime.name;
        }
    }
}
