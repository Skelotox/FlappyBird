using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private List<Pipe> pipes = new List<Pipe>();
    private List<Coin> ScoreCoinList = new List<Coin>();
    private List<GameObject> coinList = new List<GameObject>();
    private Pipe spawnedPipe;
    private bool hasStarted;
    private bool hasLost;
    private bool spawnCoin;
    private bool isCoinDestroyed;
    private GameObject tap;
    private Vector3 scale;
    private GameObject endingScore;
    private Coin  coin= new Coin();

    [Header("Variables")]
    [SerializeField] private float pipeSpeedMultiplier = 0.8f;
    [SerializeField] private float timerBeforePipeSpawn = 2f;
    [SerializeField] private float timer;
    [SerializeField] private float pipeSpawnYMin = -0.2f;
    [SerializeField] private float pipeSpawnYMax = 0.7f;
    [SerializeField] private float timeBetweenFlap = 0.3f;
    [SerializeField] private bool canFlapAgain = true;
    [SerializeField] private int coinSpawnChance = 10;

    [Header("References")]
    [SerializeField] private FlappyBird flappyBird;
    [SerializeField] private Pipe pipePrefab;
    [SerializeField] private GameObject[] coinsPrefab;


    [Header("UI")]
    [SerializeField] private Sprite[] scoreSprites;
    [SerializeField] private List<Image> scoreImages = new List<Image>();
    [SerializeField] private GameObject imagePrefab;
    [SerializeField] private GameObject startGame;
    [SerializeField] private GameObject loseGame;
    [SerializeField] private GameObject scoreGo;
    [SerializeField] private GameObject exitContinueGo;
    [SerializeField] private GameObject[] coinsCount;
    [SerializeField] private Button pauseButton;

    private int score;

    #region Update and Start Methods
    private void Awake()
    {
        if(instance== null)
        {
            instance = this;
        }
        timer = timerBeforePipeSpawn;
        tap = startGame.gameObject.transform.Find("Tap").gameObject;
        scale = new Vector3(-0.001f, -0.001f, -0.001f);
    }

    /* CORE of the game!
     * 
     * Check if the game started (First Tap input)
     * If so :
     *        - Check if Bird can flap again (low timer) to avoid exessive flap related to Update method speed (60 flap per second) causing lags
     *        - Boost a little bit the speed when Bord goes down
     *        - If Bird goes out of camera and player Tap, then we reload the scene
     * Also make Pipe spawn and move toward the Bird (Personnal choice to do so) with a subjective timer variable between each pipe spawn
     * 
     */
    private void Update()
    {
        if(!hasLost)
        {

            if (InputManager.isSpaceBarPressed && !hasStarted && !hasLost)
            {
                StartGame();
            }
            else if (!hasStarted)
            {
                tap.transform.localScale += scale;

                if(tap.transform.localScale.x < 0.8f || tap.transform.localScale.x >= 1.0f)
                {
                    scale= -scale;
                }
            }

            if(InputManager.isSpaceBarPressed && hasStarted && canFlapAgain)
            {
                canFlapAgain= false;
                flappyBird.Flap();
                StartCoroutine(FlapAgainAfterTime());
            }
            else if(!InputManager.isSpaceBarPressed && hasStarted)
            {
                flappyBird.FlyDown();
            }
        }else if(hasLost && InputManager.isSpaceBarPressed && !hasStarted)
        {

            SceneManager.LoadScene("SampleScene");
        }

        
        if(timer <= 0 && hasStarted)
        {
            SpawnPipe();
            timer = timerBeforePipeSpawn;
            if (UnityEngine.Random.Range(0, 100) < coinSpawnChance)
            {
                spawnCoin = true;
            }
        }
        timer -= Time.deltaTime;
 
        foreach(Pipe pipe in pipes)
        {
            pipe.gameObject.transform.position -= new Vector3(Time.deltaTime * pipeSpeedMultiplier,0 ,0);
            
        }
        foreach(GameObject coin in coinList) 
        {
            coin.transform.position -= new Vector3(Time.deltaTime * pipeSpeedMultiplier, 0, 0);
            
        }
    }


    private IEnumerator FlapAgainAfterTime()
    {
        yield return new WaitForSeconds(timeBetweenFlap);
        canFlapAgain = true;
    }


    #endregion


    #region Spawn and Remove Pipes
    /* Instantiate and Remove Pipes
     * 
     * The position is simply calculated by subjective fixed variables
     * Could do way better to improve this
     * 
     */
    private void SpawnPipe()
    {
        float x = pipePrefab.transform.position.x;
        float xCoin  = coinsPrefab[0].transform.position.x;
        int coinNumber = UnityEngine.Random.Range(0,100);
        float minCoin = 0;
        GameObject spawnedCoin;
        if (pipes.Count > 2 )
        {
            minCoin = pipes.Last().transform.position.y;
        }
        
        //coinList.Add(coin);
        spawnedPipe = Instantiate(pipePrefab, new Vector3(x, UnityEngine.Random.Range(pipeSpawnYMin, pipeSpawnYMax), 0), Quaternion.identity);
        pipes.Add(spawnedPipe);
        
        if(pipes.Count > 2 && spawnCoin)
        {
            //xCoin = x - ((x - pipes[pipes.Count - 2].transform.position.x) / 2f);
            float maxCoin = pipes.Last().transform.position.y;
            if (coinNumber > 0 && coinNumber < 2 && spawnCoin)
            {
                coin = Coin.Ruby;
                spawnedCoin =Instantiate(coinsPrefab[2], new Vector3(xCoin, UnityEngine.Random.Range(Math.Abs(maxCoin - minCoin) - 0.5f * minCoin, Math.Abs(maxCoin - minCoin) + 0.5f * maxCoin), 0), Quaternion.identity);
                coinList.Add(spawnedCoin);
            }
            else if (coinNumber > 2 && coinNumber < 20 && spawnCoin)
            {
                coin = Coin.Gold;
                spawnedCoin = Instantiate(coinsPrefab[1], new Vector3(xCoin, UnityEngine.Random.Range(Math.Abs(maxCoin - minCoin) - 0.5f * minCoin, Math.Abs(maxCoin - minCoin) + 0.5f * maxCoin), 0), Quaternion.identity);
                coinList.Add(spawnedCoin);
            }
            else if (coinNumber > 20 && coinNumber < 100 && spawnCoin)
            {
                coin = Coin.Silver;
                spawnedCoin = Instantiate(coinsPrefab[0], new Vector3(xCoin, UnityEngine.Random.Range(Math.Abs(maxCoin - minCoin) - 0.5f * minCoin, Math.Abs(maxCoin - minCoin) + 0.5f * maxCoin), 0), Quaternion.identity);
                coinList.Add(spawnedCoin);
            }
            spawnCoin = false;
        }
    }

    public void RemovePipeFromList()
    {
        pipes.RemoveAt(0);
    }
    #endregion

    public void BirdGetCoin(GameObject coin)
    {
        switch (coin.name)
        {
            case "RubyCoin(Clone)":
                coinsCount[0].GetComponent<TextMeshProUGUI>().text = (int.Parse(coinsCount[0].GetComponent<TextMeshProUGUI>().text) + 1).ToString();
                break;
            case "GoldCoin(Clone)":
                coinsCount[1].GetComponent<TextMeshProUGUI>().text = (int.Parse(coinsCount[1].GetComponent<TextMeshProUGUI>().text) + 1).ToString();
                break;
            case "SilverCoin(Clone)":
                coinsCount[2].GetComponent<TextMeshProUGUI>().text = (int.Parse(coinsCount[2].GetComponent<TextMeshProUGUI>().text)+1).ToString();
                break;
        }
        coinList.Remove(coin);
        isCoinDestroyed = true;
        Destroy(coin);
    }
    public void RemoveCoinFromList()
    {
        if (isCoinDestroyed)
        {
            isCoinDestroyed= false;
        }
        else
        {

            coinList.RemoveAt(0);
        }
    }

    /* Increase game SCORE with IMAGES.
     * 
     * To do so, must make a number of copy of the existing image for digit 0-9 relative to the number of digit in the score :
     * "char[] scoreByChar = new char[score.ToString().Length];"
     * Then we create images and place them at the right position within the Horizontal Layer Group
     * And finnaly, we search for the good sprite in the List of digit's sprite to assignate them to the good image.
     * 
     */
    public void IncreaseScore(int point)
    {
        score += point;

        char[] scoreByChar = new char[score.ToString().Length];
        for (int i = 0; i < score.ToString().Length; i++)
        {
            scoreByChar[i] = score.ToString()[i];
        }

        if(scoreByChar.Length > scoreImages.Count)
        {
            for (int i = scoreImages.Count; i < scoreByChar.Length; i++)
            {
                GameObject newImg = Instantiate(imagePrefab);
                newImg.GetComponent<RectTransform>().SetParent(scoreImages[0].transform.parent.transform);
                newImg.transform.localScale = scoreImages[0].transform.localScale;

                scoreImages.Add(newImg.GetComponent<Image>());
            }
        }

        for (int i = 0; i < scoreImages.Count; i++)
        {
            scoreImages[i].sprite = scoreSprites[(int)Char.GetNumericValue(scoreByChar[i])];

        }
    }

    #region Start/Lose
    private void StartGame()
    {
        startGame.gameObject.SetActive(false);
        flappyBird.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        hasStarted = true;
    }

    public void LoseGame()
    {
        //Move the SCORE to the center of the screen when game ends
        endingScore = scoreGo;
        endingScore.transform.SetParent(loseGame.gameObject.transform);
        endingScore.transform.localPosition= loseGame.transform.localPosition;
        endingScore.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.LowerCenter;
        endingScore.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        Instantiate(endingScore);
        loseGame.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);

        //Disable the Bird and the Input System (for 2 secondes) to avoid instant restart when loosing while Tapping the screen
        flappyBird.GetComponent<FlappyBird>().enabled = false;
        hasLost= true;
        hasStarted = false;

        //Add coins to score
        for (int i = 0; i < coinsCount.Length; i++)
        {
            for (int j = int.Parse(coinsCount[i].GetComponent<TextMeshProUGUI>().text); j > 0 ; j--)
            {
                switch (i)
                {
                    case 0:
                        StartCoroutine(DelayEndingScore(i, (int)Coin.Ruby));
                        //IncreaseScore((int)Coin.Ruby);
                        //coinsCount[i].GetComponent<TextMeshProUGUI>().text = (int.Parse(coinsCount[i].GetComponent<TextMeshProUGUI>().text) - 1).ToString();
                        break;
                    case 1:
                        StartCoroutine(DelayEndingScore(i, (int)Coin.Gold));
                        //IncreaseScore((int)Coin.Gold);
                        //coinsCount[i].GetComponent<TextMeshProUGUI>().text = (int.Parse(coinsCount[i].GetComponent<TextMeshProUGUI>().text) - 1).ToString();
                        break;
                    case 2:
                        StartCoroutine(DelayEndingScore(i, (int)Coin.Silver));
                        //IncreaseScore((int)Coin.Silver);
                        //coinsCount[i].GetComponent<TextMeshProUGUI>().text = (int.Parse(coinsCount[i].GetComponent<TextMeshProUGUI>().text) - 1).ToString();
                        break;
                }
            }
        }
            

        InputManager.playerInput.enabled= false;
        StartCoroutine(DelayRestart());
    }

    private void TransferCoinValue(int i, int value)
    {
        IncreaseScore(value);
        coinsCount[i].GetComponent<TextMeshProUGUI>().text = (int.Parse(coinsCount[i].GetComponent<TextMeshProUGUI>().text) - 1).ToString();
    }

    private IEnumerator DelayRestart()
    {
        yield return new WaitForSeconds(2f);
        InputManager.playerInput.enabled = true;
    }

    private IEnumerator DelayEndingScore(int index, int value)
    {
        yield return new WaitForSeconds(0.5f);
        TransferCoinValue(index, value);
    }

    #endregion


    #region Pause Menu
    public void Pause()
    {
        Time.timeScale = 0;
        exitContinueGo.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        exitContinueGo.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    #endregion

    public enum Coin
    {
        Silver = 5,
        Gold = 10,
        Ruby = 50
    }
}
