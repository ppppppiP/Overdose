using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerFarm : MonoBehaviour
{
    [SerializeField] GameObject instanceWorkObject;
    [SerializeField] int X;
    [SerializeField] int Y;

    public List<Transform> workTransforms;

    private void Awake()
    {
        // Loop through X and Y coordinates simultaneously
        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                Vector3 position = transform.position + Vector3.right * i + Vector3.forward * j;
                GameObject a = Instantiate(instanceWorkObject, position, transform.rotation, transform.parent);
                workTransforms.Add(a.transform);
            }
        }
        StartCoroutine(RespawnPoint());
    }

    public IEnumerator RespawnPoint()
    {
        WaitForSeconds wait = new WaitForSeconds(2f);
        
            foreach(var a in workTransforms)
            {
                if (!a.gameObject.activeSelf)
                {
                    yield return wait;
                    a.gameObject.SetActive(true);
                
                }

            }
        yield return null;
    } 

}
