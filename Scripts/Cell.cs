using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Sprite normar;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Normal()
    {
        gameObject.SetActive(true);
        spriteRenderer.color = Color.white;
        spriteRenderer.sprite = normar;
    }

    public void Hover()
    {
        gameObject.SetActive(true);
        spriteRenderer.color = new(1.0f, 1.0f, 1.0f, 0.5f);
        spriteRenderer.sprite = normar;
    }

    public void Hide()
    {
        gameObject.SetActive(false);   
    }
}
