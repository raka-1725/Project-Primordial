using UnityEngine;

public class Door : MonoBehaviour//, Interactable
{
    [SerializeField] private Vector3 mTargetRotation = new Vector3(0f, -100f, 0f);
    [SerializeField] private float mRotationSpeed = 3f;
    [SerializeField] private bool mStayOpen = false;
    private bool mIsOpen = false;
    private bool mIsRotating = false;
    

    public void Activate()
    {
         Debug.Log("Door Open");
        if (mIsRotating) return;
        if (mIsOpen)
            StartCoroutine(RotateDoor(-mTargetRotation));
        else
            StartCoroutine(RotateDoor(mTargetRotation));
        mIsOpen = ! mIsOpen;
    }
    private System.Collections.IEnumerator RotateDoor(Vector3 rotationAmount)
    {
        mIsRotating = true;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(rotationAmount);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * mRotationSpeed;
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }
        if (mStayOpen == false)
        mIsRotating = false;
    }
}
