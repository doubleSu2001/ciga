using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 玩家的分数信息相关
/// </summary>
public class PlayerInfo : MonoBehaviour
{
    [Header("控制玩家得分减分的UI显示控制")]
    public Text[] ScoreImage;
    [Header("字体存在时间")]
    public float time;
    void Start()
    {
        GameMode.Instance.OnHappyChange.AddListener(OnScoreChange);
    }

    void OnScoreChange(float Delta)
    {
        if(Delta > 0)
        {
            ScoreImage[0].gameObject.SetActive(true);
            ScoreImage[0].text = "+" + (int)Delta;
        }
        else
        {
            ScoreImage[1].gameObject.SetActive(true);
            ScoreImage[1].text = "-" + (int)Delta;
        }

        Invoke("HideText", time);
    }

    void HideText()
    {
        ScoreImage[0].gameObject.SetActive(false);
        ScoreImage[1].gameObject.SetActive(false);
    }

    void Update()
    {
       if( Input.GetKeyDown(KeyCode.Y))
       {
            AddScore();
       }
        if (Input.GetKeyDown(KeyCode.U))
        {
            ReductScore();
        }
    }

    /// <summary>
    /// 加分字体携程隐藏
    /// </summary>
    /// <returns></returns>
    public IEnumerator AddScoreHide()
    {
        yield return new WaitForSeconds(time);
        ScoreImage[0].gameObject. SetActive(false);
    }


    /// <summary>
    /// 减分字体携程隐藏
    /// </summary>
    /// <returns></returns>
    public IEnumerator ReductScoreHide()
    {
        yield return new WaitForSeconds(time);
        ScoreImage[1].gameObject.SetActive(false);
    }
    public void AddScore() 
    {
        ScoreImage[0].gameObject.SetActive(true);
        StartCoroutine("AddScoreHide");
        HpControl.instance.AddHp(int.Parse(ScoreImage[0].text.ToString()));
    }
    public void ReductScore()
    {
        ScoreImage[1].gameObject.SetActive(true);
        StartCoroutine("ReductScoreHide");
        HpControl.instance.DamageHp(int.Parse(ScoreImage[0].text.ToString()));
    }
}
