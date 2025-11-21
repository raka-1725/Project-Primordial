using UnityEngine;

public class SAutoDestroyVFX : MonoBehaviour
{
    private ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (ps != null && !ps.IsAlive(true))
        {
            Destroy(gameObject);
        }
    }

}
