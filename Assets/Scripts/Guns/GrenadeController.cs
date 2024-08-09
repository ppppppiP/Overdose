using System.Collections.Generic;
using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    [SerializeField] GameObject _grenade;
    [SerializeField] GameObject _grenadeOutPosition;

    KeyCode _grenadeKey;

    private void Start()
    {
        _grenadeKey = (KeyCode)PlayerPrefs.GetInt("Grenade", (int)KeyCode.G);
    }

    private void Update()
    {
        if (Input.GetKeyDown(_grenadeKey))
        {
          var GO = Instantiate(_grenade, _grenadeOutPosition.transform.position, _grenadeOutPosition.transform.localRotation);
            GO.transform.rotation = _grenadeOutPosition.transform.rotation;
        }
    }
}
