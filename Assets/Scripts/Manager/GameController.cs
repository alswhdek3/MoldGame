using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : SingtonMonoBehaviour<GameController>
{
    [SerializeField] private GameObject m_gameReady, m_gameStart;
    private int m_stage=1;//현재 스테이지
    private const int m_maxStage = 5; //최대 스테이지
    private float m_showTime; //게임 현재 시간
    private int m_score; // 현재 획득한 점수
    private bool m_isReady;

    public int Stage { get { return m_stage; } }
    public int CurrentScore { get { return m_score; } }
    public bool IsReady { get { return m_isReady; } }
    private IEnumerator Coroutine_GamePattern()
    {
        float time = 0f;
        float duration = 0f;

        if (m_stage == 1) duration = 2f;
        else if (m_stage == 2) duration = 1.5f;
        else if (m_stage == 3) duration = 1f;
        else if (m_stage == 4) duration = 0.7f;
        else if (m_stage == 5) duration = 0.5f;

        var boxTable = MoleController.Instance.BoxTable;
        var boxList = MoleController.Instance.BoxList;
        while(true)
        {
            time += Time.deltaTime;
            if(time >= duration)
            {
                var newMole = MonsterManager.Instance.CreateMonster((MonsterType)m_stage - 1);
                int index = Random.Range(0, boxList.Count);               
                if(boxTable[index] != null)
                {
                    while(true)
                    {
                        index = Random.Range(0, boxList.Count);
                        if (boxTable[index] == null) break;
                    }
                }
                newMole.transform.SetParent(boxList[index].transform);
                newMole.transform.localPosition = new Vector3(0f, -40f, 0f);
                newMole.transform.localScale = Vector3.one;
                boxTable[index] = newMole;
                newMole.StartTimer(index);

                time = 0f;
            }
            yield return null;
        }
    }
    public void AddScore(int amount)
    {
        if (amount < 0) return;
        else
        {
            m_score += amount;
            Debug.LogError(m_score);
        }
    }
    public void SetReadyUI() { Invoke("DisableReadyUI",1f); }
    private void DisableReadyUI()
    {
        m_gameReady.gameObject.SetActive(false);
        m_gameStart.gameObject.SetActive(true);
    }
    public void SetStartUI() { Invoke("DisableStartUI", 1f); }
    private void DisableStartUI()
    {
        m_gameStart.gameObject.SetActive(false);
        m_isReady = true;
        StartCoroutine(Coroutine_GamePattern());
    }
    protected override void OnStart()
    {
        m_stage = 1;
        m_isReady = false;
        var stageUI = m_gameReady.GetComponent<UIStage>();
        stageUI.SetUIStage(m_stage);
        m_gameStart.gameObject.SetActive(false);        
    }
}
