using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Instance
    static private GameManager _instance;
    static public GameManager Instance { get { return _instance; } }
    void Awake()
    {
        _instance = GetComponent<GameManager>();
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion
    #region references
    private InitManager _InitMan;
    public void setInitMan(InitManager i)
    {
        _InitMan = i;
    }
    private RollGenerator _Roller;
    public void setRollGenerator(RollGenerator r)
    {
        _Roller = r;
    }
    TurnColor _turnCol;
    public void setTurnColor(TurnColor t)
    {
        _turnCol = t;
    }
    ButtonAppearanceManager _butAppMan;
    public void setButtAppMan(ButtonAppearanceManager t)
    {
        _butAppMan = t;
    }
    #endregion
    public enum Jug { Player, IA };
    Jug roj = Jug.Player;
    Jug verd = Jug.Player;
    Jug amar = Jug.Player;
    Jug azul = Jug.Player;
    float rojRisk;
    float verdRisk;
    float amarRisk;
    float azulRisk;
    float waitingTime;
    Player[] mPlayers = new Player[4];
    int actPlayer = 0;
    public int getActPlayer() { return actPlayer; }
    int extrMov = 0;

    public void comence(Jug _r, float _rR, Jug _v, float _vR, Jug _am, float _amR, Jug _az, float _azR, float waitingTim)
    {
        roj = _r;
        verd = _v;
        amar = _am;
        azul = _az;
        rojRisk = _rR;
        verdRisk = _vR;
        amarRisk = _amR;
        azulRisk = _azR;
        waitingTime = waitingTim;
    }
    public void init(Player _roj, Player _verd, Player _ama, Player _az)
    {
        mPlayers[0] = _roj;
        _roj.initAtributes(roj == Jug.IA, rojRisk);
        mPlayers[1] = _verd;
        _verd.initAtributes(verd == Jug.IA, verdRisk);
        mPlayers[2] = _ama;
        _ama.initAtributes(amar == Jug.IA, amarRisk);
        mPlayers[3] = _az;
        _az.initAtributes(azul == Jug.IA, azulRisk);

        AIManager.Instance.getPlayers(mPlayers);

        _turnCol.Change(mPlayers[actPlayer].mColor);
        mPlayers[actPlayer].Play();
    }    

    public int Roll()
    {
        return _Roller.Roll();
    }
    public void PlayerMoveFicha(int num)
    {
        mPlayers[actPlayer].ManualMovement(num);
    }
    public void NextMove(bool repeat)
    {
        if (mPlayers[actPlayer].hasWon())
        {
            _turnCol.Victory(mPlayers[actPlayer].mColor);
        }
        else
        {
            if (extrMov <= 0)
            {
                if (!repeat) actPlayer++;
                if (actPlayer >= mPlayers.Length) actPlayer = 0;
                _turnCol.Change(mPlayers[actPlayer].mColor);
                mPlayers[actPlayer].Play();
            }
            else
            {
                mPlayers[actPlayer].Play(extrMov);
                extrMov = 0;
            }
        }
    }
    IEnumerator nexMovTimer(bool repeat)
    {
        yield return new WaitForSeconds(waitingTime * 1.25f);
        NextMove(repeat);
    }

    IEnumerator nexMovTimerIA(bool repeat)
    {
        yield return new WaitForSeconds(waitingTime);
        NextMove(repeat);
    }

    public void WaitThenMove(bool repeat)
    {
        StartCoroutine(nexMovTimer(repeat));
    }
    public void WaitThenMoveIA(bool repeat)
    {
        StartCoroutine(nexMovTimerIA(repeat));
    }
    public void setUI7(int i = 7)
    {
        _Roller.setTo7(i);
    }
    public void addExtraMovement(int a)
    {
        extrMov = a;
    }
    public void setButtonColors(int one, int two, int three, int fourth)
    {
        _butAppMan.setColors(one, two, three, fourth);
    }
    public void destruction()
    {
        _instance = null;
        Destroy(gameObject);
    }
}
