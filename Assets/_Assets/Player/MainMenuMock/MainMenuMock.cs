using UnityEngine;

public class MainMenuMock : MonoBehaviour
{
    Animator mAnimator;
    ButtonHover[] mButtons;

    private void Awake()
    {
        mAnimator = GetComponent<Animator>();

        mButtons = FindObjectsByType<ButtonHover>(FindObjectsSortMode.None);
        foreach (ButtonHover BHover in mButtons)
        {
            BHover.onHoverChanged += HoverOverButtion;
        }

    }
    public void HoverOverButtion(bool hover) 
    {
        mAnimator.SetBool("ButtonHover", hover);
        Debug.Log($"BH {hover}");
    }

    public void StartScene() 
    {
        mAnimator.SetTrigger("StartGame");
    }

    public void FinishAnim() 
    {

        AsyncLoader asyncLoader = FindAnyObjectByType<AsyncLoader>();
        asyncLoader.LoadLevel("Maze");
    }
}
