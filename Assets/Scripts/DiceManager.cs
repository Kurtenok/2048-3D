using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    [SerializeField] List<DiceView> diceViews;

    [System.Serializable]
    struct DiceView
    {
        public int diceNum;
        public Texture texture;
    }

    public static DiceManager singleton { get; private set;}

    void Awake()
    {
        if(singleton!=null)
        {
            Destroy(singleton.gameObject);
        }
        singleton=this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private bool TryGetDiceTextureByNum(int num,out Texture texture)
    {
        foreach(DiceView diceView in diceViews)
        {
            if(diceView.diceNum == num)
            {
                if(diceView.texture==null)
                {
                    Debug.LogError($"Texture for number {num} in {this.gameObject.name} isnt set");
                    continue;
                }
                texture = diceView.texture;
                return true;
            }
        }

        Debug.Log($"Texture for number {num} wasnt found");
        texture = null;
        return false;
    }

    public void ChangeDiceView(Transform dice, int numToSet)
    {
        if(!dice) return;

        Renderer rend = dice.GetComponent<Renderer>();

        if(!rend) return;

        Texture textureToSet;

        if(!TryGetDiceTextureByNum(numToSet,out textureToSet)) return;
        
        rend.material.SetTexture("_MainTex",textureToSet);

    }
}
