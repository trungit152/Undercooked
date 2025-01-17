using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredients : MonoBehaviour
{
    public bool isOriginal {  get; private set; }

    private void Start()
    {
        SetOriginal();
    }
    public void SetOriginal()
    {
        isOriginal = true;
    }
    public void Clone()
    {
        isOriginal = false;
    }
}
