using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ShortCartsTest : MonoBehaviour
{
    
    bool _canInput;
    void Start()
    {
        
    }

    
    [ExecuteInEditMode]
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && _canInput)
        {
            if(Input.GetKey(KeyCode.Q)) 
            {
                Debug.Log("Короткая клавиша сработала");
            }
            _canInput = false;
        }
        if(!Input.GetKey(KeyCode.Q)) 
        {
            _canInput = true;
        }
       
    }
    
}

// НЕ УСПЕШНО