using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public TableroManager.Col mColor;

    [SerializeField] Transform[] HomeSpaces;
    [SerializeField] Transform[] EndSpaces;

    [SerializeField] Transform[] Fichas;

    [SerializeField] GameObject botonesEleccion;

    public TableroManager.Ficha[] misFichas = new TableroManager.Ficha[4];
    private bool esIA;
    public float risk;
    private int mov;
    private bool repeat = false;
    private int repeatCounter = 0;
    private int lastMoved;

    private bool freeHome1 = false;
    private bool freeHome2 = false;
    private bool freeHome3 = false;
    private bool freeHome4 = true;

    public int initPos = -1;
    public int exitPos = -1;    

    void Start()
    {
        botonesEleccion.SetActive(false);
        TableroManager.Instance.AddPlayer(this);        
    }

    public void initAtributes(bool ia, float ris)
    {
        esIA = ia;
        risk = ris;
        initPos = TableroManager.Instance.initPos(mColor);
        exitPos = TableroManager.Instance.exitPos(mColor);
        misFichas[0].pos = initPos;
        misFichas[0].t = Fichas[0];
        misFichas[0].mCol = mColor;
        misFichas[0].setToPos();

        misFichas[1].pos = -1;
        misFichas[1].t = Fichas[1];
        misFichas[1].mCol = mColor;
        misFichas[1].t.position = HomeSpaces[0].position;

        misFichas[2].pos = -2;
        misFichas[2].t = Fichas[2];
        misFichas[2].mCol = mColor;
        misFichas[2].t.position = HomeSpaces[1].position;

        misFichas[3].pos = -3;
        misFichas[3].t = Fichas[3];
        misFichas[3].mCol = mColor;
        misFichas[3].t.position = HomeSpaces[2].position;
    }
    public bool CheckAllOut()
    {
        return (freeHome1 && freeHome2 && freeHome3 && freeHome4);
    }
    public void Play(int extramov = 0)
    {
        TableroManager.Instance.AreTherePhantomFichas();
        if (extramov !=0)
        {
            mov = extramov;
            GameManager.Instance.setUI7(extramov);
        }
        else mov = GameManager.Instance.Roll();        

        if(mov == 5)
        {                      
            int someoneInStart = doIHaveTwoMineInStart();
            if (!CheckAllOut() && someoneInStart !=2)
            {
                if(someoneInStart == 0)
                {                                        
                    int i = 0;
                    bool aux = false;
                    while (!aux)
                    {
                        if (misFichas[i].pos < 0) aux = true;
                        else i++;
                    }
                    switch (misFichas[i].pos)
                    {
                        case -1:
                            freeHome1 = true;
                            break;
                        case -2:
                            freeHome2 = true;
                            break;
                        case -3:
                            freeHome3 = true;
                            break;
                        case -4:
                            freeHome4 = true;
                            break;
                        default:
                            break;
                    }
                    misFichas[i].pos = initPos;
                    misFichas[i].setToPos();
                    mov = 0;
                    GameManager.Instance.WaitThenMove(false);
                    botonesEleccion.SetActive(false);
                }                
                else
                {                    
                    TableroManager.Instance.tryDelete(initPos, mColor);
                    int i = 0;
                    bool aux = false;
                    while (!aux)
                    {
                        if (misFichas[i].pos < 0) aux = true;
                        else i++;
                    }
                    switch (misFichas[i].pos)
                    {
                        case -1:
                            freeHome1 = true;
                            break;
                        case -2:
                            freeHome2 = true;
                            break;
                        case -3:
                            freeHome3 = true;
                            break;
                        case -4:
                            freeHome4 = true;
                            break;
                        default:
                            break;
                    }
                    misFichas[i].pos = initPos;
                    misFichas[i].setToPos();
                    mov = 0;
                    GameManager.Instance.WaitThenMove(false);
                    botonesEleccion.SetActive(false);
                }           
            }
            else
            {
                if (canYouEvenMove(mov))
                {
                    if (esIA)
                    {
                        mov = 5;
                        repeat = false;
                        AIManager.Instance.MoveMe(mov);
                    }
                    else
                    {
                        mov = 5;
                        repeat = false;
                        botonesEleccion.SetActive(true);
                        setColorsBotones(mov);
                    }
                }
                else
                {
                    GameManager.Instance.WaitThenMove(false);
                }                                
            }            
        }
        else
        {            
            if (mov == 6)
            {                
                repeat = true;
                if (canYouEvenMove(mov))
                {
                    if (repeatCounter >= 2)
                    {
                        repeatCounter = 0;
                        repeat = false;
                        if(lastMoved != -1) KillFicha(lastMoved);
                        lastMoved = -1;
                        GameManager.Instance.WaitThenMove(false);
                    }
                    else
                    {                        
                        //repeatCounter++;
                        if (CheckAllOut())
                        {
                            GameManager.Instance.setUI7();
                            mov = 7;
                        }
                        if (esIA)
                        {                            
                            AIManager.Instance.MoveMe(mov);
                        }
                        else
                        {
                            botonesEleccion.SetActive(true);
                            setColorsBotones(mov);
                        }
                    }
                }
                else
                {
                    if (repeatCounter >= 2)
                    {
                        repeatCounter = 0;
                        repeat = false;
                        if(lastMoved !=-1) KillFicha(lastMoved);
                        lastMoved = -1;
                        GameManager.Instance.WaitThenMove(false);
                    }
                    else
                    {                        
                        //repeatCounter++;
                        GameManager.Instance.WaitThenMove(true);
                    }                    
                }
            }
            else
            {
                if(extramov == 0)
                {
                    repeat = false;
                    repeatCounter = 0;
                }
                if (canYouEvenMove(mov))
                {
                    if (esIA)
                    {
                        AIManager.Instance.MoveMe(mov);
                    }
                    else
                    {
                        botonesEleccion.SetActive(true);
                        setColorsBotones(mov);
                    }
                }
                else
                {
                    GameManager.Instance.WaitThenMove(false);
                }
            }
            
        }
        
    }

    public void ManualMovement(int a)
    {
        if (CheckIfHasBarrera() && (mov == 6 || mov == 7))
        {
            if (misFichas[a].pos >= 0 && misFichas[a].canMoveBarrera(mov, exitPos, misFichas))
            {
                misFichas[a].Move(mov, exitPos, EndSpaces);
                lastMoved = misFichas[a].pos;
                mov = 0;
                botonesEleccion.SetActive(false);
                if (esIA) GameManager.Instance.WaitThenMoveIA(repeat);
                else GameManager.Instance.NextMove(repeat);
                if (repeat)
                {                    
                    repeatCounter++;
                }
                else repeatCounter = 0;
            }
        }
        else
        {
            if (misFichas[a].pos >= 0 && misFichas[a].canMove(mov, exitPos))
            {
                misFichas[a].Move(mov, exitPos, EndSpaces);
                lastMoved = misFichas[a].pos;
                mov = 0;
                botonesEleccion.SetActive(false);
                if (esIA) GameManager.Instance.WaitThenMoveIA(repeat);
                else GameManager.Instance.NextMove(repeat);
                if (repeat)
                {                    
                    repeatCounter++;
                }
                else repeatCounter = 0;
            }
        }
    }
    public void KillFicha(int pos)
    {
        int i = 0;
        bool aux = false;
        while(!aux && i < misFichas.Length)
        {
            if (misFichas[i].pos == pos) aux = true;
            else i++;
        }
        if (aux)
        {
            if (freeHome1)
            {
                freeHome1 = false;
                misFichas[i].pos = -1;
                misFichas[i].t.position = HomeSpaces[0].position;
            }
            else
            {
                if (freeHome2)
                {
                    freeHome2 = false;
                    misFichas[i].pos = -2;
                    misFichas[i].t.position = HomeSpaces[1].position;
                }
                else
                {
                    if (freeHome3)
                    {
                        freeHome3 = false;
                        misFichas[i].pos = -3;
                        misFichas[i].t.position = HomeSpaces[2].position;
                    }
                    else
                    {
                        if (freeHome4)
                        {
                            freeHome4 = false;
                            misFichas[i].pos = -4;
                            misFichas[i].t.position = HomeSpaces[3].position;
                        }
                    }
                }
            }            
        }
    }

    public bool CheckIfHasBarrera()
    {
        bool aux = false;
        int i = 0;
        while(!aux && i < misFichas.Length - 1)
        {
            if (misFichas[i].pos != 108)
            {
                int j = i + 1;
                while (!aux && j < misFichas.Length)
                {
                    if (misFichas[i].pos == misFichas[j].pos) aux = true;
                    j++;
                }
            }
            i++;
        }
        return aux;
    }
    bool canYouEvenMove(int m)
    {
        if (CheckIfHasBarrera() && (m == 6 || m == 7)) return misFichas[0].canMoveBarrera(m, exitPos, misFichas) || misFichas[1].canMoveBarrera(m, exitPos, misFichas)
            || misFichas[2].canMoveBarrera(m, exitPos, misFichas) || misFichas[3].canMoveBarrera(m, exitPos, misFichas);
        else return misFichas[0].canMove(m, exitPos) || misFichas[1].canMove(m, exitPos) || misFichas[2].canMove(m, exitPos) || misFichas[3].canMove(m, exitPos);
    }   
    int doIHaveTwoMineInStart()
    {
        int sol = 0;        
        if (TableroManager.Instance._casillas[initPos].numFichas >=2)
        {
            sol++;
            if (TableroManager.Instance._casillas[initPos].fichas[0] == TableroManager.Instance._casillas[initPos].fichas[1] &&
                TableroManager.Instance._casillas[initPos].fichas[0] == mColor) sol++;
        }        
        return sol;
    }
    public bool tryDelete(int _p)
    {
        bool aux = false;
        int i = 0;
        while (!aux && i < misFichas.Length)
        {
            if (misFichas[i].pos == _p)
            {
                aux = true;
                if (misFichas[i].ocupPos1) TableroManager.Instance._casillas[i].cas1Ocu = false;
                KillFicha(_p);
            }
            else i++;
        }

        return aux;
    }
    private void setColorsBotones(int mov)
    {        
        int one = 0;
        int two = 0;
        int three = 0;
        int fourth = 0;

        if (CheckIfHasBarrera())
        {
            if (misFichas[0].pos == 108) one = 2;
            else if (!misFichas[0].canMoveBarrera(mov, exitPos, misFichas)) one = 1;

            if (misFichas[1].pos == 108) two = 2;
            else if (!misFichas[1].canMoveBarrera(mov, exitPos, misFichas)) two = 1;

            if (misFichas[2].pos == 108) three = 2;
            else if (!misFichas[2].canMoveBarrera(mov, exitPos, misFichas)) three = 1;

            if (misFichas[3].pos == 108) fourth = 2;
            else if (!misFichas[3].canMoveBarrera(mov, exitPos, misFichas)) fourth = 1;
        }
        else
        {
            if (misFichas[0].pos == 108) one = 2;
            else if (!misFichas[0].canMove(mov, exitPos)) one = 1;

            if (misFichas[1].pos == 108) two = 2;
            else if (!misFichas[1].canMove(mov, exitPos)) two = 1;

            if (misFichas[2].pos == 108) three = 2;
            else if (!misFichas[2].canMove(mov, exitPos)) three = 1;

            if (misFichas[3].pos == 108) fourth = 2;
            else if (!misFichas[3].canMove(mov, exitPos)) fourth = 1;
        }

            GameManager.Instance.setButtonColors(one, two, three, fourth);
    }
    public bool hasWon()
    {
        return misFichas[0].pos == 108 && misFichas[1].pos == 108 && misFichas[2].pos == 108 && misFichas[3].pos == 108;
    }

    public int NumFichasActivas()
    {
        int a = 0;
        for(int i = 0; i < misFichas.Length; i++)
        {
            if (misFichas[i].pos >= 0 && misFichas[i].pos < 100) a++;
        }
        return a;
    }

    public int NumFichasPos(int pos)
    {
        int a = 0;
        for (int i = 0; i < misFichas.Length; i++)
        {
            if (misFichas[i].pos == pos) a++;
        }
        return a;
    }
}