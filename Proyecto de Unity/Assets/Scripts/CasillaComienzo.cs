using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasillaComienzo : MonoBehaviour
{
    [SerializeField] int num;
    [SerializeField] bool safe;
    [SerializeField] TableroManager.Col st;
    [SerializeField] TableroManager.Col end;

    void Start()
    {
        if(num >=0 && num <= 67)
        {
            TableroManager.Instance.getCasilla(num,safe, GetComponent<Transform>(), st, end);
        }
    }
}
