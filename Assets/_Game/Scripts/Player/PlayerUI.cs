using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    private TimeManager timeManager;
    private Ragdoll playerRagdoll;
    private Canvas playerUICanvas;
    [SerializeField] private Slider timeRewindSlider;
    [SerializeField] private GameObject deathPanel;

    private void Awake()
    {
        timeManager = FindObjectOfType<TimeManager>();
        playerRagdoll = FindObjectOfType<Ragdoll>();
        playerUICanvas = GetComponent<Canvas>();

        timeRewindSlider.maxValue = timeManager.MaxRewindFrames;

        if (playerUICanvas.worldCamera == null) playerUICanvas.worldCamera = Camera.main;
    }

    //private void Update()
    //{
    //    timeRewindSlider.value = timeManager.FrameCounter;

    //    if (timeManager.FrameCounter == 0 && playerRagdoll.RagdollActive)
    //    {
    //        ToggleUI();
    //    }
    //}

    private void ToggleUI()
    {
        deathPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
