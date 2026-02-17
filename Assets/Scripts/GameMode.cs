using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

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

    private GameObject controlledDice;
    private float startX; 
    private Vector2 startTouch;
    private int currentScore=0;

    void Awake()
    {
        chanceOf4DiceSpawn = Mathf.Clamp(chanceOf4DiceSpawn,0,1);

        currentScore=0;
    }
    void Start()
    {
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if(!dicePrefab) return;

                controlledDice = Instantiate(dicePrefab,diceSpawnPoint.position,Quaternion.identity);

                if(Random.Range(1,101)<=chanceOf4DiceSpawn*100)
                {
                    controlledDice.GetComponent<Dice>().SetDiceNum(4);
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
            }
        }
    }

    public void UpdateScoreText()
    {
        if(!scoreText) return;

        scoreText.text =$"Score: {currentScore}";
    }
}
