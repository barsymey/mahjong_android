using System;
using System.Collections;
using System.IO;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class GameSpace : MonoBehaviour
{
    public int[,,] grid =new int[11,8,4];
    private int[] sack = new int[162];
    private GameObject[] blockBase;
    
    public TextAsset[] lvlFileBase;
    public int fieldL = 11;
    public int fieldH = 8;
    public int diversity;
    private int currentLevel;
    private int blockAmount;
    public int blocksLeft;

    public GameObject block;
    public GameObject blockField;

    private Block block1 = null;
    private Block block2 = null;

    private float startTime;
    
    private void Start()
    {
        Input.backButtonLeavesApp = false;
        GetComponent<Ads>().LoadAd();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.touchCount>0)
        {
                CompareBlocks();
        }
    }

    private void FillSack(int amount)
    {
        for (int x= 0; x< amount; x+=2)
        {
            sack[x] = Random.Range(1,diversity+1);
            sack[x + 1] = sack[x];
        }
        MixSack(amount);
        print("sack filled");

    }

    void MixSack(int inSack)
    {
        for (int x = 0; x < inSack; x++)
        {
            int tmp = sack[x];
            int swap = Random.Range(0, inSack);
            sack[x] = sack[swap];
            sack[swap] = tmp;
        }
        print("sack mixed");
    }

    public void InitGrid(int lev)
    {
        blockField.SetActive(true);
        blockAmount = 0;
        string lvlData = lvlFileBase[lev].text;
        int dataPointer = 0;
        for (int x = 0; x < 11; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int z = 0; z < 4; z++)
                {
                    grid[x, y, z] = 0;
                    if (lvlData[dataPointer] == '1')
                    {
                        blockAmount++;
                    }
                    dataPointer++;
                }
            }
        }
        print("field returned to zero");
        FillSack(blockAmount);
        int iteration = 0;
        dataPointer = 0;
        for (int x = 0; x < 11; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int z = 0; z < 4; z++)
                {
                    if (lvlData[dataPointer] == '1')
                    {
                        grid[x, y, z] = sack[iteration];
                        iteration++;
                    }
                    dataPointer++;
                }
            }
        }
        print("sack applied to grid");
        DrawGrid();
        GetComponent<Ads>().ShowAdInit();
    }
    
    [ContextMenu("Fill field")]
    public void InitEditorGrid()
    {
        blocksLeft = 0;
        for (int x = 0; x < 11; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int z = 0; z < 4; z++)
                {
                    grid[x, y, z] = 0;
                }
            }
        }
        print("field returned to zero");
        FillSack(162);
        int iteration = 0;
        blockAmount = 0;
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                for (int z = 0; z < 3; z++)
                {
                    grid[x+1, y+1, z] = sack[iteration];
                        iteration++;
                }
            }
        }

        print("sack applied to grid");
        DrawGrid();
    }

    private void DrawGrid()
    {
        DestroyBlocks();
        blocksLeft = 0;
        for (int x = 0; x < 11; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int z = 0; z < 4; z++)
                {
                    if (grid[x, y, z] != 0)
                    {
                        float add = 0;
                        if (y % 2 == 0)
                        {
                            add = 0.5f;
                        }
                        Vector3 pos = new Vector3(x - (fieldL+1) / 2 + z*0.05f+add, y - fieldH / 2+ z*0.07f - 0.5f, -z);
                        GameObject newBlock = Instantiate(block, pos, quaternion.identity, blockField.transform);
                        newBlock.GetComponent<Block>().type = grid[x, y, z];
                        newBlock.GetComponent<Block>().blockY = y;
                        newBlock.GetComponent<Block>().blockX = x;
                        newBlock.GetComponent<Block>().blockZ = z;
                        blocksLeft++;
                    }
                }
            }
        }

    }
    
    public Block PickBlock()
    {
        //Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        Vector3 screenDir = new Vector3(screenPoint.x, screenPoint.y, 1000f);
        Ray ray = new Ray(screenPoint,screenDir);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider == null)
        {
            return null;
        }
        if (hit.collider.GetComponent<Block>().destroyed)
        {
            return null;
        }
        return hit.collider.GetComponent<Block>();
    }
    
    void CompareBlocks()
    {
        Block pickedBlock = PickBlock();
        if (pickedBlock != null)
        {
            if (CheckClear(pickedBlock) == false)
            {
                pickedBlock.Unavailable();
                print("unavailable");
                return;
            }

            if (block1 == null)
            {
                block1 = pickedBlock;
                block1.Select();
                return;
            }

            if (block2 == null & pickedBlock != block1)
            {
                block2 = pickedBlock;
                block1.Unselect();
                if (block1.type == block2.type)
                {
                    if (CheckClear(block1) && CheckClear(block2))
                    {
                        RemoveBlock(block1);
                        RemoveBlock(block2);
                        GetComponent<SoundManager>().DestroySound();
                        Win();
                    }
                }
                block1 = null;
                block2 = null;
            }
        }
        else
        {
            if (block1 != null)
            {
                block1.Unselect();
            }
            block1 = null;
            block2 = null;
        }
    }
   
    private bool CheckClear(Block b)
    {
        if (grid[b.blockX, b.blockY, b.blockZ + 1] == 0)
        {
            if (grid[b.blockX + 1, b.blockY, b.blockZ] == 0)
            {
                return true;
            }

            if (grid[b.blockX - 1, b.blockY, b.blockZ] == 0)
            {
                return true;
            }
        }
        
        print("Block blocked");
            return false;
    }

    public void RemoveBlock(Block block)
    {
        grid[block.blockX, block.blockY, block.blockZ] = 0;
        block.MarkDestroy();
        block.destroyed = true;
        blocksLeft--;
    }

    
    [ContextMenu("InstaWin")]

    void Win()
    {
        if (blocksLeft <= 2)
        {
            blockField.SetActive(false);
            DestroyBlocks();
            GetComponent<SoundManager>().WinSound();
            GetComponent<MenuManager>().winMenu.SetActive(true);
            GetComponent<MenuManager>().winAnim.GetComponent<WinAnim>().Appear();
        } 
    }

    public void MixBlocks()
    {
        GetComponent<SoundManager>().MixSound();
        DestroyBlocks();
        int iteration = 0;
        for (int x = 0; x < 11; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int z = 0; z < 4; z++)
                {
                    if (grid[x,y,z] != 0)
                    {
                        sack[iteration] = grid[x, y, z];
                        iteration++;
                    }
                }
            }
        }
        MixSack(iteration);

        iteration = 0;
        for (int x = 0; x < 11; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int z = 0; z < 4; z++)
                {
                    if (grid[x,y,z] != 0)
                    {
                        grid[x, y,z] = sack[iteration];
                        iteration++;
                    }
  
                }
            }
        }
        GetComponent<Ads>().ShowAdMix();
        DrawGrid();
    }

    void DestroyBlocks()
    {
        blockBase = GameObject.FindGameObjectsWithTag("Block");
        foreach (var bl in blockBase)
        {
            bl.GetComponent<Block>().MarkDestroy();
        }
    }
    
    
}

