using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpControl : MonoBehaviour
{
    [Header("玩家血量")]
    public float CurrentHp;
    [Header("玩家开始最大血量")]
    public float MaxHp=50;
    public Image HpImage;
    public static HpControl instance;
    [Header("结束时候的GameoverUI显示")]
    public GameObject gameover;
    [Header("是否游戏结束")]
    private bool IsGameOver=false;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        CurrentHp = MaxHp;
    }

   
    void Update()
    {
     
    }

    /// <summary>
    /// 受伤的方法调用
    /// </summary>
    public void DamageHp(float sum)
    {
        if (!IsGameOver)
        {
            CurrentHp -= sum;
            if (CurrentHp <= 0)
            {
                gameover.SetActive(true);
                IsGameOver = true;
            }
            HpImage.fillAmount = CurrentHp / MaxHp;
        }
     
    }

    /// <summary>
    /// 增加血量方法
    /// </summary>
    public void AddHp(float sum)
    {
        if (!IsGameOver)
        {
            CurrentHp += sum;
            if (CurrentHp >= MaxHp)
            {
                CurrentHp = MaxHp;
            }
            HpImage.fillAmount = CurrentHp / MaxHp;
        }    
    }
}
