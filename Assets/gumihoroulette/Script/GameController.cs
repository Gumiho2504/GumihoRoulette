///  date : thu aug 22 2024
///  developer name - hem chanmetreu
///  game name - roulette
///


using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
enum RESULT
{
    NONE,BLACK, RED,ONETO18,NIGHTEENTO36,ODD,EVEN
}

struct PlayerGuessResult
{
    RESULT guessResult;
    int guessResultCoin;
    
}
public class GameController : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject coinPrefab;

    [Header("-------Color---------------")]
    public Color32 blackColor;
    public Color32 redColor;
    public Color32 whiteColor;
    public Color32 greenColor;
    public Color32 matchColor;
    public Color32 normalColor;

    [Header("-------GameObject--------")]

    public GameObject barGameObject;
    public GameObject wheelGameObject;
    public GameObject glass_in;
    public GameObject glass_out;
    public List<GameObject> betTableGameObjectList = new List<GameObject>();
    public GameObject coinParent;
    public GameObject homeScreen,loadingScreen;


    public Transform spawnPoint; 
    public Transform parentObject; 
    public int numberOfBalls = 5;
    public float spawnInterval = 0.5f;

    [Header("-------Animator--------")]
    public Animator wheelAnimator;


    [Header("-------Image--------")]
    public Image resultImage;

    [Header("-------TEXT--------")]
    [SerializeField] private Text totalCoinText;
    [SerializeField] private Text evenGuessCoinText;
    [SerializeField] private Text oddGuessCoinText;
    [SerializeField] private Text guessCoin118Text;
    [SerializeField] private Text guessCoin1936Text;
    [SerializeField] private Text redGuessCoinText;
    [SerializeField] private Text blackGuessCoinText;
    [SerializeField] private Text winCoinText;

    public static bool isBallColiResultBar = true;
    public static bool isRestartGame = false;
    public static string resultString;

    int totalCoin = 10000;
    int totalBetCoin = 0;
    int winCoin = 0;
    int evenGuessCoin = 0;
    int oddGuessCoin = 0;
    int guessCoin118 = 0;
    int guessCoin1936 = 0;
    int redGuessCoin = 0;
    int blackGuessCoin = 0;
    int betCoin = 10;

    List<RESULT> gameResultList = new List<RESULT>();

    RESULT guessEven = RESULT.NONE;
    RESULT guessOdd = RESULT.NONE;
    RESULT guess118 = RESULT.NONE;
    RESULT guess1936 = RESULT.NONE;
    RESULT guessRed = RESULT.NONE;
    RESULT guessBlack = RESULT.NONE;

    [SerializeField] private Button spinButton;

    private void Awake()
    {

        UpdateTextUI();
    }

    private IEnumerator Start()
    {
        LeanTween.scale(loadingScreen.transform.GetChild(0).gameObject, new Vector3(0.8f, 0.8f, 0.8f), 1f).setEaseInOutQuart().setLoopPingPong();
        yield return new WaitForSeconds(4f);
        LeanTween.scale(loadingScreen, Vector3.zero, 1f).setEaseInOutQuart();
        LeanTween.cancel(loadingScreen.transform.GetChild(0).gameObject);


        coinPrefab.GetComponent<Image>().sprite = tempButton.image.sprite;
        coinPrefab.transform.GetChild(0).GetComponent<Text>().text = betCoin.ToString();
        spinButton.interactable = false;
        StartCoroutine(SpawnBalls());
    }

    private void FixedUpdate()
    {
        
    }


    IEnumerator SpawnBalls()
    {
        for (int i = 0; i < 37; i++)
        {
            GameObject spawnedBall = Instantiate(ballPrefab,new Vector3(spawnPoint.transform.position.x + Random.RandomRange(-1,1), spawnPoint.transform.position.y, spawnPoint.transform.position.z), Quaternion.identity);
            if(i == 0)
            {
                spawnedBall.gameObject.name = i.ToString("00") + "g";
                spawnedBall.GetComponent<SpriteRenderer>().color = greenColor;
                spawnedBall.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = i.ToString();
                spawnedBall.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().color = whiteColor;
            }
            else
            {
                if (i % 2 == 0)
                {
                    spawnedBall.gameObject.name = i.ToString("00") + "b";
                    spawnedBall.GetComponent<SpriteRenderer>().color = blackColor;
                    spawnedBall.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = i.ToString();
                    spawnedBall.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().color = whiteColor;

                }
                else
                {
                    spawnedBall.gameObject.name = i.ToString("00") + "r";
                    spawnedBall.GetComponent<SpriteRenderer>().color = redColor;
                    spawnedBall.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = i.ToString();
                    spawnedBall.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().color = whiteColor;
                }
            }
            


            spawnedBall.transform.SetParent(parentObject);
            AnimateBall(spawnedBall);

            yield return new WaitForSeconds(0.1f);
            
        }
        
    }

    void AnimateBall(GameObject ball)
    {
      
        float moveDuration = 0.1f; 
        float moveDistance = 1f; 

        Vector3 originalPosition = ball.transform.position;
        Vector3 targetPosition = originalPosition + Vector3.up * moveDistance;

        LeanTween.move(ball, targetPosition, moveDuration)
            .setEase(LeanTweenType.easeOutQuad)
            .setLoopPingPong(1); // Move up and then back down
    }

