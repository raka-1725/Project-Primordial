using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private Vector3 mTargetRotation = new Vector3(0f, -100f, 0f);
    [SerializeField] private Vector3 mTargetMove = new Vector3(0f, -3f, 0f);
    [SerializeField] private float mRotationSpeed = 1f;
    [SerializeField] private bool mStartOpen = true; //To let bake mesh not mess up
    [SerializeField] private bool mMovesDown = true; //Door opens by moving down
    [SerializeField] private bool mStayOpen = false; // for doors that shouldn't reopen.
    [SerializeField] private bool mLeverOnly = false; //needs lever
    [SerializeField] private bool mLocked = false; //need key
    private bool mIsOpen = false;
    private bool mIsRotating = false;
    

    public void Start()
    {
        if (mStartOpen == true)
        {
            Debug.Log("Opening");
            mIsOpen = true;
            OpenMoveDoor();
        }
    }
    public void Activate(Interactions player)
    {
            if (mLeverOnly)
            {
                Debug.Log("This door can only be opened by a lever.");
                return;
            }
            if (mLocked)
            {
                if(player.mKeys <= 0)
                {
                    Debug.Log("This door is locked.");
                    return;
                }
                else
                {
                    player.mKeys--;
                    mLocked = false;
                }
            }
        OpenDoor();
    }
    public void ActivateFromLever()
    {
        OpenDoor();
    }
    private void OpenDoor()
    {
        Debug.Log("Door Open");
        if (mIsRotating) return;
        if (mMovesDown)
            OpenMoveDoor(); return;
        if (mIsOpen)
            StartCoroutine(RotateDoor(-mTargetRotation));
        else
            StartCoroutine(RotateDoor(mTargetRotation));
        mIsOpen = ! mIsOpen;
    }
    private void OpenMoveDoor()
    {
        if (mIsOpen)
            StartCoroutine(MoveDoor(-mTargetMove));
        else
            StartCoroutine(MoveDoor(mTargetMove));
         mIsOpen = ! mIsOpen;
    }
    private System.Collections.IEnumerator MoveDoor(Vector3 mTargetMove)
    {
        mIsRotating = true;

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + mTargetMove;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * mRotationSpeed;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        
        mIsRotating = false;
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
    public string GetInteractText()
    {
        if (mLocked)
            return "Locked (Need Key)";
        if (mLeverOnly)
            return "Requires Lever";
        return "Press F to Open Door";
    }
}
