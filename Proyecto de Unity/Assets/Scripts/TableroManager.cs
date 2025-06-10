using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableroManager : MonoBehaviour
{
    #region Instance
    static private TableroManager _instance;
    static public TableroManager Instance { get { return _instance; } }
    void Awake()
    {
        _instance = GetComponent<TableroManager>();
    }
    #endregion
    public enum Col { None, Rojo, Verde, Amarillo, Azul}
    public struct Casilla
    {
        public int num;
        public Transform t;
        public bool safe;
        public Col start;
        public Col end;
        public int numFichas;
        public Col[] fichas;
        public bool cas1Ocu;
    };
    public struct Ficha
    {
        public int pos;
        public Transform t;
        public Col mCol;
        public bool ocupPos1;

        public void setToPos()
        {
            int aux = 0;
            TableroManager.Instance._casillas[pos].numFichas++;
            if (TableroManager.Instance._casillas[pos].fichas[0] == Col.None) TableroManager.Instance._casillas[pos].fichas[0] = mCol;
            else if (TableroManager.Instance._casillas[pos].fichas[1] == Col.None) TableroManager.Instance._casillas[pos].fichas[1] = mCol;
            if (TableroManager.Instance._casillas[pos].cas1Ocu)
            {
                ocupPos1 = false;
                aux++;
            }
            else
            {
                ocupPos1 = true;
                TableroManager.Instance._casillas[pos].cas1Ocu = true;
            }
            t.position = TableroManager.Instance.getPosCas(pos) + (new Vector3(10, 0, 0) * aux);
        }

        public void Move(int x,int ext, Transform[] EndSpaces)
        {            
            if (pos > 100)
            {
                int y = pos - 101 + x;
                t.position = EndSpaces[y].position;
                pos += x;
            }
            else
            {
                int cuantoSeHaPasado = -1;
                TableroManager.Instance._casillas[pos].numFichas--;
                if (TableroManager.Instance._casillas[pos].fichas[0] == mCol) TableroManager.Instance._casillas[pos].fichas[0] = Col.None;
                else if (TableroManager.Instance._casillas[pos].fichas[1] == mCol) TableroManager.Instance._casillas[pos].fichas[1] = Col.None;
                if(ocupPos1) TableroManager.Instance._casillas[pos].cas1Ocu = false;
                int aux = pos + x;
                if (aux > ext && aux - x <= ext) cuantoSeHaPasado = aux - ext;                
                if (aux >= 68) aux -= 68;
                if (cuantoSeHaPasado != -1)
                {
                    pos = 100 + cuantoSeHaPasado;
                    t.position = EndSpaces[cuantoSeHaPasado - 1].position;
                }
                else
                {
                    if (TableroManager.Instance._casillas[aux].numFichas > 0 && !TableroManager.Instance._casillas[aux].safe)
                    {                        
                        TableroManager.Instance.tryDelete(aux, mCol);                        
                    }
                    pos = aux;
                    setToPos();
                }
            }
            if(pos == 108) GameManager.Instance.addExtraMovement(10);            
        }

        public bool canMove(int x, int ext)
        {
            if(pos <0) return false;
            else
            {
                if(pos > 100)
                {
                    int i = pos - 100;
                    if(i+x <= 8) return true;
                    else return false;
                }
                else
                {
                    int i = pos + x;
                    if (pos <= ext && i > ext + 8) return false;
                    else
                    {
                        if (i >= 68) i = i - 68;
                        if (TableroManager.Instance._casillas[i].numFichas >= 2) return false;
                        else
                        {
                            if (TableroManager.Instance.hayBloqueo(pos, pos + x)) return false;
                            else return true;
                        }
                    }
                }
            }
        }
        public bool canMoveBarrera(int x, int ext, Ficha[] posOthers)
        {
            if (pos < 0) return false;
            else
            {
                if(x == 6 || x == 7)
                {
                    bool aux = false;
                    for(int i = 0; i < posOthers.Length && !aux; i++)
                    {
                        if(t != posOthers[i].t)
                        {
                            aux = pos == posOthers[i].pos;
                        }
                    }
                    if (aux)
                    {
                        return canMove(x, ext);
                    }
                    else return false;
                }
                else
                {
                    return canMove(x, ext);
                }                
            }
        }
    }
    public Casilla[] _casillas = new Casilla[68];
    private Player[] _jugs = new Player[4];
    public void AddPlayer(Player p)
    {
        int i = 0;
        bool aux = true;
        while(aux && i < 4)
        {
            if (_jugs[i] == null) aux = false;
            else i++;
        }        
        if(!aux) _jugs[i] = p;
    }

    public void getCasilla(int _num, bool _safe, Transform _t, Col _start, Col _end)
    {
        _casillas[_num].num = _num;
        _casillas[_num].safe = _safe;
        _casillas[_num].t = _t;
        _casillas[_num].start = _start;
        _casillas[_num].end = _end;
        _casillas[_num].numFichas = 0;
        _casillas[_num].fichas = new Col[2];
        _casillas[_num].cas1Ocu = false;
    }
    public int initPos(Col c)
    {
        if (c == Col.None) return -1;
        else
        {
            bool aux = false;
            int i = 0;
            while (!aux && i < _casillas.Length)
            {                
                if (_casillas[i].start == c) aux = true;
                else i++;
            }
            if (aux) return i;
            else return -1;
        }
    }
    public int exitPos(Col c)
    {
        if (c == Col.None) return -1;
        else
        {
            bool aux = false;
            int i = 0;
            while (!aux && i < _casillas.Length)
            {
                if (_casillas[i].end == c) aux = true;
                else i++;
            }
            if (aux) return i;
            else return -1;
        }
    }
    public Vector3 getPosCas(int a)
    {        
        return _casillas[a].t.position;
    }
    public bool hayBloqueo(int inicio, int final)
    {
        bool aux = false;
        int x = inicio + 1;
        while(x<=final && !aux)
        {
            int i = x;
            if (i >= 68) i = i - 68;

            if (_casillas[i].numFichas >= 2 && _casillas[i].safe && _casillas[i].fichas[0] == _casillas[i].fichas[1]) aux = true;
            else x++;
        }
        return aux;
    }

    public bool tryDelete(int pos, Col c = Col.None)
    {
        bool aux = false;        
        if(_jugs[0].mColor != c)
        {
            aux = _jugs[0].tryDelete(pos);
        }
        if (!aux)
        {
            if (_jugs[1].mColor != c)
            {
                aux = _jugs[1].tryDelete(pos);
            }
            if (!aux)
            {
                if (_jugs[2].mColor != c)
                {
                    aux = _jugs[2].tryDelete(pos);
                }
                if (!aux)
                {
                    if (_jugs[3].mColor != c)
                    {
                        aux = _jugs[3].tryDelete(pos);
                    }                    
                }

            }
        }
        if (aux)
        {
            GameManager.Instance.addExtraMovement(20);
            _casillas[pos].numFichas--;
        }        
        return aux;
    }    
    public void AreTherePhantomFichas()
    {
        int numTab = 0;
        for(int i = 0; i < _casillas.Length; i++)
        {
            numTab += _casillas[i].numFichas;
        }

        int numJugs = 0;
        for(int j = 0; j < _jugs.Length; j++)
        {
            numJugs += _jugs[j].NumFichasActivas();
        }
        
        if (numJugs < numTab)
        {            
            DeletePhantomFichas();
        }
    }

    private void DeletePhantomFichas()
    {
        for (int i = 0; i < _casillas.Length; i++)
        {
            if (_casillas[i].numFichas > 0)
            {
                int numReal = 0;
                for (int j = 0; j < _jugs.Length; j++)
                {
                    numReal += _jugs[j].NumFichasPos(i);
                }
                _casillas[i].numFichas = numReal;
            }
        }
    }
}
