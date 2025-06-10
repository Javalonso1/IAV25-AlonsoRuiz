using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TurnColor : MonoBehaviour
{
    [SerializeField] Image col;

    [SerializeField] GameObject Vict;
    [SerializeField] Image colVict;

    void Start()
    {
        Vict.SetActive(false);
        GameManager.Instance.setTurnColor(this);
    }

    public void Change(TableroManager.Col c)
    {
        switch (c)
        {
            case TableroManager.Col.Rojo:
                col.color = Color.red;
                break;
            case TableroManager.Col.Verde:
                col.color = Color.green;
                break;
            case TableroManager.Col.Amarillo:
                col.color = Color.yellow;
                break;
            case TableroManager.Col.Azul:
                col.color = Color.blue;
                break;
            default:
                break;
        }
    }

    public void Victory(TableroManager.Col c)
    {
        Vict.SetActive(true);
        switch (c)
        {
            case TableroManager.Col.Rojo:
                colVict.color = Color.red;
                break;
            case TableroManager.Col.Verde:
                colVict.color = Color.green;
                break;
            case TableroManager.Col.Amarillo:
                colVict.color = Color.yellow;
                break;
            case TableroManager.Col.Azul:
                colVict.color = Color.blue;
                break;
            default:
                break;
        }
    }

    public void GoBack()
    {
        GameManager.Instance.destruction();
        SceneManager.LoadScene("MainMenu");
    }
}
