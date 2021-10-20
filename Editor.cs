using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Editor : MonoBehaviour
{
    public bool editorMode;
    public GameSpace gamespace;
    public int lvlNum;
        
    void Start()
    {
        gamespace = GetComponent<GameSpace>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (editorMode)
            {
                RemoveBlock(PickBlock());
            }
        }
    }

    public Block PickBlock()
    {
        Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 screenDir = new Vector3(screenPoint.x, screenPoint.y, 1000f);
        Ray ray = new Ray(screenPoint,screenDir);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider == null)
        {
            print("no hit");
            return null;
        }
        print("got hit");
        return hit.collider.GetComponent<Block>();
    }

    private void RemoveBlock(Block block)
    {
        gamespace.grid[block.blockX, block.blockY, block.blockZ] = 0;
        Destroy(block.gameObject);
        gamespace.blocksLeft--;
    }
    
    [ContextMenu("Save template")]
    public void Save()
    {
        string data = ("");
        string path = "Assets/Levels/" + lvlNum + ".txt";
        StreamWriter writer = new StreamWriter(path,true);
         for (int x = 0; x < 11; x++)
         {
             for (int y = 0; y<8; y++)
             {
                 for (int z = 0; z < 4; z++)
                 {
                     if (gamespace.grid[x, y, z] == 0)
                     {
                         data = (data + 0);
                     }
                     else
                     {
                         data = (data + 1);
                     }
                 }
             }
         }
         writer.WriteLine(data);
         writer.Close();
    }
}
