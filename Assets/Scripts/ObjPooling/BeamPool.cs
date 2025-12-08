using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamPool : MonoBehaviour
{
    // bikin instance nya
    public static BeamPool instance;
    // reference ke beam gameobj nya
    public GameObject beamColliderPrefab;
    // queue buat poolingnya
    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // bikin poolnya dulu
        CreateObjPool();
    }

    private void CreateObjPool()
    {
        // bikin 2 saja
        for (int i = 0; i < 2; i++)
        {
            GameObject obj = Instantiate(beamColliderPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    // buat ngambil beamcollidernya
    public GameObject GetFromPool()
    {
        // kalau ada maka di return, kalau nggak ada maka di instantiate dulu
        if (pool.Count > 0) return pool.Dequeue();
        else return Instantiate(beamColliderPrefab);
    }

    // balikin ke poolnya
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
