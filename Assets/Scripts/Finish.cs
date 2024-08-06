using Controller;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] GameObject FinishMenu_Bad;
    [SerializeField] TextMeshProUGUI _shprizCount;
    [SerializeField] GameObject FinishMenu_Good;
    public int MaxShpriz;
    public int Shpriz;

    public static Finish instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetFinishActive()
    {
        if (Shpriz < MaxShpriz)
        {
            _shprizCount.text = Shpriz.ToString();
            FinishMenu_Bad.SetActive(true);
        }
        else
        {
            FinishMenu_Good.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>())
        {
            SetFinishActive();
        }
    }
}
