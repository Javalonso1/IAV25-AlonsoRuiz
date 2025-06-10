using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] Player roj;
    [SerializeField] Player ver;
    [SerializeField] Player ama;
    [SerializeField] Player azu;


    void LateUpdate()
    {
        GameManager.Instance.init(roj, ver, ama, azu);        
        Destroy(gameObject);
    }


}
