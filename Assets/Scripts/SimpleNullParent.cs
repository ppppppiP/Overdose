using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNullParent : MonoBehaviour
{
    private void OnEnable()
    {
        transform.parent = null;
    }
}
