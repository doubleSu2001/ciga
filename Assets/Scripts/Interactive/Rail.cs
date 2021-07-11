using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator manim;
    public Animator alert_anim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            stoptrain();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            continuetrain();
        }
    }
    public void stoptrain()
    {
        manim.speed = 0;
        alert_anim.SetBool("alert", true);
    }
    public void continuetrain()
    {
        manim.speed = 1;
        alert_anim.SetBool("alert", false);
    }
}
