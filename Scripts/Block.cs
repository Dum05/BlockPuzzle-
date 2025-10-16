using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public const int Size = 5;
    private readonly Vector3 inputOffset = new(0.0f, 2.0f, 0.0f);
    [SerializeField] private Board board;
    [SerializeField] private Blocks blocks;
    [SerializeField] private Cell cellPrefab;
    private int polygonIndex;
    
    private readonly Cell[,] cells = new Cell[Size, Size];
    private Vector3 position;
    private Vector3 scale;
    private Vector2 inputPoint;
    private Vector3 previousMousePosition = Vector3.positiveInfinity;
    private Vector2Int previousDragPoin;
    private Vector2Int currentDragPoin;
    
    private Camera mainCamera;
    private Vector2 center;
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    public void Initialize()
    {
        for(var r = 0; r < Size; r++)
        {
            for (var c = 0; c < Size; c++)
            {
                cells[r, c] = Instantiate(cellPrefab, transform);
            }
        }
        position = transform.localPosition;
        scale = transform.localScale;
    }

    public void Show(int polygonIndex)
    {
        this.polygonIndex = polygonIndex;
        Hide();
        var polygon = Polygons.Get(polygonIndex);
        var polygonRows = polygon.GetLength(0);
        var polygonColumns = polygon.GetLength(1);
        center = new Vector2(polygonColumns / 2f, polygonRows / 2f);
        
        for (var r = 0; r < polygonRows; r++)
        {
            for (var c = 0; c < polygonColumns; c++)
            {
                if (polygon[r, c] > 0)
                {
                    cells[r, c].transform.localPosition = new(c - center.x + 0.5f, r - center.y + 0.5f, 0.0f);
                    cells[r, c].Normal();
                }
            }
        }
    }

    private void Hide()
    {
        for(var r = 0; r < Size; r++)
        {
            for (var c = 0; c < Size; c++)
            {
                cells[r, c].Hide();
            }
        }
    }

    private void OnMouseDown()
    {
        inputPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.localPosition = position + inputOffset;
        transform.localScale = Vector3.one;
        currentDragPoin = Vector2Int.RoundToInt((Vector2)transform.position - center);
        previousDragPoin = currentDragPoin;
        previousMousePosition = Input.mousePosition;
    }
    private void OnMouseDrag()
    {
        var currentMousePosition = Input.mousePosition;
        if (currentMousePosition != previousMousePosition)
        {
            previousMousePosition = currentMousePosition;
            var inputDelta = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition) - inputPoint;
            transform.localPosition = position + inputOffset + (Vector3)inputDelta * 1.5f; 
            currentDragPoin = Vector2Int.RoundToInt((Vector2)transform.position - center);
            if (currentDragPoin != previousDragPoin)
            {
                previousDragPoin = currentDragPoin;
            }
        }
        
    }private void OnMouseUp()
         {
             previousMousePosition = Vector3.positiveInfinity;
             currentDragPoin = Vector2Int.RoundToInt((Vector2)transform.position - center);
             if (board.Place(currentDragPoin, polygonIndex) == true)
             {
                 gameObject.SetActive(false);
                 blocks.Remove();
             }
             transform.localPosition = position;
             transform.localScale = scale;
         }
    
}
