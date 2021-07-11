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
    public ChildBehaviour character;
    public Vector3 UpBias = Vector3.up;

    public void SetParam(int i)
    {
        img.sprite = GameMode.Instance.TrainSpriteMap[i];
        slider.value = 0;
        tick = 0;
    }
    

    public void SetTarget(ChildBehaviour child)
    {
        character = child;
    }

    // Update is called once per frame
    void Update()
    {
        tick += Time.deltaTime;
        if(tick > GameMode.Instance.NeedWaitTime)
        {
            // 需求失败
            Destroy(gameObject);
            character.FailEvent.Invoke();
        }
        slider.value = tick / GameMode.Instance.NeedWaitTime;
        Vector3 first = Camera.main.WorldToScreenPoint(character.transform.position);
        RootImg.transform.position = first + UpBias;
    }
}
