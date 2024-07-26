using UnityEngine;

public class Scope: MonoBehaviour
{
    [SerializeField] Animator anim;

    bool _switch;

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Mouse1)) 
        {
            if (!_switch) 
            { 
                _switch = true;
                anim.CrossFade("Scope", 0.2f);
            }
            else
            {
                _switch = false;
                anim.CrossFade("Idle", 0.2f);
            }
        }
        
    }
}