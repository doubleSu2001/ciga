using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ToyTimeControl : MonoBehaviour
{
    [Header("控制小孩玩具的时间")]
    public float MaxToyTime;
    private float CurrentTime;
    public Image Toytime;
    public GameObject ToyNeedImage;
    //需求UI
    public GameObject[] Item;
    //回馈UI
    public Image[] Back;
    //协程同步时间
    [Header("控制小孩哭脸笑脸显示时间")]
    public float time;
    [Header("控制小孩需求UI开始显示的时间")]
    public float needTime;
    // Start is called before the first frame update
    void Start()
    {
        CurrentTime = MaxToyTime;
    }

    // Update is called once per frame
    void Update()
    {
        ReduceTime();
        ReplaceColor(CurrentTime);
        ReplaceChildUI(CurrentTime);

    }
    //时间减少
    public void ReduceTime()
    {
        CurrentTime -= Time.deltaTime;
        Toytime.fillAmount = CurrentTime / MaxToyTime;
    }
    //改变冷却条颜色
    public void ReplaceColor(float CurrentTime)
    {
        if (CurrentTime <= 20 && CurrentTime >= 10)
        {
            Toytime.color = Color.yellow;
        }
        else if (CurrentTime < 10)
        {
            Toytime.color = Color.red;
        }
    }
    public void NeedImageInstance()
    {
        ToyNeedImage = Item[Random.Range(0, Item.Length)];
    }
    //这里连接孩子需求是否正确的代码
    public void ReplaceChildUI(float CurrentTime)
    {
        if (CurrentTime > 0&&Input.GetKeyDown(KeyCode.Z)) //需求正确
        {
            Toytime.gameObject.SetActive(false);
            ChildHappy();

        }
        if(CurrentTime<=0)//需求错误或时间耗尽
        {
            Toytime.gameObject.SetActive(false);
            ChildCry();
        }
    }
    public IEnumerable ChildCryHide()
    {
        yield return new WaitForSeconds(time);
        Back[0].gameObject.SetActive(false);
    }
    public IEnumerable ChildHappyHide()
    {
        yield return new WaitForSeconds(time);
        Back[1].gameObject.SetActive(false);
    }
    public void ChildCry()
    {
        Back[0].gameObject.SetActive(true);
        StartCoroutine("ChildCryHide");
    }
    public void ChildHappy()
    {
        Back[1].gameObject.SetActive(true);
        StartCoroutine("ChildHappyHide");
    }
    //需求准备生成的时间
    public IEnumerable NeedInsitateTime()
    {
        yield return new WaitForSeconds(needTime);
        NeedImageInstance();
    }
}
