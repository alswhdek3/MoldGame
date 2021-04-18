using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleController : SingtonMonoBehaviour<MoleController>
{
    [SerializeField] private UIGrid m_boxGrid;
    [SerializeField] private GameObject m_boxPrefab;
    [SerializeField] private Camera m_uiCamera;

    private List<MoleBox> m_boxList = new List<MoleBox>();
    private Dictionary<int, Mole> m_boxTable = new Dictionary<int, Mole>();

    //플레이어 스텟
    private int m_hp;
    private float m_time;

    public List<MoleBox> BoxList { get { return m_boxList; } }
    public Dictionary<int, Mole> BoxTable { get { return m_boxTable; } }

    private GameObject GetTouchBox()
    {
        Ray ray = m_uiCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;       
        if(Physics.Raycast(ray,out hit,1000f,1<<LayerMask.NameToLayer("UI")))
        {
            for(int i=0; i<m_boxList.Count; i++)
            {
                if(hit.collider.transform == m_boxList[i].transform)
                {
                    return hit.collider.gameObject;
                }
            }
        }
        return null;
    }
    private void CheckMonster()
    {
        var boxTable = m_boxTable;
        var boxList = m_boxList;
        for(int i=0; i<boxList.Count; i++)
        {
            if(boxList[i].IsSelect) //박스가 선택되어있고
            {
                if(boxTable[i] != null) //박스안에 두더지가 있으면 점수 추가 획득
                {
                    Mole killMole = boxTable[i];
                    var stateInfo = killMole.StateInfo;
                    if(!stateInfo.IsName("Kill"))
                    {
                        int stage = GameController.Instance.Stage;
                        int score = stage * 10;
                        GameController.Instance.AddScore(score); //점수 추가 획득

                        killMole.KillPlay();
                        break;
                    }                   
                }
            }
        }
        if(GameController.Instance.CurrentScore > 0) Debug.LogError(GameController.Instance.CurrentScore);
    }
    protected override void OnStart()
    {
        //두더지 박스 생성
        for (int i = 0; i < 9; i++)
        {
            var obj = Instantiate(m_boxPrefab);
            obj.transform.SetParent(m_boxGrid.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            m_boxGrid.Reposition();
            var moleBox = obj.GetComponent<MoleBox>();
            moleBox.gameObject.SetActive(true);
            m_boxList.Add(moleBox);
        }
        //박스 선택 초기화
        for (int i = 0; i < m_boxList.Count; i++)
        {
            m_boxTable.Add(i, null);
            m_boxList[i].SetSelect(false);
        }
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var target = GetTouchBox();
            if(target != null && GameController.Instance.IsReady)
            {
                for(int i=0; i<m_boxList.Count; i++)
                    m_boxList[i].SetSelect(false);
                var box = target.GetComponent<MoleBox>();
                box.SetSelect(true); // 박스 선택 상태로 변경
                CheckMonster();// 선택한 박스에 두더지가있는지 확인 -> 잡히면 점수 추가
                box.StartTimer();                
            }
        }
    }
}       
