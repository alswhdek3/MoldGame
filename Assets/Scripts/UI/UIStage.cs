using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStage : MonoBehaviour
{
    [SerializeField]
    private UI2DSprite m_stage2DSprite;

    public void SetUIStage(int stage)
    {
        var result = Resources.Load<Sprite>("UI/Stage/Stage_" + stage);
        m_stage2DSprite.sprite2D = result;
    }
}
