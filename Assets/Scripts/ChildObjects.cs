using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildObjects : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer parentSpriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        spriteRenderer.sortingOrder = parentSpriteRenderer.sortingOrder;
    }
}
