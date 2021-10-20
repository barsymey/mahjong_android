using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    public GameObject[] lvlPageBase;
    private int currentPage = 0;
    private void Start()
    {
        NextPage();
    }

    void HidePages()
    {
        foreach (var page in lvlPageBase)
        {
            page.SetActive(false);
        }
    }

    

    public void NextPage()
    {
        HidePages();
        lvlPageBase[currentPage].SetActive(true);
        currentPage++;
        if (currentPage >= lvlPageBase.Length)
        {
            currentPage = 0;
        }
    }
}
