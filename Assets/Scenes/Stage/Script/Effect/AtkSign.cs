using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkSign : MonoBehaviour
{
    public Sprite[] SpTbl;

    public enum SignType { 
        Circle,
        Sec45,
        Sec60,
        Sec90,
        Rect,
    };
    public enum ShellType {
        Def,
        Bomb,
        Gaze,
        Eff,
    };
    public SignType SType; 

    // 設定サイズ
    public float SetSize = 60;
    public float DelCount = 3.0f;

    bool masterFollowFlag = true;       // 親の座標に追従するか
    bool masterDieFlag = true;          // 親が死んだら死ぬか

    // 基本サイズ
    float defSize = 60;
    float rectSize = 40;

    float scaleRate;
    float widthRate;
    float heightRate;

    float scaleSpd;
    float scaleCount = 0.2f;

    GameObject master;
    GameObject shell = null;
    Vector3 tagOfs;
    ShellType shellType;

    
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = SpTbl[(int)SType];

        if (SType == SignType.Rect)
        {
            scaleRate = SetSize / rectSize;
        }
        else {
            scaleRate = SetSize / defSize;
        }
        scaleSpd = scaleRate / scaleCount;

        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        // マスターが死んだら死ぬか
        if (masterDieFlag) {
            if (master == null){
                Destroy(gameObject);
                return;
            }
        }

        // 徐々に拡大
        if ( scaleCount > 0) {
            scaleCount -= Time.deltaTime;

            Vector3 setScl = transform.localScale;

            setScl.x += scaleSpd * Time.deltaTime;
            setScl.y += scaleSpd * Time.deltaTime;

            if (SType == SignType.Rect)
            {
                // 矩形は幅高さで
                if (setScl.x > widthRate) { setScl.x = widthRate; }
                if (setScl.y > heightRate) { setScl.y = heightRate; }
            }
            else {
                // 他はサイズから
                if (setScl.x > scaleRate) { setScl.x = scaleRate; }
                if (setScl.y > scaleRate) { setScl.y = scaleRate; }
            }

            setScl.z = 1;

            transform.localScale = setScl;
        }

        // 時間で消す
        DelCount -= Time.deltaTime;
        if ( DelCount <= 0 ) {
            // 死ぬときにシェルを起動するタイプ
            if (shell) {
                GameObject obj;

                // 目線用パラメータ設定
                switch (shellType) {
                    case ShellType.Def:
                        obj = Instantiate(shell, transform.position, Quaternion.identity);
                        obj.GetComponent<EShellDef>().SetUp(SetSize);
                        break;
                    case ShellType.Bomb:
                        obj = Instantiate(shell, transform.position, Quaternion.identity);
                        obj.GetComponent<EShellBomb>().SetUp(SetSize);
                        break;
                    case ShellType.Gaze:
                        obj = Instantiate(shell, transform.position, transform.rotation);
                        obj.GetComponent<EShellGaze>().SetUp(master, SetSize, tagOfs);
                        break;
                    case ShellType.Eff:
                        obj = Instantiate(shell, transform.position, Quaternion.identity);
                        obj.GetComponent<EffDef>().SetUp(SetSize);
                        break;
                }
            }
            Destroy(gameObject);
        }

        // 親に座標を追従するかチェック
        if (masterFollowFlag)
        {
            if (master != null)
            {
                transform.position = master.transform.position;
                transform.position += tagOfs;
            }
        }
    }

    // 引数：親オブジェクト、オフセット、サイズ、表示時間
    public void SetUp(GameObject mas, SignType sType, Vector3 ofs, float size, float count, GameObject shlObj, ShellType shlType = ShellType.Bomb) {
        master = mas;
        masterDieFlag = true;
        masterFollowFlag = true;
        tagOfs = ofs;

        SType = sType;
        SetSize = size;
        DelCount = count;
        shell = shlObj;
        shellType = shlType;

        // 扇形の場合は、PLの方向に合わせる
        if ( sType != SignType.Circle )
        {
            GameObject pl = StageManager.Ins.PlObj;
            // 角度を設定
            float radian = Mathf.Atan2(master.transform.position.y - pl.transform.position.y,
                master.transform.position.x - pl.transform.position.x);
            Vector3 rotate = transform.localEulerAngles;
            rotate.z = radian * Mathf.Rad2Deg - 90;
            transform.localEulerAngles = rotate;
        }
    }

    // 予兆、親追従無し
    public void SetUpSignOnly(GameObject mas, SignType sType, float size, float count, GameObject shlObj = null, ShellType shlType = ShellType.Bomb) {
        master = mas;
        masterDieFlag = true;
        masterFollowFlag = false;

        SType = sType;
        SetSize = size;
        DelCount = count;
        shell = shlObj;
        shellType = shlType;
    }

    // 直線予兆
    public void SetUpRect(GameObject mas, float size, float count, float deg) {
        master = mas;
        masterDieFlag = true;
        masterFollowFlag = true;

        SType = SignType.Rect;
        DelCount = count;
        SetSize = size;

        widthRate = SetSize / rectSize;
        heightRate = 3.5f;

        shell = null;
        {
            Vector3 rotate = transform.localEulerAngles;
            rotate.z = deg;
            transform.localEulerAngles = rotate;
            transform.localScale = Vector3.zero;
        }
    }
}
