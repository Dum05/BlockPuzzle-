using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Blocks : MonoBehaviour
{
    [SerializeField] private Block[] blocks;
    [SerializeField] Board board;
    [SerializeField] private GameObject loseGameObject;
    
    private int[] polygonIndex;
    private int blockCount = 0;
    private void Start()
    {
        var blockWidth = (float)Board.Size / blocks.Length;
        var cellSize = (float)Board.Size / (Block.Size * blocks.Length + blocks.Length + 1);
        for (var i = 0; i < blocks.Length; i++)
        {
            blocks[i].transform.localPosition = new(blockWidth * (i + 0.5f), - 0.25f - cellSize * 4.0f, 0.0f);
            blocks[i].transform.localScale = new(cellSize, cellSize, cellSize);
            blocks[i].Initialize();
        }
        polygonIndex = new int[blocks.Length];
        Generate();
    }

    private void Generate()
    {
        blockCount = 0;
        for (var i = 0; i < blocks.Length; i++)
        {
            polygonIndex[i] = Random.Range(0, Polygons.Length);
            blocks[i].gameObject.SetActive(true);
            blocks[i].Show(polygonIndex[i]);
            blockCount++;
        }
    }
    
    public void Remove()
    {
        blockCount--;
        
        if (blockCount > 0)
        {
            bool canPlace = false;
            for (var i = 0; i < blocks.Length; i++)
            {
                if (blocks[i].gameObject.activeSelf == true && board.CheckPlace(polygonIndex[i]) == true)
                {
                    canPlace = true;
                    break;
                }
            }

            if (canPlace == false)
            {
                Lose();
            }
        }
        else
        {
            blockCount = 0;
            Generate();
            CheckLoseAfterGenerate();
        }
    }

    private void CheckLoseAfterGenerate()
    {
        bool canPlace = false;
        for (var i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].gameObject.activeSelf == true && board.CheckPlace(polygonIndex[i]) == true)
            {
                canPlace = true;
                break;
            }
        }

        if (canPlace == false)
        {
            Lose();
        }
    }

    private void Lose()
    {
        loseGameObject.SetActive(true);
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
