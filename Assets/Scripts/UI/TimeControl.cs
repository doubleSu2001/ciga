using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeControl : MonoBehaviour
{
    [Header("玩家血量")]
    public float CurrentTime=50;
    [Header("玩家开始最大血量")]
    public float MaxTime = 200;
    public Image HpImage;
    void Start()
    {
        //CurrentTime = MaxTime;
    }


    void Update()
    {
        DamageHp();
     
    }

    /// <summary>
    /// 受伤的方法调用
    /// </summary>
    public void DamageHp()
    {
        CurrentTime -= Time.deltaTime;
        HpImage.fillAmount = CurrentTime / MaxTime;
    }

}
