using UnityEngine;
using UnityEngine.UIElements;

public class PassPlayerPosition : MonoBehaviour
{
    public Material seeThroughMaterial;
    public Transform mPlayer;


    private void Awake()
    {
        mPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        seeThroughMaterial.SetVector("_PlayerPosition", mPlayer.position);
    }
}
