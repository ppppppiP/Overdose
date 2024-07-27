using UnityEngine;

public class Scope: MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject hands;
    [SerializeField] GameObject target;

    bool _switch;

    private void Update()
    {
        
        if(Input.GetKey(KeyCode.Mouse1)) 
        {
            if (!_switch) 
            { 
                _switch = true;
                anim.CrossFade("Scope", 0.2f);
                hands.transform.position = target.transform.position;
            }
            
        }
        else if(_switch)
        {
            _switch = false;
            anim.CrossFade("Noscope", 0.2f);
        }


    }
}
