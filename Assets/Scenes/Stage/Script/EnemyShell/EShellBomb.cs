using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EShellBomb : MonoBehaviour
{
    float defSize = 100;
    float scaleRate;
    SpriteRenderer spComp;

    void Start()
    {
        spComp = GetComponent<SpriteRenderer>();
        SeManager.Instance.Play("Bomb2");
    }

    // Update is called once per frame
    void Update()
    {
        // 現在の座標から、表示優先を決定させてみる
        spComp.sortingOrder = IObject.GetSpriteOrder(transform.position);
    }

    public void HitEnd()
    {
        GetComponent<HitBase>().SetAtkActive(false);
    }

    public void AnimEnd()
    {
        Destroy(gameObject);
    }

    public void SetUp(float size)
    {
        scaleRate = size / defSize;
        transform.localScale *= scaleRate;
    }
}
