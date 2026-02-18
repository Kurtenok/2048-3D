using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using Zenject;

public class DiceManager : MonoBehaviour
{
    [SerializeField] List<DiceView> diceViews;

    [System.Serializable]
    struct DiceView
    {
        public int diceNum;
        public Texture texture;
    }

    [Inject] private GameMode gameMode;

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

    public void ChangeDiceView(Transform dice, int numToSet,bool DiceCollided=false)
    {
        if(!dice) return;

        Renderer rend = dice.GetComponent<Renderer>();

        if(!rend) return;

        Texture textureToSet;

        if(!TryGetDiceTextureByNum(numToSet,out textureToSet)) return;
        
        rend.material.SetTexture("_MainTex",textureToSet);

        if(gameMode && DiceCollided)
        {
            gameMode.AddScore(numToSet/4); // /4 because numToSet is the next score, so it should be divided by 2 and additional /2 cause player gets only half of points

            MusicManager.singleton.PlayDiceMergeSound();
        }

    }
}
