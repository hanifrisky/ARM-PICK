using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int currentStep = 0;

    private void Awake()
    {
        instance = this;
    }

    public void NextStep()
    {
        currentStep++;
        Debug.Log("Step sekarang: " + currentStep);
    }
}