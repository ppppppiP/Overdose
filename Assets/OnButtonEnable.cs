using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class OnButtonEnable : MonoBehaviour
{
    private void OnEnable()
    {
        Wait();
    }

    public async void Wait()
    {
        await Task.Delay(3000);
        transform.DOScale(1, 1f);
    }
}
