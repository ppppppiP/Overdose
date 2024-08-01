using UnityEngine;

public class Lazer: MonoBehaviour
{
    [SerializeField] GameObject _lazer;
    private void Update()
    {
       
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        

        if (Physics.Raycast(ray, out RaycastHit hit))
        {

            _lazer.transform.position = hit.point;
            
        }
    }
}