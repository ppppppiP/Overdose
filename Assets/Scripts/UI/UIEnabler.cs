using UnityEngine;

public class UIEnabler: MonoBehaviour
{
    [SerializeField] GameObject _UI;
    KeyCode _key;

    private void Start()
    {
        _key = (KeyCode)PlayerPrefs.GetInt("OpenMenu", (int)KeyCode.Escape);
    }
    private void Update()
    {
        if (Input.GetKeyDown(_key))
        {
            if(_UI.activeSelf != !_UI.activeSelf)
            {
                _UI.active = !_UI.activeSelf;
                Cursor.visible = _UI.activeSelf;
                Cursor.lockState = _UI.activeSelf ?  CursorLockMode.Confined: CursorLockMode.Locked;
                Time.timeScale = _UI.activeSelf? 0f : 1f;
            }
        }
    }
}