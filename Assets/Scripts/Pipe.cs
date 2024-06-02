using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{

    private void OnBecameInvisible()
    {
        GameManager.instance.RemovePipeFromList();
        Destroy(gameObject);
    }
}
