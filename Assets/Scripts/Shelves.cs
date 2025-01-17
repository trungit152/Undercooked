using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelves : MonoBehaviour
{
   public enum type{
        free,
        knife,
        food,
        knifeAndFood,
        stove,
        plate,
        bin,
        sold
    }
    public type curType;

    private void Start()
    {
        CheckShelfType();
    }
    public void Free()
    {
        curType = type.free;
    }
    public void CheckShelfType()
    {
        if (transform.childCount == 0)
        {
            curType = type.free;
            gameObject.tag = "Untagged";
        }
        else if (transform.childCount == 1 && transform.GetChild(0).gameObject.CompareTag("food"))
        {
            curType = type.food;
            transform.GetChild(0).gameObject.transform.position = transform.position;
            gameObject.tag = "food";
        }
        else if (transform.childCount == 1 && transform.GetChild(0).gameObject.CompareTag("knife"))
        {
            curType = type.knife;
            transform.GetChild(0).gameObject.transform.position = transform.position;
            gameObject.tag = "knife";
        }
        else if (transform.GetChild(0).gameObject.CompareTag("plate"))
        {
            curType = type.plate;
            transform.GetChild(0).gameObject.transform.position = transform.position;
            gameObject.tag = "plate";
        }
        else if (transform.childCount == 2 && transform.GetChild(0).gameObject.CompareTag("knife") && transform.GetChild(1).gameObject.CompareTag("food"))
        {
            curType = type.knifeAndFood;
            transform.GetChild(0).gameObject.transform.position = transform.position;
            transform.GetChild(1).gameObject.transform.position = transform.position;
            gameObject.tag = "knife";
        }
        else if (transform.childCount == 1 && transform.GetChild(0).gameObject.CompareTag("stove"))
        {
            curType = type.stove;
            transform.GetChild(0).gameObject.transform.position = transform.position;
            gameObject.tag = "stove";
        }
        else if (transform.childCount == 1 && transform.GetChild(0).gameObject.CompareTag("bin"))
        {
            curType = type.bin;
            transform.GetChild(0).gameObject.transform.position = transform.position;
            gameObject.tag = "bin";
        }
        else if (transform.childCount == 1 && transform.GetChild(0).gameObject.CompareTag("sold"))
        {
            curType = type.sold;
            transform.GetChild(0).gameObject.transform.position = transform.position;
            gameObject.tag = "sold";
        }
    }   
}
