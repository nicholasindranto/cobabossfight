using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePool : MonoBehaviour
{
    // bikin instance nya
    public static MissilePool instance;
    // reference ke missile gameobj nya
    public GameObject missileDamagePrefab;
    // reference ke missile pickupable gameobj nya
    public GameObject missilePickupablePrefab;
    // queue buat poolingnya
    private Queue<GameObject> poolRegularMissile = new Queue<GameObject>();
    // queue buat poolingnya
    private Queue<GameObject> poolPickUpMissile = new Queue<GameObject>();

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
        // bikin 3 saja untuk yang damage
        for (int i = 0; i < 3; i++)
        {
            GameObject obj = Instantiate(missileDamagePrefab);
            obj.SetActive(false);
            poolRegularMissile.Enqueue(obj);
        }

        // bikin 2 saja untuk yang pickupable
        for (int i = 0; i < 2; i++)
        {
            GameObject obj = Instantiate(missilePickupablePrefab);
            obj.SetActive(false);
            poolPickUpMissile.Enqueue(obj);
        }
    }

    // buat ngambil regular missilenya
    public GameObject GetFromPool(string regularOrPickUp)
    {
        if (regularOrPickUp == "regular")
        {
            // kalau ada maka di return, kalau nggak ada maka di instantiate dulu
            if (poolRegularMissile.Count > 0) return poolRegularMissile.Dequeue();
            else return Instantiate(missileDamagePrefab);
        }
        else
        {
            // kalau ada maka di return, kalau nggak ada maka di instantiate dulu
            if (poolPickUpMissile.Count > 0) return poolPickUpMissile.Dequeue();
            else return Instantiate(missilePickupablePrefab);
        }
    }

    // balikin ke poolnya
    public void ReturnToPool(GameObject obj, string regularOrPickUp)
    {
        obj.SetActive(false);
        if (regularOrPickUp == "regular") poolRegularMissile.Enqueue(obj);
        else poolPickUpMissile.Enqueue(obj);
    }
}
