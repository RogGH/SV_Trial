using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public GameObject emObj;
    //public int poolSize = 10;
    private List<GameObject> pool;

    void Start()
    {
        InitPool();
    }

    void InitPool()
    {
        pool = new List<GameObject>();

        //for (int i = 0; i < poolSize; i++)
        //{
        //    GameObject obj = Instantiate(emObj);
        //    obj.SetActive(false);
        //    pool.Add(obj);
        //}
    }

    public GameObject GetObjFromPool(Vector2 pos)
	{
		foreach (GameObject obj in pool)
		{
            if (!obj.activeInHierarchy) {
                obj.transform.position = pos;
                obj.transform.rotation = Quaternion.identity;
                obj.SetActive(true);
                return obj;
            }
		}

        GameObject newObj = Instantiate(emObj, pos, Quaternion.identity);
        pool.Add(newObj);
        return newObj;
	}

    public void ReturnObjToPool(GameObject obj) {
        obj.SetActive(false);
    }
}