// ==========================onclick button ========================================

    public void OnClickSpin(GameObject g)
    {
        AudioController.Instance.PlaySFX("tap");
        AnimateButtonPress(g);
        spinButton.interactable = false;
        isOnClickBet = false;
        StartCoroutine(SpinAnimation());
    }

    bool isOnClickBet = true;
    public void onClickBet(int i)
    {
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

        AnimateButtonPress(betTableGameObjectList[i]);

        if (isOnClickBet && totalCoin >= betCoin && totalCoin > 0)
        {
            AudioController.Instance.PlaySFX("coin");
            GameObject coin = Instantiate(coinPrefab, new Vector3(selectedObject.transform.localPosition.x + Random.RandomRange(-70, 70),
                selectedObject.transform.localPosition.y + Random.RandomRange(-70, 70),
                selectedObject.transform.localPosition.z + Random.RandomRange(-50, 50)
                ), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            LeanTween.scale(coin, Vector3.one * 1.5f, 0.3f).setEase(LeanTweenType.easeOutElastic).setOnComplete(() =>
            {
                LeanTween.scale(coin,new Vector3(0.8f,0.8f,0.8f), 0.3f).setEase(LeanTweenType.easeInElastic);
            });
            coin.transform.SetParent(coinParent.transform, false);

            spinButton.interactable = true;
            totalCoin -= betCoin;
            totalBetCoin += betCoin;
            switch (i)
            {
                case 0:
                    
                    guessCoin118 += betCoin;
                    guess118 = RESULT.ONETO18;
                    break;
                case 1:
                  
                    guessCoin1936 += betCoin;
                    guess1936 = RESULT.NIGHTEENTO36;
                    break;
                case 2:
                   
                    evenGuessCoin += betCoin;
                    guessEven = RESULT.EVEN;
                    break;
                case 3:
                    
                    oddGuessCoin += betCoin;
                    guessOdd = RESULT.ODD;
                    break;
                case 4:
                    redGuessCoin += betCoin;
                    guessRed = RESULT.RED;
                    break;
                default:
                    blackGuessCoin += betCoin;
                    guessBlack = RESULT.BLACK;
                    break;
            }

            UpdateTextUI();
        }  
    }

    public Button tempButton;
    public void OnClickChangeBetCoinType(Button currentButton)
    {
        string name = currentButton.name.ToLower();
        AudioController.Instance.PlaySFX("tap");
        if (currentButton.GetInstanceID() != tempButton.GetInstanceID())
        {
            GameObject blockPanel = currentButton.transform.GetChild(1).gameObject;
            LeanTween.scale(blockPanel, Vector3.one, 0.5f).setEaseSpring();
            blockPanel.SetActive(true);

            blockPanel = tempButton.transform.GetChild(1).gameObject;
            LeanTween.scale(blockPanel, Vector3.zero, 0.5f).setEaseSpring();
           // blockPanel.SetActive(false);
            tempButton = currentButton;
        }
        
        
        switch (name)
        {
            case "10":
                betCoin = 10;
                break;
            case "100":
                betCoin = 100;
                break;
            case "200":
                betCoin = 200;
                break;
            case "50":
                betCoin = 50;
                break;
            default:
                betCoin = 500;
                break;
        }
        coinPrefab.GetComponent<Image>().sprite = currentButton.image.sprite;
        coinPrefab.transform.GetChild(0).GetComponent<Text>().text = betCoin.ToString();
    }


    // ========================== game-loop - setup ========================================

    IEnumerator SpinAnimation()
    {
        LeanTween.rotateAround(barGameObject, Vector3.forward, -360f, 3f)
                    .setEase(LeanTweenType.easeOutQuad).setLoopClamp();
        AudioController.Instance.PlaySFX("shake");
        AudioController.Instance.PlaySFX("shake");
        yield return new WaitForSeconds(3f);
        glass_in.SetActive(false);

        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(0.5f);
        AudioController.Instance.PlaySFX("shake");
        LeanTween.rotateAround(wheelGameObject, Vector3.forward, 315f, 5f)
                    .setEase(LeanTweenType.easeOutQuad);

        yield return new WaitForSeconds(5f);

        LeanTween.cancel(barGameObject);
        glass_out.SetActive(false);

    }

    public void ResultShow() {

        print($"result - {resultString}");

        LeanTween.scale(resultImage.gameObject, Vector3.one, 1f).setEaseSpring();
        AudioController.Instance.PlaySFX("show");
        LeanTween.rotateAround(wheelGameObject, Vector3.forward, 45f, 4f)
                   .setEase(LeanTweenType.easeOutQuad);

        glass_out.SetActive(true);

        StartCoroutine(ResetGameShowAnimation());
       /// isRestartGame = true;
    }

    IEnumerator ResetGameShowAnimation()
    {
        yield return ResultShowAnimation();

        yield return new WaitForSeconds(3f);

        GameObject remove = GameObject.Find("remove");
        Destroy(remove);
        LeanTween.scale(resultImage.gameObject, Vector3.zero, 1f).setEaseSpring();

        isBallColiResultBar = true;


        gameResultList.Clear();
        betTableGameObjectList.ForEach(g => g.GetComponent<Image>().color = normalColor);

        winCoin = totalBetCoin = guessCoin1936 = guessCoin118 = evenGuessCoin = oddGuessCoin = redGuessCoin = blackGuessCoin = 0;

        isOnClickBet = true;
        spinButton.interactable = false;

        for(int i = 0;i<coinParent.transform.childCount; i++)
        {
            Destroy(coinParent.transform.GetChild(i).gameObject);
        }

        UpdateTextUI();
    }

    IEnumerator ResultShowAnimation()
    {


        int resultNumber = 0;
        try {
            resultNumber = int.Parse(resultString.Substring(0, 2));
        }
        catch(System.Exception e)
        {
            throw e.GetBaseException();
        }
        // Correct usage of int.Parse
        string colorResul = resultString.Substring(resultString.Length - 1);

        //Debug.Log("The first two characters converted to int: " + resultNumber + "-" + colorResul);


        RESULT oddEven = RESULT.NONE;
        RESULT redBlack = RESULT.NONE;
        RESULT fromTo = RESULT.NONE;

        if (resultNumber % 2 == 0) oddEven = RESULT.EVEN;
        else oddEven = RESULT.ODD;




        if (colorResul == "r") redBlack = RESULT.RED;
        else redBlack = RESULT.BLACK;


        if (resultNumber >= 1 && resultNumber <= 18) fromTo = RESULT.ONETO18;
        else fromTo = RESULT.NIGHTEENTO36;

        gameResultList.Add(oddEven);
        gameResultList.Add(redBlack);
        gameResultList.Add(fromTo);

        //gameResultList.ForEach(r => print(r));

        yield return new WaitForSeconds(1f);

        // table bet panel match result

        foreach(RESULT r in gameResultList)
        {
            if (r == RESULT.ONETO18) betTableGameObjectList[0].GetComponent<Image>().color = matchColor;
            else if(r == RESULT.NIGHTEENTO36) betTableGameObjectList[1].GetComponent<Image>().color = matchColor;
            else if (r == RESULT.EVEN) betTableGameObjectList[2].GetComponent<Image>().color = matchColor;
            else if (r == RESULT.ODD) betTableGameObjectList[3].GetComponent<Image>().color = matchColor;
            else if (r == RESULT.RED) betTableGameObjectList[4].GetComponent<Image>().color = matchColor;
            else if (r == RESULT.BLACK) betTableGameObjectList[5].GetComponent<Image>().color = matchColor;

        }

        yield return new WaitForSeconds(1f);

        // Caculate coin win
        foreach (RESULT r in gameResultList)
        {
            if (r == guess118) { winCoin += guessCoin118 * 2; }
            else if (r == guess1936) { totalCoin += guessCoin1936 * 2; winCoin += guessCoin1936; }
            else if (r == guessEven) { totalCoin += evenGuessCoin * 2; winCoin += evenGuessCoin; }
            else if (r == guessOdd) { totalCoin += oddGuessCoin * 2; winCoin += oddGuessCoin; }
            else if (r == guessRed) { totalCoin += redGuessCoin * 2; winCoin += redGuessCoin; }
            else if (r == guessBlack) { totalCoin += blackGuessCoin * 2; winCoin += blackGuessCoin; }
           
        }

        UpdateTextUI();
        print($"totalbet - {totalBetCoin} - win - {winCoin}");
        if(winCoin*2 > totalBetCoin)
        {
            winCoinText.text = $"YOU WIN PROFIT {winCoin*2 - totalBetCoin}$";
        }
        else if (winCoin*2 < totalBetCoin)
        {
            winCoinText.text = $"YOU LOSS {totalBetCoin-winCoin*2}$";
        }
        else
        {
            winCoinText.text = $"YOU NOT LOSS BECAUSE YOU TOTALBET = WINBET";
        }
        AudioController.Instance.PlaySFX("show");
        ShowWinningAnimation(winCoinText.gameObject);
    }


    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    void UpdateTextUI()
    {
        totalCoinText.text = totalCoin.ToString() + "$";
        evenGuessCoinText.text = evenGuessCoin.ToString() + "$";
        oddGuessCoinText.text = oddGuessCoin.ToString() + "$";
        guessCoin118Text.text = guessCoin118.ToString() + "$";
        guessCoin1936Text.text = guessCoin1936.ToString() + "$";
        redGuessCoinText.text = redGuessCoin.ToString() + "$";
        blackGuessCoinText.text = blackGuessCoin.ToString() + "$";
        winCoinText.text = winCoin.ToString() + "$";
    }


















    //////////Animation////////
    Image fadeImage;
    public void FadeIn()
    {
        LeanTween.alpha(fadeImage.rectTransform, 0f, 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            fadeImage.gameObject.SetActive(false);
        });
    }

    public void FadeOutAndLoadScene(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);
        LeanTween.alpha(fadeImage.rectTransform, 1f, 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }


    public void AnimateButtonPress(GameObject button)
    {
        LeanTween.scale(button, Vector3.one * 0.9f, 0.1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            LeanTween.scale(button, Vector3.one, 0.1f).setEase(LeanTweenType.easeInOutQuad);
        });
    }

    public void ShowWinningAnimation(GameObject winningText)
    {
        winningText.SetActive(true);
        //messageShowPanel.SetActive(true);
        LeanTween.scale(winningText, Vector3.one * 1.1f, 0.5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(() =>
        {
            LeanTween.scale(winningText, Vector3.one, 0.5f).setEase(LeanTweenType.easeInElastic).setOnComplete(() =>
            {
                winningText.SetActive(false);
                //messageShowPanel.SetActive(false);
            });
        });
    }

    public void AnimateScoreUpdate(Text scoreText)
    {
        LeanTween.scale(scoreText.gameObject, Vector3.one * 1.2f, 0.3f).setEase(LeanTweenType.easeOutBounce).setOnComplete(() =>
        {
            LeanTween.scale(scoreText.gameObject, Vector3.one, 0.3f).setEase(LeanTweenType.easeInBounce);
        });
    }

    public void StartButton(GameObject g)
    {
        AudioController.Instance.PlaySFX("tap");
        AnimateButtonPress(g);
        LeanTween.scale(homeScreen, Vector3.zero, 0.5f).setEaseInOutElastic();
    }

    public void HomeButton(GameObject g)
    {
        AudioController.Instance.PlaySFX("tap");
        AnimateButtonPress(g);
        LeanTween.scale(loadingScreen, Vector3.one, 0.5f).setEaseInOutElastic().setOnComplete(()=> { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); });
    }
    public void QuitButton()
    {
        AudioController.Instance.PlaySFX("tap");
        Application.Quit();
    }
}