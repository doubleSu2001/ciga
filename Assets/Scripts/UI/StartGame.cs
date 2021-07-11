using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public Image [] StudyUI;
    [Header("开始学习UI判断")]
    private bool isStudyUI;
    [Header("学习UI的页数控制")]
    private int studyIndex=0;

    // Start is called before the first frame update
    void Start()
    {      

    }
    public void StudyUI1()
    {
        isStudyUI = true;
        StudyUI[0].gameObject.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)&&isStudyUI)
        {
            StudyUI[studyIndex].gameObject.SetActive(true);
            studyIndex += 1;
        }
        if (studyIndex >= 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("转换场景");
                SceneManager.LoadScene(1);
            }
        }
    }
}
