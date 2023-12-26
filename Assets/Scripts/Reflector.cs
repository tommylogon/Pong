using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : MonoBehaviour
{
    Collider2D triggerArea;
    // Start is called before the first frame update
    void Start()
    {
        triggerArea = GetComponent<Collider2D>();
    }


    
}
