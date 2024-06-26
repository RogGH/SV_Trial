using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EShellSand : MonoBehaviour
{
    SpriteRenderer spComp;

    void Start()
    {
        spComp = GetComponent<SpriteRenderer>();
        SeManager.Instance.Play("Sand");
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

}
