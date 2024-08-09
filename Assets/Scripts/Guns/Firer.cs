using UnityEngine;

public class Firer : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject Sound;
    [SerializeField] Transform Gun;

    bool _switch;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse2))
        {

            if (!_switch)
            {
                _switch = true;
                anim.CrossFade("Light", 0f);
                Instantiate(Sound, Gun.position, Gun.rotation);

            }
            else if (_switch)
            {
                _switch = false;
                anim.CrossFade("NoLight", 0.2f);
                Instantiate(Sound, Gun.position, Gun.rotation);
            }
        }



    }

}
