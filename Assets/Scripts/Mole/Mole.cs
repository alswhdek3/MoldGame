using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    private Animator m_animator;
    private MonsterType m_type;

    public MonsterType Type { get { return m_type; } }
    public AnimatorStateInfo StateInfo { get { return m_animator.GetCurrentAnimatorStateInfo(0); } }

    private IEnumerator Coroutine_RemoveMonster(int index)
    {
        int stage = GameController.Instance.Stage;
        float duration = 0f;
        if (stage == 1) duration = 2f;
        else if (stage == 2) duration = 1.75f;
        else if (stage == 3) duration = 1.5f;
        else if (stage == 4) duration = 1.35f;
        else if (stage == 5) duration = 1f;

        var moleTable = MoleController.Instance.BoxTable;
        yield return new WaitForSeconds(duration);
        MonsterManager.Instance.AddPoolMonster(m_type, this);
        moleTable[index] = null;

    }  
    public void SetMole(MonsterType type)
    {
        m_type = type;
    }
    public void StartTimer(int index)
    {
        StartCoroutine(Coroutine_RemoveMonster(index));
    }
    public void KillPlay() //몬스터를 잡았을경우 애니메이션 재생
    {       
        var stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("Kill"))
        {
            m_animator.Play("Kill");
            if (!IsInvoking("DisableMonster")) Invoke("DisableMonster", 0.5f);
        }     
    }
    private void DisableMonster(int index)
    {
        var moleTable = MoleController.Instance.BoxTable;
        MonsterManager.Instance.AddPoolMonster(m_type, this);
        moleTable[index] = null;
    }
    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }
}
