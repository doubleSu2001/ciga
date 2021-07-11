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

    public Sprite WinSprite;
    public Sprite FailSprite;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        GameMode.Instance.OnWinEvent.AddListener(ShowWin);
        GameMode.Instance.OnLoseEvent.AddListener(ShowLose);
    }

    void ShowWin()
    {
        gameover.SetActive(true);
        gameover.GetComponent<Image>().sprite = WinSprite;
        gameover.GetComponent<Image>().SetNativeSize();
    }
    void ShowLose()
    {
        gameover.SetActive(true);
        gameover.GetComponent<Image>().sprite = FailSprite;
        gameover.GetComponent<Image>().SetNativeSize();
    }

    void Update()
    {
        HpImage.fillAmount = GameMode.Instance.CurHappy / GameMode.Instance.MaxHappy;
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
            if (CurrentHp <= 0)
            {
                gameover.SetActive(true);
                IsGameOver = true;
            }
            HpImage.fillAmount = CurrentHp / MaxHp;
        }    
    }
}
