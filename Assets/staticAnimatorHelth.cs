using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class staticAnimatorHelth : MonoBehaviour
{
    public Animator anim;

    public static staticAnimatorHelth instance;
    void Start()
    {
        instance = this;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
