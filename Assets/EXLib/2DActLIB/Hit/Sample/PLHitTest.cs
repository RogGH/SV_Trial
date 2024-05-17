using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLHitTest : MonoBehaviour
{
   HitBase hb;     // コンポーネント用変数
        
    // Start is called before the first frame update
    void Start()
    {
        hb = GetComponent<HitBase>();           // Hitbaseコンポーネント取得
        hb.Setup(Damage, Die);                  // HitBase初期化
    }

    // Update is called once per frame
    void Update()
    {
        if (hb.PreUpdate()) { return; }         // HitBaseアップデート前処理（死亡したらこれ以上行わない）

        // 左右移動
        Vector3 pos = transform.position;
        float dir = Input.GetAxis("Horizontal");
        pos.x += 0.1f * dir;
        transform.position = pos;
    }

    private void LateUpdate()
    {
        hb.PostUpdate();                        // HitBaseアップデート後処理
    }

    void Damage() {
        Debug.Log("ダメージ受けました");
        // 無敵点滅コルーチン起動
        this.StartCoroutine("DmgCoroutine");
    }

    IEnumerator DmgCoroutine()
    {
        hb.SetDefActive(false);                               // HitBaseの防御無効化

        int count = 10;
        while (count > 0){
            //透明にする
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.color = new Color(1, 1, 1, 0);
            //0.05秒待つ
            yield return new WaitForSeconds(0.05f);
            //元に戻す
            sr.color = new Color(1, 1, 1, 1);
            //0.05秒待つ
            yield return new WaitForSeconds(0.05f);
            count--;
        }

        hb.SetDefActive(true);                               // HitBaseの防御無効化
    }


    bool Die() {
        Debug.Log("死亡");
        Destroy(this.gameObject);
        return true;    // 処理終了
    }


    // 外部からコンポーネント取得
    public HitBase getHitBase() {
        return hb;
    }
}
