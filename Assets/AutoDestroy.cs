using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float destroyTime = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroyTimer());
    }

    IEnumerator destroyTimer()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
        
    }
}
