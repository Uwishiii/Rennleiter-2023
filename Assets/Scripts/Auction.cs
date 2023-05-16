using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using Unity.Mathematics;

public class Auction : MonoBehaviour
{
    private List<GameObject> carList = new List<GameObject>();
    private List<GameObject> carPosList = new List<GameObject>();
    private List<GameObject> carBoughtPosList = new List<GameObject>();
    
    [SerializeField] public GameObject gameCamera;
    [SerializeField] public GameObject gameCameraPosGarage;
    [SerializeField] public GameObject gameCameraPosAuction;
    [SerializeField] public GameObject gameCameraPosRaceView;
    private float timeLeft;

    [SerializeField] private GameObject leftText;
    [SerializeField] private GameObject middleText;
    [SerializeField] private GameObject rightText;

    [SerializeField] private CurrentPoints pointsScript;
    private int points;
    private int cost;
    private List<int> costList = new List<int>();

    private void Start()
    {
        Initiation();
        
        InitiateLists();
        
        CarPositioner();
        
        CostShowcase();
    }
    
    private void Update()
    {
        CarSelector();
    }

    private void Initiation()
    {
        leftText.SetActive(true);
        middleText.SetActive(true);
        rightText.SetActive(true);
    }
    
    private void InitiateLists()
    {
        foreach (GameObject carPos in GameObject.FindGameObjectsWithTag("CarPos"))
        {
            carPosList.Add(carPos);
        }
        
        foreach (GameObject car in GameObject.FindGameObjectsWithTag("Car"))
        {
            carList.Add(car);
        }
        
        foreach (GameObject carBoughtPos in GameObject.FindGameObjectsWithTag("CarBoughtPos"))
        {
            carBoughtPosList.Add(carBoughtPos);
        }
    }

    private void CarPositioner()
    {
        for (int i = 0; i < 3; i++)
        {
            int randomCar = Random.Range(0, carList.Count);
            carList[randomCar].transform.position = carPosList[i].transform.position;
            carList.Remove(carList[randomCar]);
        }
    }

    private void CarSelector()
    {
        points = pointsScript.points;
        
        if (Input.GetMouseButtonDown(0/*& && points >= cost*/)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
                if (hit.transform != null && hit.transform.gameObject.CompareTag("Car"))
                {
                    if (hit.transform.name == "Car 1")
                    {
                        cost = costList[0];
                        
                        if (points >= cost)
                        {
                            int randomCarBoughtPos = Random.Range(0, carBoughtPosList.Count);
                    
                            hit.transform.position = carBoughtPosList[randomCarBoughtPos].transform.position;
                            hit.transform.rotation *= Quaternion.Euler(90, 0, 0);
                            carBoughtPosList.Remove(carBoughtPosList[randomCarBoughtPos]);
                            
                            leftText.SetActive(false);
                            pointsScript.points -= cost;
                        }
                    }
                    
                    if (hit.transform.name == "Car 2")
                    {
                        cost = costList[1];
                        
                        if (points >= cost)
                        {
                            int randomCarBoughtPos = Random.Range(0, carBoughtPosList.Count);
                    
                            hit.transform.position = carBoughtPosList[randomCarBoughtPos].transform.position;
                            hit.transform.rotation *= Quaternion.Euler(90, 0, 0);
                            carBoughtPosList.Remove(carBoughtPosList[randomCarBoughtPos]);
                            
                            middleText.SetActive(false);
                            pointsScript.points -= cost;
                        }
                    }
                    
                    if (hit.transform.name == "Car 3")
                    {
                        cost = costList[2];
                        
                        if (points >= cost)
                        {
                            int randomCarBoughtPos = Random.Range(0, carBoughtPosList.Count);
                    
                            hit.transform.position = carBoughtPosList[randomCarBoughtPos].transform.position;
                            hit.transform.rotation *= Quaternion.Euler(90, 0, 0);
                            carBoughtPosList.Remove(carBoughtPosList[randomCarBoughtPos]);
                            
                            rightText.SetActive(false);
                            pointsScript.points -= cost;
                        }
                    }
                }
        }
    }

    private void CostShowcase()
    {
        for (int i = 0; i < 3; i++)
        {
            costList.Add(Random.Range(5, 15));
        }

        leftText.GetComponent<TextMeshPro>().text = costList[0].ToString();
        middleText.GetComponent<TextMeshPro>().text = costList[1].ToString();
        rightText.GetComponent<TextMeshPro>().text = costList[2].ToString();
    }
    
    IEnumerator CameraMove2(int timeInSeconds, Quaternion desiredRotation, Vector3 desiredPosition)
    {
        while (gameCamera.transform.position != desiredPosition && gameCamera.transform.rotation != desiredRotation)
        {
            gameCamera.transform.position = Vector3.Lerp(gameCamera.transform.position, desiredPosition, Time.deltaTime * timeInSeconds);
            gameCamera.transform.rotation = Quaternion.Slerp(gameCamera.transform.rotation, desiredRotation, Time.deltaTime * timeInSeconds);
            
            timeLeft -= Time.deltaTime;
            
            yield return null;
        }
    }

    public void CameraMoveGarage()
    {
        timeLeft = 3.0f;
        StopAllCoroutines();
        StartCoroutine(CameraMove2(3, gameCameraPosGarage.transform.rotation, gameCameraPosGarage.transform.position));
    }
    
    public void CameraMoveAuction()
    {
        timeLeft = 3.0f;
        StopAllCoroutines();
        StartCoroutine(CameraMove2(3, gameCameraPosAuction.transform.rotation, gameCameraPosAuction.transform.position));
    }
    
    public void CameraMoveRaceView()
    {
        timeLeft = 3.0f;
        StopAllCoroutines();
        StartCoroutine(CameraMove2(3, gameCameraPosRaceView.transform.rotation, gameCameraPosRaceView.transform.position));
    }
}
