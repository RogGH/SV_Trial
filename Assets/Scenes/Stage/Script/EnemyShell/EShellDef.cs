using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EShellDef : MonoBehaviour
{
    SpriteRenderer spComp;
    [SerializeField] float defSize = 0;
    float scaleRate;
    public string SeName;

    void Start()
    {
        spComp = GetComponent<SpriteRenderer>();
        if (SeName != "") {
            SeManager.Instance.Play(SeName);
        }
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

    public void SetUp(float size) {
        scaleRate = size / defSize;
        transform.localScale *= scaleRate;
    }
}
