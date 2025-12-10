using UnityEngine;

public class GameMode: MonoBehaviour
{
    public static GameMode Instance { get; private set; }

    public bool bClock = true;
    public int enemyCount = 10;

    private void Awake()
    {
        Instance = this;
    }

    public void NormalGameMode() 
    {
        bClock = true;
        enemyCount = 10;
    }

    public void HCGameMode() 
    {
        bClock = true;
        enemyCount = 30;
    }

    public void TAGameMode() 
    {
        bClock = false;
        enemyCount = 15;
    }
}
