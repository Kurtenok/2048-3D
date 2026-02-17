using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private int diceNum = 2;
    private Rigidbody rb;

    void Awake()
    {
        rb=GetComponent<Rigidbody>();
    }
    void Start()
    {
       SetDiceNum(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetDiceNum()
    {
        return diceNum;
    }
    public void SetDiceNum(int num)
    {
        if(num < 0 || !Mathf.IsPowerOfTwo(num))
        {
            Debug.LogError("Wrong number to set on dice "+ this.gameObject.name);
            return;
        }

        diceNum = num;

        UpdateDiceView();
    }

    public void UpdateDiceView()
    {
        DiceManager.singleton.ChangeDiceView(this.transform,diceNum);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.TryGetComponent<Dice>(out Dice dice)) return;
        if(dice.GetDiceNum()!=diceNum) return;

        Rigidbody otherRb= collision.gameObject.GetComponent<Rigidbody>();

        if(otherRb.linearVelocity.magnitude>rb.linearVelocity.magnitude) return;

        Destroy(collision.gameObject);
        SetDiceNum((int)Mathf.Pow(diceNum,2));
    }
}
