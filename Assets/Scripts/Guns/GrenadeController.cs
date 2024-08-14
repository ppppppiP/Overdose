using System.Collections.Generic;
using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    [SerializeField] private GameObject _grenade;
    [SerializeField] private GameObject _molotov;
    [SerializeField] private Transform _grenadeOutPosition;
    [SerializeField] private GameObject _grenadeMenu;
    [SerializeField] private GrenadesArsenal grenades;

    public int currentGrenID = 1;
    private GameObject currentGren;

    private KeyCode _grenadeKey;

    private void Start()
    {
        // ��������� ������� ������ �������
        _grenadeKey = (KeyCode)PlayerPrefs.GetInt("Grenade", (int)KeyCode.G);
        UpdateCurrentGrenade();
    }

    private void Update()
    {
        if (!_grenadeMenu.activeSelf)
        {
            if (Input.GetKeyUp(_grenadeKey))
            {
                UpdateCurrentGrenade();
                ThrowGrenade();
            }
        }
    }

    private void ThrowGrenade()
    {
        // ���������, ���� �� ������� ����� �������
        if (currentGrenID == 1 && grenades.CurrentGrenadeID == 1 && grenades.GetGrenadeCount() > 0)
        {
            Instantiate(currentGren, _grenadeOutPosition.position, _grenadeOutPosition.rotation);
            grenades.RemoveItem(1, currentGrenID);
        }
        else if (currentGrenID == 2 && grenades.CurrentGrenadeID == 2 && grenades.GetMolotovCount() > 0)
        {
            Instantiate(currentGren, _grenadeOutPosition.position, _grenadeOutPosition.rotation);
            grenades.RemoveItem(1, currentGrenID);
        }
    }

    // ����� ��� ���������� �������� ���� �������
    private void UpdateCurrentGrenade()
    {
        switch (currentGrenID)
        {
            case 1:
                currentGren = _grenade;
                break;
            case 2:
                currentGren = _molotov;
                break;
           
        }
    }

    // ����� ��� ������������ ���� ������� �� ����
    public void SelectGrenade(int grenadeID)
    {
        currentGrenID = grenadeID;
        UpdateCurrentGrenade();
    }
}
