using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RollGenerator : MonoBehaviour
{
    [SerializeField] TMP_Text rollNum;

    void Start()
    {
        GameManager.Instance.setRollGenerator(this);
    }    
    public int Roll()
    {        
        int a = Random.Range(1, 7);        
        rollNum.text = "" + a;        
        return a;
    }
    public void setTo7(int i = 7)
    {
        rollNum.text = "" + i;
    }

    public void PlayerMoveFicha(int num)
    {
        GameManager.Instance.PlayerMoveFicha(num);
    }
}
