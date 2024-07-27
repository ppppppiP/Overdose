using UnityEngine;

public class Firer : MonoBehaviour
{
    [SerializeField] Animator anim;

    bool _switch;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!_switch)
            {
                _switch = true;
                anim.CrossFade("Light", 0f);

            }
            else if (_switch)
            {
                _switch = false;
                anim.CrossFade("NoLight", 0.2f);
            }
        }



    }

}