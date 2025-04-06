using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    GameObject coinPrefab;
    [SerializeField]
    LayerMask rayLayerMask_Floor;
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    TextMeshProUGUI text_Score;

    private float timer = 0;
    private float timerTotal = 1;

    private float timeSinceStart = 0;

    [HideInInspector]
    public int score = 0;

    private bool levelTransitioned = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 300; i++)
        {
            timer = 0;
            GameObject coin = Instantiate(coinPrefab);
            Vector3 v3 = new Vector3(Random.Range(-10f, 10f), Random.Range(1f, 3f), Random.Range(-10f, 10f));
            coin.transform.position = v3;
        }
    }


    // Update is called once per frame
    void Update()
    {
        timeSinceStart += Time.timeScale;

        if (score <= 0 && timeSinceStart > 800)
        {
            RestartScene();
        }
            if (SceneManager.GetActiveScene().name == "SampleScene" 
            && score > 200 && !levelTransitioned)
        {
            // Transition to level 2 (you can use the actual name of your next scene)
            SceneManager.LoadScene("Level2");

            // Set the flag to true to ensure it only happens once
            levelTransitioned = true;
        }

        if (SceneManager.GetActiveScene().name == "Level2"
            && score > 250 && levelTransitioned)
        {
            SceneManager.LoadScene("Level3");

        }

        if (SceneManager.GetActiveScene().name == "Level3"
            && score > 250 && levelTransitioned)
        {
            SceneManager.LoadScene("EndLevel");

        }

        //Create coins
        if (timer > timerTotal)
        {
            timer = 0;
            GameObject coin = Instantiate(coinPrefab);
            Vector3 v3 = new Vector3(Random.Range(-10f, 10f), 10, Random.Range(-10f, 10f));
            coin.transform.position = v3;
        }
        else
        {
            timer += Time.deltaTime;
        }
        //Update UI
        text_Score.text = "<color=#000fff>Score: </color>" + score;
    }
    public void RestartScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
