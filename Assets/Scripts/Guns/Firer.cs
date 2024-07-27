using UnityEngine;

public class Firer : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject hands;
    [SerializeField] GameObject target;

    bool _switch;

    private void Update()
    {

        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (!_switch)
            {
                _switch = true;
                anim.CrossFade("Fire", 0f);
                hands.transform.position = target.transform.position;
            }

        }
        else if (_switch)
        {
            _switch = false;
            anim.CrossFade("Idle", 0f);
        }


    }
}