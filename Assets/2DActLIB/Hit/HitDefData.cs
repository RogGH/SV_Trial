using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDefData : HitData
{
    [SerializeField] HitLayer defLayer;                      // レイヤー
    [SerializeField] HitElement defEle = HitElement.None;    // 属性（ダメージ計算用属性）
    [SerializeField] HitDefKind defKind;                     // 種類
    [SerializeField] HitDefPlus defPlus;                    // 防御付加
    [SerializeField] HitShape defShape = HitShape.Rect;     // 形状（現状矩形のみ）
    [SerializeField] int defPow;                           // 防御力（防御力）

    private List<GameObject> defList =  new List<GameObject>();

    public HitElement DefEle { get { return defEle; } }
    public HitDefKind DefKind { get { return defKind; } }
    public HitDefPlus DefPlus { get { return defPlus; } }
    public int DefPow { get { return defPow; } set { defPow = value; } }

    BoxCollider2D box;
    HitBase hb;

    void Awake(){
        // 親設定
        this.Type = HitType.DEF;
        this.Layer = defLayer;

        // ヒットベース取得
        hb = HitDefine.getHitBase(this.gameObject);
        // 防御のみ
        defList.Clear();

#pragma warning disable CS0162
        if (HitDefine.DebugDisp){
            box = GetComponent<BoxCollider2D>();
        }
#pragma warning disable CS0162
    }

    void Update()
    {
        // 表示
        if (HitDefine.DebugDisp){
            if (hb.DefActive){
                if (defShape == HitShape.Rect){
                    HitDefine.RectDisp(transform.root.localScale.x,
                        box.offset,
                        box.size * transform.root.localScale,
                        transform.root.position,
                        Color.green);
                }
            }
        }
    }
}
