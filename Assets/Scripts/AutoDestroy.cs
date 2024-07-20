using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField]
    bool AutoStartTimer = false;

    [SerializeField]
    float timer = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (AutoStartTimer)
        {
            Invoke("DestroyNow", timer);
        }
    }

    public void RemoteDestroy(float delay)
    {
        Invoke("DestroyNow", delay);
    }

    void DestroyNow()
    {
        Destroy(gameObject);
    }
}
