using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    public bool isDestroyed;
    private void OnBecameInvisible()
    {
        if (gameObject)
        {
            GameManager.instance.RemoveCoinFromList();
            Destroy(gameObject);

        }
    }
}
