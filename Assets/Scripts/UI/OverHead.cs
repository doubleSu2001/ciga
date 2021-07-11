using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverHead : MonoBehaviour, ISpawnInfo
{
    public Image img;
    public Image RootImg;
    public Slider slider;
    public float tick = 0;
    public ChildBehaviour target;
    public Vector3 UpBias = Vector3.up;
    public bool bCount = true;

    public void SetParam(int i)
    {
        if(i < 10) // 需求
        {
            img.sprite = GameMode.Instance.TrainSpriteMap[i];
            slider.value = 0;
            tick = 0;
        }
        else // 表情符号
        {
            bCount = false;
            slider.gameObject.SetActive(false);
            var Gift = GameMode.Instance.Gifts[i - 10];
            img.sprite = Gift.Image;
            Destroy(gameObject, GameMode.Instance.FaceKeepTime);
        }
    }
    
    public void SetTarget(ChildBehaviour child)
    {
        target = child;
        target.OnDestoryEvent.AddListener(End);
        child.FailEvent.AddListener(OnEnd);
        child.SuccessEvent.AddListener(OnEnd);
    }

    void End()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(bCount)
        {
            tick += Time.deltaTime;
            if (tick > GameMode.Instance.NeedWaitTime)
            {
                target.FailEvent.Invoke();
                GameMode.Instance.ApplyGift("递交失败", target);
                return;
            }
            slider.value = tick / GameMode.Instance.NeedWaitTime;
        }
        Vector3 first = Camera.main.WorldToScreenPoint(target.transform.position);
        RootImg.transform.position = first + UpBias;
    }

    void OnEnd()
    {
        target.FailEvent.RemoveListener(OnEnd);
        target.SuccessEvent.RemoveListener(OnEnd);
        Destroy(gameObject);
    }
}
