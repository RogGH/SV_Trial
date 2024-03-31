using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffDef : MonoBehaviour
{
    SpriteRenderer spComp;
    [SerializeField] float defSize = 0;
    float scaleRate;
    public string SeName;

    void Start()
    {
        spComp = GetComponent<SpriteRenderer>();
        if (SeName != "")
        {
            SeManager.Instance.Play(SeName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 現在の座標から、表示優先を決定させてみる
        Vector3 pPos = transform.position;
        pPos.y += -50.0f;       // 若干手前に映るように座標を修正
        spComp.sortingOrder = IObject.GetSpriteOrder(pPos);
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
