using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using Zenject;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameMode : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private Transform diceSpawnPoint;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Dice Control")]
    [SerializeField] private float sensitivity = 1;
    [SerializeField] private float maxOffset = 10f;
    [SerializeField] private float DicePushForce = 100;

    [Header("Settings")]
    [SerializeField] private float chanceOf4DiceSpawn=0.25f; // should be between 0 and 1

    [Header("LevelLoad")]
    [SerializeField] private GameObject LoadindScreen;

    [Header("WinCondition")]
    [SerializeField] private int scoreToWin=100;
    [SerializeField] private TextMeshProUGUI ScoreToWinText;
    [SerializeField] private Transform winScreen;
    [SerializeField] private TextMeshProUGUI winScreenScoreText;

    private GameObject controlledDice;
    private float startX; 
    private Vector2 startTouch;
    private int currentScore=0;
    private bool isContolLocked=false;

    [Inject] private DiceManager diceManager;

    void Awake()
    {
        chanceOf4DiceSpawn = Mathf.Clamp(chanceOf4DiceSpawn,0f,1f);

        currentScore=0;

        isContolLocked=false;
    }
    void Start()
    {
        UpdateScoreText();

        if(winScreen)
        {
            winScreen.gameObject.SetActive(false);
        }
        if(ScoreToWinText)
        {
            ScoreToWinText.text=$"To win: {scoreToWin}";
        }
    }

    void Update()
    {

        if(isContolLocked) return;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if(!dicePrefab) return;

                controlledDice = Instantiate(dicePrefab,diceSpawnPoint.position,Quaternion.identity);

                Dice dice= controlledDice.GetComponent<Dice>();

                if(diceManager)
                {   
                    dice.SetDiceManager(diceManager);
                }
                int rand = Random.Range(1,101);

                if(rand<=chanceOf4DiceSpawn*100)
                {
                    dice.SetDiceNum(4);
                }

                controlledDice.GetComponent<Rigidbody>().isKinematic = true;

                startTouch = touch.position;
                startX = controlledDice.transform.position.x;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                if(!controlledDice) return;

                float delta = (touch.position.x - startTouch.x) * sensitivity;

                float targetX = startX + delta;
                targetX = Mathf.Clamp(targetX, startX - maxOffset, startX + maxOffset);

               controlledDice.transform.position = new Vector3(
                    targetX,
                    controlledDice.transform.position.y,
                    controlledDice.transform.position.z
                );

            }

            if (touch.phase == TouchPhase.Ended)
            {
                if(!controlledDice) return;

                Rigidbody rb = controlledDice.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.AddRelativeForce(transform.forward * DicePushForce);


                MusicManager.singleton.PlayDiceLaunchSound();
            }
        }
    }

    public void UpdateScoreText()
    {
        if(!scoreText) return;

        scoreText.text =$"Score: {currentScore}";
    }

    public void AddScore(int numToAdd)
    {
        currentScore+=numToAdd;
        UpdateScoreText();

        if(currentScore>=scoreToWin)
        {
            EndGamebyWin();
        }
    }

    public void ReloadCurrentLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        LoadScene(level);
    }

    public void LoadScene(int buildIndex)
    {
        if (LoadindScreen)
            LoadindScreen.gameObject.SetActive(true);

        StartCoroutine(Loading(buildIndex));
    }

    IEnumerator Loading(int buildIndex)
    {
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(buildIndex);
        loadAsync.allowSceneActivation = false;

        while (!loadAsync.isDone)
        {
            if (loadAsync.progress >= 0.9f && !loadAsync.allowSceneActivation)
            {
                loadAsync.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private void EndGamebyWin()
    {
        isContolLocked=true;
        MusicManager.singleton.PlayWinSound();

        if(!winScreen)
        {
            Debug.LogError($"Set win screnn on {gameObject.name} to win");
            return;
        }

        if(winScreenScoreText)
        {
            winScreenScoreText.text=$"Score: {currentScore}";
        }

        winScreen.gameObject.SetActive(true);

    }

}
