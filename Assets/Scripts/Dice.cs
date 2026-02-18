using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    [SerializeField] private int diceNum = 2;
    private Rigidbody rb;
    [Inject] private DiceManager diceManager; // Imject here for dices, whis was spawned before the game launch

    void Awake()
    {
        rb=GetComponent<Rigidbody>();
    }
    void Start()
    {
        if(diceNum%2!=0 || diceNum<2)
        {
            SetDiceNum(2);
        }
    }

    void Update()
    {
        
    }
    public int GetDiceNum()
    {
        return diceNum;
    }
    public void SetDiceNum(int num)
    {
        if(num < 0 || num%2!=0)
        {
            Debug.LogError("Wrong number to set on dice "+ this.gameObject.name);
            return;
        }

        diceNum = num;

        UpdateDiceView();
    }

    public void UpdateDiceView()
    {
        if(!diceManager) return;

        diceManager.ChangeDiceView(this.transform,diceNum);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.TryGetComponent<Dice>(out Dice dice)) return;
        if(dice.GetDiceNum()!=diceNum) return;

        Rigidbody otherRb= collision.gameObject.GetComponent<Rigidbody>();

        if(otherRb.linearVelocity.magnitude==rb.linearVelocity.magnitude)
        {
            Destroy(collision.gameObject);
            SetDiceNum(diceNum * 2);
            return;
        }

        if(otherRb.linearVelocity.magnitude>rb.linearVelocity.magnitude) return;

        Destroy(collision.gameObject);
        SetDiceNum(diceNum * 2);
    }

    public void SetDiceManager(DiceManager manager)
    {
        diceManager = manager;
    }
}
