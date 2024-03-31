using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EShellGaze : MonoBehaviour
{
    HitBase hb;
    GameObject master;
    Vector3 tagOfs;
    SpriteRenderer spComp;

    // 基本サイズ
    public float SetSize = 100;
    float defSize = 160;
    float scaleRate;

    void Start()
    {
        transform.localScale *= scaleRate;
        hb = GetComponent<HitBase>();
        spComp = GetComponent<SpriteRenderer>();
        SeManager.Instance.Play("Beam2");
    }

    // Update is called once per frame
    void Update()
    {
        if (master == null)
        {
            Destroy(gameObject);
            return;
        }

        if (master != null)
        {
            transform.position = master.transform.position;
            transform.position += tagOfs;
        }
 
           // 現在の座標から、表示優先を決定させてみる
        spComp.sortingOrder = IObject.GetSpriteOrder(transform.position);
 }

    void HitEnd()
    {
        hb.SetAtkActive(false);
    }


    void AnimEnd() {
        Destroy(gameObject);
    }

    // 引数：親オブジェクト、オフセット、サイズ、表示時間
    public void SetUp(GameObject mas, float size, Vector3 ofs)
    {
        master = mas;
        SetSize = size;

        scaleRate = SetSize / defSize;
        // 
        Vector3 rotate = transform.localEulerAngles;
        rotate.z -= 90.0f;
        transform.localEulerAngles = rotate;

        // 回転値が0の時にyが-40
        float length = -40.0f * scaleRate;
        float radian = (rotate.z + 90)* Mathf.Deg2Rad;

        ofs.x += length * Mathf.Cos(radian);
        ofs.y += length * Mathf.Sin(radian);

        tagOfs = ofs;
    }
}
