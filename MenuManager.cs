using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private GameObject[] menuBase;
    public GameObject initMenu;
    private GameObject prevMenu;
    private GameObject currMenu;
    public GameObject winMenu;
    public GameObject winAnim;
    public Slider musicVolume;
    public Slider soundVolume;
    public AudioSource musicSource;
    public AudioSource soundSource;

    private void Start()
    {
        menuBase = GameObject.FindGameObjectsWithTag("Menu");
        OpenMenu(initMenu);
    }

    void HideMenus()
    {
        foreach (var menu in menuBase)
        {
            menu.SetActive(false);
        }
    }
    
    public void OpenMenu(GameObject menu)
    {
        prevMenu = currMenu;
        HideMenus();
        menu.SetActive(true);
        currMenu = menu;
    }

    public void PreviousMenu()
    {
        OpenMenu(prevMenu);
    }
    
    public void SetMusicVol()
    {
        musicSource.volume = musicVolume.value;
    }
    
    public void SetSoundVol()
    {
        soundSource.volume = soundVolume.value;
    }
}
