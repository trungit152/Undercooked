using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Food : MonoBehaviour
{
    [SerializeField] private GameObject saladPrefab;
    [SerializeField] private GameObject beefsteakPrefab;
    public static Level2Food instance;

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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MakeSalad(Transform transform)
    {
        GameObject salad = Instantiate(saladPrefab);
        salad.transform.position = Vector3.zero;
        salad.transform.SetParent(transform, false);
        salad.transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.z);
    }
    public void MakeBeefsteak(Transform transform)
    {
        GameObject beefsteak = Instantiate(beefsteakPrefab);
        beefsteak.transform.position = Vector3.zero;
        beefsteak.transform.SetParent(transform, false);
        beefsteak.transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.z);
    }

}
