using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIControl : MonoBehaviour
{
    [Header("生成小孩预制体拖入")]
    public GameObject Children;
    [Header("等待时间")]
    private float time;
    public Sprite[] Item;
    private 
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            time = 5;
            StartCoroutine("WiatTime");
        }
    }
    public IEnumerator WiatTime()
    {
        yield return new WaitForSeconds(time);
        ImageInstiate();
    }

    public void ImageInstiate()
    {
       var v= Instantiate(Children);
        v.transform.SetParent(gameObject.transform);
        v.transform.localPosition = new Vector3(0, 0, 0);
        v.GetComponent<Image>().sprite = Item[Random.Range(0, Item.Length)];
    }
}
