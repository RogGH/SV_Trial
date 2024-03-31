using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField]DropTblNo tblNo;

    HitBase hb;
    GameObject plObj;
    SpriteRenderer spComp;
    Player plScr;
    //bool plDieFlag = false;
    bool magnetFlag = false;
    float moveSpd = 2.0f * 60.0f;
    float add = 2.0f * 60.0f;
    const float MoveSpdMax = 8.0f * 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        hb = GetComponent<HitBase>();
        spComp = GetComponent<SpriteRenderer>();
        plScr = StageManager.Ins.PlScr;
        plObj = StageManager.Ins.PlObj;

        if (tblNo == DropTblNo.Tresure) {
            SeManager.Instance.Play("TresureApp");
        }
    }

    // Update is called once per frame
    void Update()
    {
        hb.PreUpdate();

        if (tblNo == DropTblNo.Tresure) {
            magnetFlag = false;
        }
        else {
            // PLの距離をチェック
            if (magnetFlag == false) { 
                if (plObj != null) {
                    if( !plScr.CheckDie()){
                        // プレイヤーとの距離を見る
                        Vector3 vec1 = plScr.GetCenterPos() - transform.position;
                        float length = plScr.CalcRateToValue(plScr.Magnet, plScr.MagnetRate);
                        if (vec1.magnitude < length){
                            magnetFlag = true;
                        }
                    }
                }
            }
        }

        if (magnetFlag) {
            // プレイヤーの方向へ向かう
            float radian =
                Mathf.Atan2(plScr.GetCenterPos().y - transform.position.y,
                            plScr.GetCenterPos().x - transform.position.x);
            Vector3 spd = Vector3.zero;

            spd.x = moveSpd * Mathf.Cos(radian);
            spd.y = moveSpd * Mathf.Sin(radian);

            moveSpd += add * Time.deltaTime;

            if (moveSpd >= MoveSpdMax) { moveSpd = MoveSpdMax; }

            Vector3 pos = transform.position;
            pos.x += (spd.x * Time.deltaTime) + add * Time.deltaTime * Time.deltaTime;
            pos.y += (spd.y * Time.deltaTime) + add * Time.deltaTime * Time.deltaTime;
            transform.position = pos;

            if (plScr.CheckDie())
            {
                magnetFlag = false;
            }
        }

        // 現在の座標から、表示優先を決定させてみる
        spComp.sortingOrder = IObject.GetSpriteOrder(transform.position);

        // 接触したときチェック
        if (hb.CheckAttackHit() && !plScr.CheckDie() ) {
            switch (tblNo) {
                case DropTblNo.ExpS:
                    plScr.AddExp(ItemDefine.AddExpS);
                    SeManager.Instance.Play("Exp");
                    Destroy(gameObject);
                    break;

                case DropTblNo.ExpM:
                    plScr.AddExp(ItemDefine.AddExpM);
                    SeManager.Instance.Play("Exp");
                    Destroy(gameObject);
                    break;

                case DropTblNo.ExpL:
                    plScr.AddExp(ItemDefine.AddExpL);
                    SeManager.Instance.Play("Exp");
                    Destroy(gameObject);
                    break;

                case DropTblNo.MoneyS:
                    plScr.AddMoney(ItemDefine.DropAddMoney);
                    SeManager.Instance.Play("Money");
                    Destroy(gameObject);
                    break;

                case DropTblNo.HealS:
                    plScr.HealHP(ItemDefine.PotionHealHP);
                    SeManager.Instance.Play("Heal");
                    Destroy(gameObject);
                    break;

                case DropTblNo.Tresure:
                    if ( !StageManager.Ins.CheckStop() ) {
                        StageManager.Ins.SetTresure();
                        Destroy(gameObject);
                    }
                    break;
            }
        }

        hb.PostUpdate();
    }
}
