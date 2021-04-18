using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleBox : MonoBehaviour
{
    [SerializeField] private GameObject m_selectEffect;
    private bool m_isSelect;    
    
    public bool IsSelect { get { return m_isSelect; } }
    private IEnumerator Coroutine_StartTimer()
    {
        yield return new WaitForSeconds(0.2f);
        m_isSelect = false;
        m_selectEffect.gameObject.SetActive(false);
    }
    public void SetSelect(bool isSelect)
    {
        m_isSelect = isSelect;
        m_selectEffect.gameObject.SetActive(isSelect);
    }
    public void StartTimer() { StartCoroutine(Coroutine_StartTimer()); }
}
