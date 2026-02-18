using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    [SerializeField] private int diceNum = 2;
    [SerializeField] private float minImpulseToCollide = 1;
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
    public void SetDiceNum(int num,bool DiceCollided=false)
    {
        if(num < 0 || num%2!=0)
        {
            Debug.LogError("Wrong number to set on dice "+ this.gameObject.name);
            return;
        }

        diceNum = num;

        UpdateDiceView(DiceCollided);


    }

    public void UpdateDiceView(bool DiceCollided=false)
    {
        if(!diceManager) return;

        diceManager.ChangeDiceView(this.transform,diceNum,DiceCollided);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.TryGetComponent<Dice>(out Dice dice)) return;
        if(dice.GetDiceNum()!=diceNum) return;

        Rigidbody otherRb= collision.gameObject.GetComponent<Rigidbody>();

        if(collision.impulse.magnitude<minImpulseToCollide)
        return;

        float thisDiceImpulse=rb.linearVelocity.magnitude;
        float otherDiceImpulse=otherRb.linearVelocity.magnitude;

        if(otherDiceImpulse==thisDiceImpulse)
        {
            Destroy(collision.gameObject);
            SetDiceNum(diceNum * 2,true);
            return;
        }

        if(otherDiceImpulse>thisDiceImpulse) return;

        Destroy(collision.gameObject);
        SetDiceNum(diceNum * 2,true);

    }

    public void SetDiceManager(DiceManager manager)
    {
        diceManager = manager;
    }
}
