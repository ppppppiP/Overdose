using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathEvents: MonoBehaviour
{
    public List<MonoBehaviour> disableScripts;
    public Rigidbody CameraRidjidBody;
    public Image DeathImg;
    public GameObject ButtonReload;


    float i = 0;
    private void OnEnable()
    {
        PlayerHP.OnPlayerDie += Death;
    }

    private void OnDisable()
    {
        PlayerHP.OnPlayerDie -= Death;
    }

    public void Death()
    {
        foreach(var scr in disableScripts)
        {
            scr.enabled = false;
        }
        CameraRidjidBody.isKinematic = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        StartCoroutine(ColorRoutine());
        ButtonReload.SetActive(true);
    }

    IEnumerator ColorRoutine()
    {
        
        yield return new WaitForSeconds(0.1f);
        DeathImg.color = new Color(DeathImg.color.r, DeathImg.color.g, DeathImg.color.b, i);
        i += 0.05f;
        if(i <= 255)
        {
            yield return null;
        }
    }
}