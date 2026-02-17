using UnityEngine;

public class Dice : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private int diceNum = 2;
    void Start()
    {
       SetDiceNum(4);
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
}
