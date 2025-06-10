using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    #region Instance
    static private AIManager _instance;
    static public AIManager Instance { get { return _instance; } }
    void Awake()
    {
        _instance = GetComponent<AIManager>();
    }
    #endregion

    Player[] mPlayers;
    public void getPlayers(Player[] a)
    {
        mPlayers = a;
    }

    public void MoveMe(int mov)
    {        
        int jugAct = GameManager.Instance.getActPlayer();

        bool[] cuales = new bool[mPlayers[jugAct].misFichas.Length];
        int numJugables = 0;
        for(int i = 0; i < mPlayers[jugAct].misFichas.Length; i++)
        {
            if((mov == 6 || mov == 7) && mPlayers[jugAct].CheckIfHasBarrera())
            {
                if (mPlayers[jugAct].misFichas[i].canMoveBarrera(mov, mPlayers[jugAct].exitPos, mPlayers[jugAct].misFichas))
                {
                    cuales[i] = true;
                    numJugables++;
                } 
                else cuales[i] = false;
            }
            else
            {
                if (mPlayers[jugAct].misFichas[i].canMove(mov, mPlayers[jugAct].exitPos))
                {
                    cuales[i] = true;
                    numJugables++;
                }
                else cuales[i] = false;
            }
        }

        if(numJugables == 0)
        {
            GameManager.Instance.WaitThenMove(false);
        }
        else
        {
            if (numJugables == 1)
            {
                for(int i = 0; i < cuales.Length; i++)
                {
                    if (cuales[i]) mPlayers[jugAct].ManualMovement(i);
                }
            }
            else
            {
                int[] fichs = new int[cuales.Length];
                for(int k = 0; k < cuales.Length; k++)
                {
                    if (cuales[k])
                    {
                        fichs[k] = calculate(k, mov);
                    }
                    else fichs[k] = -99999999;
                }
                int max = -99999999;
                int posMax = 0;
                bool pareja = false;

                for(int i = 0; i < fichs.Length; i++)
                {
                    if(max == fichs[i]) pareja = true;
                    else
                    {
                        if(max < fichs[i])
                        {
                            max = fichs[i];
                            posMax = i;
                            pareja = false;
                        }
                    }
                }                
                if (pareja)
                {                    
                    for(int y = 0; y < fichs.Length; y++)
                    {
                        if (fichs[y] == max)
                        {
                            cuales[y] = true;
                        }
                        else cuales[y] = false;
                    }
                    max = -999999999;                    
                    if(posMax > 0)
                    {
                        for (int y = 0; y < fichs.Length; y++)
                        {                            
                            if (cuales[y])
                            {
                                int aux = mPlayers[jugAct].misFichas[y].pos;
                                if (aux < 100 && aux > mPlayers[jugAct].exitPos) aux -= 63;
                                if (max < fichs[y] * aux)
                                {
                                    max = fichs[y] * aux;
                                    posMax = y;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int y = 0; y < fichs.Length; y++)
                        {
                            if (cuales[y])
                            {
                                int aux = mPlayers[jugAct].misFichas[y].pos;
                                if (aux < 100 && aux > mPlayers[jugAct].exitPos) aux -= 63;
                                aux = aux * -1;
                                if (max < fichs[y] * aux)
                                {
                                    max = fichs[y] * aux;
                                    posMax = y;
                                }
                            }
                        }                        
                    }
                }                
                mPlayers[jugAct].ManualMovement(posMax);
            }
        } 
    }

    int calculate(int ficha, int mov)
    {
        int result;

        int movFinal = mPlayers[GameManager.Instance.getActPlayer()].misFichas[ficha].pos + mov;
        if (movFinal > mPlayers[GameManager.Instance.getActPlayer()].exitPos && movFinal - mov <= mPlayers[GameManager.Instance.getActPlayer()].exitPos)
            movFinal = movFinal - mPlayers[GameManager.Instance.getActPlayer()].exitPos + 100;
        result = valuePos(movFinal, ficha) - valuePos(mPlayers[GameManager.Instance.getActPlayer()].misFichas[ficha].pos, ficha);
        return result;
    }

    int valuePos(int pos, int numfich, bool avoidRepetition = false)
    {        
        int jugAct = GameManager.Instance.getActPlayer();
        int value = 0;        

        if (pos >= 100)
        {
            if(isRisky()) value += 2;
            else value += 4;
            if (pos == 108)
            {
                value += 2;
                if (!avoidRepetition)
                {
                    value += CheckExtraMovement(10, numfich, pos) * 2;
                }
            }
        }
        else
        {
            if (pos >= 68) pos -= 68;
            if (TableroManager.Instance._casillas[pos].safe)
            {
                if (isRisky()) value += 2;
                else value += 3;
                int aux = 0;
                for (int i = 0; i < mPlayers[jugAct].misFichas.Length; i++)
                {
                    if (i != numfich && mPlayers[jugAct].misFichas[i].pos == pos) aux++;
                }
                if (aux >= 1)
                {
                    value += 3;
                    if (pos == mPlayers[jugAct].initPos && !mPlayers[jugAct].CheckAllOut()) 
                    {
                        if (isRisky()) value -= 1;
                        else value -= 4;                        
                    } 
                }
                

                for(int x = 0; x < mPlayers.Length; x++)
                {
                    if(x != jugAct)
                    {
                        if (pos == mPlayers[x].initPos)
                        {
                            if (!mPlayers[x].CheckAllOut())
                            {
                                value -= 1;
                                for (int k = 0; k < mPlayers[x].misFichas.Length; k++)
                                {
                                    if (mPlayers[x].misFichas[k].pos == pos)
                                    {
                                        if (isRisky()) value -= 2;
                                        else value -= 3;
                                    }
                                }
                            }
                        }
                    }
                }                
            }
            else
            {                
                if (mPlayers[jugAct].misFichas[numfich].pos != pos)
                {
                    if (!TableroManager.Instance._casillas[pos].safe && TableroManager.Instance._casillas[pos].numFichas > 0)
                    {
                        if ((TableroManager.Instance._casillas[pos].fichas[0] != mPlayers[jugAct].mColor && TableroManager.Instance._casillas[pos].fichas[0] != TableroManager.Col.None)
                            || (TableroManager.Instance._casillas[pos].fichas[1] != mPlayers[jugAct].mColor && TableroManager.Instance._casillas[pos].fichas[1] != TableroManager.Col.None))
                        {
                            if (isRisky()) value += 10;
                            else value += 3;
                            if (!avoidRepetition)
                            {
                                value += CheckExtraMovement(20, numfich, pos);
                            }
                        }
                    }
                }

                for (int i = 1; i <= 12; i++)
                {
                    int i2 = pos - i;
                    if (i2 < 0) i2 += 68;
                    if (TableroManager.Instance._casillas[i2].numFichas > 0)
                    {
                        if ((TableroManager.Instance._casillas[i2].fichas[0] != mPlayers[jugAct].mColor && TableroManager.Instance._casillas[i2].fichas[0] != TableroManager.Col.None)
                            || (TableroManager.Instance._casillas[i2].fichas[1] != mPlayers[jugAct].mColor && TableroManager.Instance._casillas[i2].fichas[1] != TableroManager.Col.None))
                        {
                            if (isRisky())
                            {
                                if (i < 7) value -= 1;
                            }
                            else
                            {
                                if (i < 7) value -= 3;
                                else value -= 1;
                            }
                        }
                    }
                }
            }
            for(int i = 1; i <= 7; i++)
            {
                int i2 = pos + i;
                if (i2 >= 68) i2 -= 68;
                if (!TableroManager.Instance._casillas[i2].safe && TableroManager.Instance._casillas[i2].numFichas > 0)
                {
                    if ((TableroManager.Instance._casillas[i2].fichas[0] != mPlayers[jugAct].mColor && TableroManager.Instance._casillas[i2].fichas[0] != TableroManager.Col.None)
                        || (TableroManager.Instance._casillas[i2].fichas[1] != mPlayers[jugAct].mColor && TableroManager.Instance._casillas[i2].fichas[1] != TableroManager.Col.None))
                    {
                        if (isRisky()) value += 2;
                        else value += 1;
                    }
                }
            }
        }
        return value;
    }

    bool isRisky()
    {
        float probability = mPlayers[GameManager.Instance.getActPlayer()].risk;
        float prob2 = Random.Range(0.0f, 1.0f);
        return probability > prob2;
    }

    int CheckExtraMovement(int extraMov, int fich, int pos)
    {
        int response = -10;
        for(int i = 0; i < mPlayers[GameManager.Instance.getActPlayer()].misFichas.Length; i++)
        {
            if (mPlayers[GameManager.Instance.getActPlayer()].misFichas[i].canMove(extraMov, mPlayers[GameManager.Instance.getActPlayer()].exitPos))
            {
                if(i == fich)
                {
                    int a = valuePos(pos + extraMov, i, true);
                    if (a > response) response = a;
                }
                else
                {
                    int a = valuePos(mPlayers[GameManager.Instance.getActPlayer()].misFichas[i].pos + extraMov, i, true);
                    if(a > response) response = a;
                }
            }
        }        
        return response;
    }
}
