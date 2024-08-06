using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiPointHolder : MonoBehaviour
{
    public List<Transform> Points;

    private void Start()
    {
        transform.parent = null;
    }
}
