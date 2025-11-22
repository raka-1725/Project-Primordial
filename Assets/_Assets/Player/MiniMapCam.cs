using UnityEngine;

public class MiniMapCam : MonoBehaviour
{
    [SerializeField] private Transform mPlayerTransform;
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(90f, 0, 0f);
    }
}
