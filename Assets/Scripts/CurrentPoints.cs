using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = System.Random;

public class CurrentPoints : MonoBehaviour
{
    public int points = 0;
    public TextMeshProUGUI pointsGUIHUD;
    public TextMeshProUGUI pointsGUIRaceManager;
    private float period = 0.0f;
    public int CyberSecurityScore = 1;
    
    private void Update()
    {
        pointsGUIHUD.text = points.ToString();
        pointsGUIRaceManager.text = points.ToString();
        if (period > 2)
        {
            addPoints();
            period = 0;
        }
        period += UnityEngine.Time.deltaTime;
    }
    
    public void addPoints()
    {
        points += 1 * CyberSecurityScore;
    }
}
