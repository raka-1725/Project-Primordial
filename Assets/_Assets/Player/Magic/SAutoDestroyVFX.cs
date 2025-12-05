using UnityEngine;

public class SAutoDestroyVFX : MonoBehaviour
{
    private ParticleSystem ps;
    [SerializeField] private GameObject mAoE;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        mAoE = ps.gameObject;
    }

    void Update()
    {
        if (ps.gameObject != null)
        {
            Destroy(gameObject);
        }
    }

}
