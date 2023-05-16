using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CarStatsLoad : MonoBehaviour
{
    public CarStats carStats = new CarStats();
    public TrackStats trackStats = new TrackStats();

    public int trackNr;

    public GameObject WinScreen;
    public GameObject LoseScreen;
    public GameObject TiedScreen;
    public GameObject CarSelection;
    public GameObject CloseRaceScreen;
    public Image WinAnimationImage;
    public Image LoseAnimationImage;
    public Image TiedAnimationImage;
    
    public Sprite[] WinSpriteArray;
    public Sprite[] LoseSpriteArray;
    public Sprite[] TiedSpriteArray;
    
    public float m_Speed = .02f;
    public int m_IndexSprite;
    Coroutine m_CorotineAnim;
    bool IsDone;

    public void LoadCarsFromJson()
    {
        string filePath = Application.persistentDataPath + "/CarStats.json";
        string carStatsData = System.IO.File.ReadAllText(filePath);
        carStats = JsonUtility.FromJson<CarStats>(carStatsData);
    }
    
    public void LoadTracksFromJson()
    {
        string filePath = Application.persistentDataPath + "/TrackStats.json";
        string trackStatsData = System.IO.File.ReadAllText(filePath);
        trackStats = JsonUtility.FromJson<TrackStats>(trackStatsData);
    }

    private void Start()
    {
        LoadCarsFromJson();
        LoadTracksFromJson();
        //Debug.Log(carStats.cars[1].carModel);
        //Debug.Log(trackStats.tracks[1].straights);
    }

    public void RaceCalculation(int _carNum1)
    {
        var car1 = carStats.cars[_carNum1];
        var car2 = carStats.cars[UnityEngine.Random.Range(0, carStats.cars.Count - 1)];
        var track = trackStats.tracks[trackNr];

        float car1StraightResult = ((car1.baseSpeed + (car1.baseAcceleration * 0.5f)) * track.straights);
        float car1TurnResult = ((car1.baseAcceleration + car1.baseHandling + car1.tireQuality) * track.turns);
        float car1RoadResult = ((car1.baseHandling + car1.tireQuality) * track.road);
        float car1Result = car1StraightResult + car1TurnResult + car1RoadResult;
        
        float car2StraightResult = ((car2.baseSpeed + (car2.baseAcceleration * 0.5f)) * track.straights);
        float car2TurnResult = ((car2.baseAcceleration + car2.baseHandling + car2.tireQuality) * track.turns);
        float car2RoadResult = ((car2.baseHandling + car2.tireQuality) * track.road);
        float car2Result = car2StraightResult + car2TurnResult + car2RoadResult;

        int car1ResultFinal = Mathf.RoundToInt(car1Result);
        int car2ResultFinal = Mathf.RoundToInt(car2Result);
        
        if (car1ResultFinal > car2ResultFinal)
        {
            Debug.Log("You Win!");
            //Debug.Log(car1.carModel);
            //Debug.Log(car2.carModel);
            WinScreen.SetActive(true);
            LoseScreen.SetActive(false);
            TiedScreen.SetActive(false);
            CloseRaceScreen.SetActive(true);
            StartCoroutine(Func_PlayAnimUI(WinAnimationImage, WinSpriteArray));
            //Debug.Log(m_IndexSprite);
        }
        if (car1ResultFinal < car2ResultFinal)
        {
            Debug.Log("You Lose!");
            //Debug.Log(car1.carModel);
            //Debug.Log(car2.carModel);
            LoseScreen.SetActive(true);
            TiedScreen.SetActive(false);
            WinScreen.SetActive(false);
            CloseRaceScreen.SetActive(true);
            StartCoroutine(Func_PlayAnimUI(LoseAnimationImage, LoseSpriteArray));
            //Debug.Log(m_IndexSprite);
        }
        if (Math.Abs(car1ResultFinal - car2ResultFinal) < 0.00001)
        {
            Debug.Log("It's a Tie!");
            //Debug.Log(car1.carModel);
            //Debug.Log(car2.carModel);
            TiedScreen.SetActive(true);
            LoseScreen.SetActive(false);
            WinScreen.SetActive(false);
            CloseRaceScreen.SetActive(true);
            StartCoroutine(Func_PlayAnimUI(TiedAnimationImage, TiedSpriteArray));
        }
    }

    public void SelectTrack(int _trackNr)
    {
        trackNr = _trackNr - 1;
    }
    
    IEnumerator Func_PlayAnimUI(Image m_Image, Sprite[] m_SpriteArray)
    {
        yield return new WaitForSeconds(m_Speed);
        
        m_Image.sprite = m_SpriteArray[m_IndexSprite];
        
        if (m_IndexSprite == m_SpriteArray.Length - 1)
        {
            IsDone = true;
        }
        
        m_IndexSprite += 1;
        
        if (m_IndexSprite > m_SpriteArray.Length - 1)
        {
            m_IndexSprite = 0;
            StopCoroutine(Func_PlayAnimUI(m_Image, m_SpriteArray));
        }
        
        if (IsDone == false)
        {
            m_CorotineAnim = StartCoroutine(Func_PlayAnimUI(m_Image, m_SpriteArray));
        }
    }

    public void IsDoneFalse()
    {
        IsDone = false;
        WinAnimationImage.sprite = WinSpriteArray[0];
        LoseAnimationImage.sprite = LoseSpriteArray[0];
        TiedAnimationImage.sprite = TiedSpriteArray[0];
    }
}

[Serializable]
public class CarStats
{
    public List<Cars> cars = new List<Cars>();
    public TextAsset jsonFile;
}

[Serializable]
public class Cars
{
    public string carModel;
    public float baseSpeed;
    public float baseAcceleration;
    public float baseHandling;
    public float tireQuality;
}

[Serializable]
public class TrackStats
{
    public List<Tracks> tracks = new List<Tracks>();
    public TextAsset jsonFile;
}

[Serializable]
public class Tracks
{
    public int trackNr;
    public float straights;
    public float turns;
    public float road;
}