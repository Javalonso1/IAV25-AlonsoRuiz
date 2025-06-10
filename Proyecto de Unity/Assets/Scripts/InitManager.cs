using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InitManager : MonoBehaviour
{
   
    void Start()
    {
        GameManager.Instance.setInitMan(this);
    }

    GameManager.Jug roj = GameManager.Jug.Player;
    GameManager.Jug verd = GameManager.Jug.Player;
    GameManager.Jug amar = GameManager.Jug.Player;
    GameManager.Jug azul = GameManager.Jug.Player;

    [SerializeField] Slider riskRoj;
    [SerializeField] Slider riskVerd;
    [SerializeField] Slider riskAmar;
    [SerializeField] Slider riskAzul;
    [SerializeField] Slider waitingTime;

    public void Comence()
    {
        GameManager.Instance.comence(roj, riskRoj.value, verd, riskVerd.value, amar, riskAmar.value, azul, riskAzul.value, waitingTime.value);
        SceneManager.LoadScene("Parchis");
    }

    public void ChangeIARojo()
    {
        if (roj == GameManager.Jug.Player) roj = GameManager.Jug.IA;
        else roj = GameManager.Jug.Player;
    }
    public void ChangeIAVerd()
    {
        if (verd == GameManager.Jug.Player) verd = GameManager.Jug.IA;
        else verd = GameManager.Jug.Player;
    }
    public void ChangeIAAmar()
    {
        if (amar == GameManager.Jug.Player) amar = GameManager.Jug.IA;
        else amar = GameManager.Jug.Player;
    }
    public void ChangeIAAzul()
    {
        if (azul == GameManager.Jug.Player) azul = GameManager.Jug.IA;
        else azul = GameManager.Jug.Player;
    }
}
