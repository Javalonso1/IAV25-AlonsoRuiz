using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAppearanceManager : MonoBehaviour
{
    [SerializeField] Image Butt1;
    [SerializeField] Image Butt2;
    [SerializeField] Image Butt3;
    [SerializeField] Image Butt4;
    void Start()
    {
        GameManager.Instance.setButtAppMan(this);
    }

    public void setColors(int one, int two, int three, int fourth)
    {                
        setIndividual(one, ref Butt1);
        setIndividual(two, ref Butt2);
        setIndividual(three, ref Butt4);
        setIndividual(fourth, ref Butt3);
    }
    void setIndividual(int cant, ref Image i)
    {
        switch (cant)
        {
            case 0:
                i.color = new Color(1, 1, 1);
                break;
            case 1:
                i.color = new Color(0.47f, 0.47f, 0.47f);
                break;
            case 2:
                i.color = new Color(0, 1, 0);
                break;
            default:
                break;
        }
    }
}
