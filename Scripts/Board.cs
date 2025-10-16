using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public const int Size = 8;
    private const string bestScoreKey = "best_score";
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform cellsTransform;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    
    private readonly Cell[,] cells = new Cell[Size, Size];
    private readonly int[,] data = new int[Size, Size];
    private readonly List<Vector2Int> hoverPoints = new();
    private readonly List<int> fullLineRows = new();
    private readonly List<int> fullLineColumns = new();
    private int score;
    private int bestScore;
    private void Start()
    {
        for (var r = 0; r < Size; r++)
        {
            for (var c = 0; c < Size; c++)
            {
                cells[r, c] = Instantiate(cellPrefab, cellsTransform);
                cells[r, c].transform.position = new(c + 0.5f, r + 0.5f, 0.0f);
                cells[r, c].Hide();
            }
        }
        score = 0;
        bestScore = PlayerPrefs.GetInt(bestScoreKey, 0);
        scoreText.text = score.ToString();
        bestScoreText.text = bestScore.ToString();
    }

    private void HoverPoints(Vector2Int point, int polygonRows, int polygonColumns, int[,] polygon)
    {
        for (var r = 0; r < polygonRows; r++)
        {
            for (var c = 0; c < polygonColumns; c++)
            {
                if (polygon[r, c] > 0)
                {
                    var hoverPoint = point + new Vector2Int(c, r);
                    if (IsValidPoint(hoverPoint) == false)
                    {
                        hoverPoints.Clear();
                        return;
                    }
                    hoverPoints.Add(hoverPoint);
                }
            }
        }
    }

    private bool IsValidPoint(Vector2Int point)
    {
        if(point.x < 0 || point.x >= Size || point.y < 0 || point.y >= Size) return false;
        if(data[point.y, point.x] > 0) return false;
        return true;
    }
    
    private void Unhover()
    {
        hoverPoints.Clear();
    }
    
    public bool Place(Vector2Int point, int polygonIndex)
    {
        var polygon = Polygons.Get(polygonIndex);
        var polygonRows = polygon.GetLength(0);
        var polygonColumns = polygon.GetLength(1);
        Unhover();
        HoverPoints(point, polygonRows, polygonColumns, polygon);
        if (hoverPoints.Count > 0)
        {
            PlaceBlocks(point, polygonColumns, polygonRows);
            return true;
        }
        return false;
    }

    private void PlaceBlocks(Vector2Int point, int polygonColumns, int polygonRows)
    {
        foreach (var hoverPoint in hoverPoints)
        {
            data[hoverPoint.y, hoverPoint.x] = 2;
            cells[hoverPoint.y, hoverPoint.x].Normal();
        }

        ClearFullLines(point, polygonColumns, polygonRows);
        hoverPoints.Clear();
    }

    private void ClearFullLines(Vector2Int point, int polygonColumns, int polygonRows)
    {
        FullLineColumns(point.x, point.x + polygonColumns);
        FullLineRows(point.y, point.y + polygonRows);
        AddScore(fullLineRows.Count * Size + fullLineColumns.Count * Size);
        ClearFullLineColumns();
        ClearFullLineRows();
    }

    private void FullLineColumns(int fromColumn, int toColumnExclusive)
    {
        fullLineColumns.Clear();
        var finalColExclusive = toColumnExclusive;
        if (finalColExclusive > Size)
        {
            finalColExclusive = Size;
        }
    
        if (fromColumn < 0) fromColumn = 0;
        if (fromColumn >= Size) return;
        for (var c = fromColumn; c < finalColExclusive; c++)
        {
            var isFullLine = true;
            for (var r = 0; r < Size; r++)
            {
                if (data[r, c] != 2)
                {
                    isFullLine = false;
                    break;
                }
            }

            if (isFullLine == true)
            {
                fullLineColumns.Add(c);
            }
        }
    }
    
    private void FullLineRows(int fromRow, int toRowExclusive)
    {
        fullLineRows.Clear();
        var finalRowExclusive = toRowExclusive;
        if (finalRowExclusive > Size)
        {
            finalRowExclusive = Size;
        }
        
        if (fromRow < 0) fromRow = 0;
        if (fromRow >= Size) return;
        for (var r = fromRow; r < finalRowExclusive; r++)
        {
            var isFullLine = true;
            for (var c = 0; c < Size; c++)
            {
                if (data[r, c] != 2)
                {
                    isFullLine = false;
                    break;
                }
            }

            if (isFullLine == true)
            {
                fullLineRows.Add(r);
            }
        }
    }

    private void ClearFullLineColumns()
    {
        foreach (var c in fullLineColumns)
        {
            for (var r = 0; r < Size; r++)
            {
                data[r, c] = 0;
                cells[r, c].Hide();
            }
        }
    }
    
    private void ClearFullLineRows()
    {
        foreach (var r in fullLineRows)
        {
            for (var c = 0; c < Size; c++)
            {
                data[r, c] = 0;
                cells[r, c].Hide();
            }
        }
    }

    public bool CheckPlace(int polygonIndex)
    {
        var polygon = Polygons.Get(polygonIndex);
        var polygonRows = polygon.GetLength(0);
        var polygonColumns = polygon.GetLength(1);
        for (var r = 0; r <= Size - polygonRows; r++)
        {
            for (var c = 0; c <= Size - polygonColumns; c++)
            {
                if (Check(c, r, polygonColumns, polygonRows, polygon) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool Check(int column, int row, int polygonColumns, int polygonRows, int[,] polygon)
    {
        for (var r = 0; r < polygonRows; r++)
        {
            for (var c = 0; c < polygonColumns; c++)
            {
                if (polygon[r, c] > 0 && data[row + r, column + c] == 2)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt(bestScoreKey, bestScore);
        }
        scoreText.text = score.ToString();
        bestScoreText.text = bestScore.ToString();
    }
}
