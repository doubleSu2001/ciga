using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeControl : MonoBehaviour
{
    public Image HpImage;
    void Start()
    {

    }

    void Update()
    {
        HpImage.fillAmount = (GameMode.Instance.MaxTime - GameMode.Instance.CurTime) / GameMode.Instance.MaxTime;
    }


}
