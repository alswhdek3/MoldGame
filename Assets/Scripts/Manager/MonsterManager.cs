using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    None=-1,
    Duck,
    Max
}
public class MonsterManager : SingtonMonoBehaviour<MonsterManager>
{
    [SerializeField] private GameObject m_molePrefab;
    private Dictionary<MonsterType, List<Mole>> m_dicMolePool = new Dictionary<MonsterType, List<Mole>>();
    private Dictionary<MonsterType, GameObject> m_dicMolePrefab = new Dictionary<MonsterType, GameObject>();

    public Mole CreateMonster(MonsterType type)
    {
        Mole newMole = null;
        if (!m_dicMolePool.ContainsKey(type)) return null;
        else
        {
            var pool = m_dicMolePool[type];
            if(pool.Count > 0)
            {
                newMole = pool[0];
                pool.Remove(newMole);
                newMole.SetMole(type);
                newMole.gameObject.SetActive(true);
                return newMole;
            }
            else
            {
                if (!m_dicMolePrefab.ContainsKey(type)) return null;
                var prefab = m_dicMolePrefab[type];
                newMole = CreateMonsterUnit(type,prefab);
                newMole.gameObject.SetActive(true);
                return newMole;
            }
        }
    }
    private Mole CreateMonsterUnit(MonsterType type, GameObject prefab)
    {
        var mole = prefab.GetComponent<Mole>();
        mole.SetMole(type);
        if(mole == null) mole = prefab.GetComponent<Mole>();
        return mole;
    }
    public void AddPoolMonster(MonsterType type,Mole mole)
    {
        if (!m_dicMolePool.ContainsKey(type)) return;
        else
        {
            mole.gameObject.SetActive(false);
            m_dicMolePool[type].Add(mole);
        }
    }
    protected override void OnStart()
    {
        for(int i=0; i<(int)MonsterType.Max; i++)
        {
            m_molePrefab = Resources.Load("Prefab/Monster/" + (MonsterType)i) as GameObject;
            List<Mole> moleList = new List<Mole>();
            moleList.Clear();
            for(int j=0; j<2; j++)
            {
                var obj = Instantiate(m_molePrefab);
                obj.transform.SetParent(transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                var newMole = obj.GetComponent<Mole>();
                newMole.gameObject.SetActive(false);
                moleList.Add(newMole);
            }
            if (!m_dicMolePool.ContainsKey((MonsterType)i)) m_dicMolePool.Add((MonsterType)i, moleList);
            //Dictionary Prefab 추가
            var prefab = Instantiate(m_molePrefab);
            prefab.transform.SetParent(transform);
            prefab.transform.localPosition = Vector3.zero;
            prefab.transform.localScale = Vector3.one;
            prefab.gameObject.SetActive(false);
            if (!m_dicMolePrefab.ContainsKey((MonsterType)i)) m_dicMolePrefab.Add((MonsterType)i, prefab);
        }
    }
}
