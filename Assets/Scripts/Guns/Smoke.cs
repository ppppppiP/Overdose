using UnityEngine;

public class Smoke : MonoBehaviour
{
    [SerializeField] private GameObject Parent;
    [SerializeField] private GameObject Child;
    public void Start()
    {
        Parent = GameObject.FindGameObjectWithTag("Gun");
        Child = this.gameObject;


        Child.transform.SetParent(Parent.transform);

    }
}
