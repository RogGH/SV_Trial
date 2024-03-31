using UnityEngine;

public static class WeaponDefine
{
    public const float LiveCountDef = 10.0f;

    public const float CamOutOfs = 40.0f;

    public const int LevelMax = 7;
    public const int EquipMax = 6;

    // ‰æ–ÊŠO”»’è
    public static bool CameraOut(GameObject obj) {

        Vector3 pointLB = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 pointRU = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 1, Screen.height - 1, 0));

        pointLB.x += -CamOutOfs;
        pointLB.y += -CamOutOfs;

        pointRU.x += CamOutOfs;
        pointRU.y += CamOutOfs;

        Vector3 pos = obj.transform.position;

        if( pos.x < pointLB.x  || pointRU.x < pos.x 
        ||  pos.y < pointLB.y  || pointRU.y < pos.y 
        ){
            return true;
        }
        return false;
    }
};

