using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StageManager : MonoBehaviour
{
    public bool DebugMode = false;

    private static StageManager ins;
    public static StageManager Ins{
        get{ 
            if (ins == null) {
                ins = (StageManager)FindObjectOfType(typeof(StageManager));
                if (ins == null) { Debug.LogError(typeof(StageManager) + "is nothing"); }
            }
            return ins;
        }
    }

    static GameObject plObj = null;
    public GameObject PlObj { get { return plObj; } }

    static Player plScr = null;
    public Player PlScr { get { return plScr; } }

    static HitBase plHB = null;
    public HitBase PlHB { get { return plHB; } }

    [SerializeField]float stageTime = 0;
    public float StageTime{
        get{ return stageTime; }
        set{ stageTime = value; }
    }

    public float GameSpd = 1.0f;

    // UI関連
    public GameObject PauseUI;
    public GameObject LevelUpUI;
    public GameObject BaseUI;
    public GameObject TresureUI;
    public GameObject GameOverUI;
    public GameObject ClearUI;
    // Player
    public GameObject CamObj;
    // 
    private bool pauseFlag = false;
    private bool levelupFlag = false;
    private bool tresureFlag = false;
    private bool forceEscapeFlag = false;
    private bool gameoverFlag = false;
    private bool clearFlag = false;

    void Awake()
    {
        //シングルトンのためのコード
        if (this != Ins){ Destroy(this.gameObject); return; }

        // PL起動
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0 )
        {
            plObj = players[0];
            Instantiate((GameObject)Resources.Load("Prefabs/Effect/PLDireIcon"));

            CamObj.GetComponent<CinemachineVirtualCamera>().Follow = plObj.transform;

            plScr = plObj.GetComponent<Player>();
            plHB = plObj.GetComponent<HitBase>();
            plScr.Money = SystemManager.Ins.sData.money;
        }

        BaseUI.transform.SetAsFirstSibling();
        BaseUI.SetActive(true);

        BgmManager.Instance.Play("Stage1");
    }

    void Update()
    {
        if (CheckStop())
        {
            Time.timeScale = 0;
            if (pauseFlag)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    pauseFlag = !pauseFlag;
                }
            }
        }
        else
        {
            stageTime += Time.deltaTime * GameSpd;
            Time.timeScale = 1.0f;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseFlag = !pauseFlag;
                if (pauseFlag) { SetPause(); }
                SeManager.Instance.Play("Button1");
            }
        }

        if (DebugMode) {
            // 強制終了フラグ管理
            if (Input.GetKeyDown(KeyCode.P))
            {
                forceEscapeFlag = !forceEscapeFlag;
            }
        }

        if (!pauseFlag) { PauseUI.SetActive(false); }
        if (!levelupFlag){ LevelUpUI.SetActive(false); }
        if (!tresureFlag) { TresureUI.SetActive(false); }
        if (!gameoverFlag) { GameOverUI.SetActive(false); }
        if (!clearFlag) { ClearUI.SetActive(false); }
    }


    public bool CheckStop() {
        return pauseFlag | levelupFlag | tresureFlag | gameoverFlag | clearFlag;
    }

    public void SetPause()
    {
        pauseFlag = true;
        PauseUI.SetActive(true);
    }

    public void SetLevelUp()
    {
        levelupFlag = true;
        LevelUpUI.GetComponent<LevelUpManager>().SetUp();
    }
    public void EndLevelUp()
    {
        levelupFlag = false;
    }
    public void SetTresure()
    {
        tresureFlag = true;
        TresureUI.GetComponent<TresureManager>().SetUp();
    }
    public void EndTresure()
    {
        tresureFlag = false;
    }

    public void SetGameOver() {
        gameoverFlag = true;
        GameOverUI.GetComponent<GameOverManager>().SetUp();
    }

    public void SetClear() {
        clearFlag = true;
        ClearUI.GetComponent<ClearManager>().SetUp();
        SeManager.Instance.Play("Clear");
    }


    public void Save() {
        SystemManager.Ins.sData.money = plScr.Money;
        SystemManager.Ins.SaveSystemData();
    }

    public bool CheckForceEscape() { return forceEscapeFlag; }
}
