using UnityEngine.EventSystems;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private Transform diceSpawnPoint;
    [Header("Dice Control")]
    [SerializeField] private float sensitivity = 1;
    [SerializeField] private float maxOffset = 10f;
    [SerializeField] private float DicePushForce = 100;

    private GameObject controlledDice;
    private float startX;
    private Vector2 startTouch;
    void Start()
    {
        
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
}
