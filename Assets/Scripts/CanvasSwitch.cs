using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanvasSwitch : MonoBehaviour
{
    public GameObject HUD;
    public GameObject RaceManagerMenu;
    public GameObject SettingsMenu;

    void Start()
    {
        HUD.SetActive(true);
        RaceManagerMenu.SetActive(false);
        SettingsMenu.SetActive(false);
    }

    public void EnableRaceManager()
    {
        HUD.SetActive(false);
        RaceManagerMenu.SetActive(true);
        SettingsMenu.SetActive(false); 
    }
    
    public void EnableSettings()
    {
        HUD.SetActive(false);
        RaceManagerMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }
    
    public void EnableHUD()
    {
        HUD.SetActive(true);
        RaceManagerMenu.SetActive(false);
        SettingsMenu.SetActive(false); 
    }
}
    
    
