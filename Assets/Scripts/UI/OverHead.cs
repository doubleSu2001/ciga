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

    public void SetParam(int i)
    {
        img.sprite = GameMode.Instance.TrainSpriteMap[i];
        slider.value = 0;
        tick = 0;
    }
    
    public void SetTarget(ChildBehaviour child)
    {
        target = child;
        child.FailEvent.AddListener(OnEnd);
        child.SuccessEvent.AddListener(OnEnd);
    }

    // Update is called once per frame
    void Update()
    {
        tick += Time.deltaTime;
        if(tick > GameMode.Instance.NeedWaitTime)
        {
            target.FailEvent.Invoke();
            return;
        }
        slider.value = tick / GameMode.Instance.NeedWaitTime;
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
