using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{ 
    public static ObjectPool instance;
    // Create definitions for object pools
    public List<GameObject> pooledRocks;
    public GameObject rockToPool;
    public int howManyRocks;
    public List<GameObject> pooledStars;
    public GameObject starToPool;
    public int howManyStars;
    public List<GameObject> pooledEBullets;
    public GameObject eBulletToPool;
    public int howManyEBullets;
    public List<GameObject> pooledPBullets;
    public GameObject pBulletToPool;
    public int howManyPBullets; 
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Create lists to hold object pools
        pooledStars = new List<GameObject>();
        pooledEBullets = new List<GameObject>();
        pooledPBullets = new List<GameObject>();
        pooledRocks = new List<GameObject>();
        GameObject tmp;

        // Instantiate and populate lists with object instances
        for(int i = 0; i < howManyStars; i++)
        {
            tmp = Instantiate(starToPool);
            tmp.SetActive(false);
            pooledStars.Add(tmp);
        }
        for (int i = 0; i < howManyPBullets; i++)
        {
            tmp = Instantiate(pBulletToPool);
            tmp.SetActive(false);
            pooledPBullets.Add(tmp);
        }
        for (int i = 0; i < howManyEBullets; i++)
        {
            tmp = Instantiate(eBulletToPool);
            tmp.SetActive(false);
            pooledEBullets.Add(tmp);
        }
        for (int i = 0; i < howManyRocks; i++)
        {
            tmp = Instantiate(rockToPool);
            tmp.SetActive(false);
            pooledRocks.Add(tmp);
        }
    }
    public GameObject GetPooledStars()
    {
        for (int i = 0; i < howManyStars; i++)
        {
            if (!pooledStars[i].activeInHierarchy)
            {
                return pooledStars[i];
            }
        }
        return null;
    }
    public GameObject GetPooledPBullets()
    {
        for (int i = 0; i < howManyPBullets; i++)
        {
            if (!pooledPBullets[i].activeInHierarchy)
            {
                return pooledPBullets[i];
            }
        }
        return null;
    }
    public GameObject GetPooledEBullets()
    {
        for (int i = 0; i < howManyEBullets; i++)
        {
            if (!pooledEBullets[i].activeInHierarchy)
            {
                return pooledEBullets[i];
            }
        }
        return null;
    }
    public GameObject GetPooledRocks()
    {
        for (int i = 0; i < howManyRocks; i++)
        {
            if (!pooledRocks[i].activeInHierarchy)
            {
                return pooledRocks[i];
            }
        }
        return null;
    }
}
