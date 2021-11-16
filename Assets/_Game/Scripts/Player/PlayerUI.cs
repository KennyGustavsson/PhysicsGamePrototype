using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    private TimeManager timeManager;
    private Canvas playerUICanvas;
    [SerializeField] private Slider timeRewindSlider;
    [SerializeField] private GameObject deathPanel;

    private void Awake()
    {
        timeManager = FindObjectOfType<TimeManager>();
        playerUICanvas = GetComponent<Canvas>();

        timeRewindSlider.maxValue = timeManager.MaxRewindFrames;

        if (playerUICanvas.worldCamera == null) playerUICanvas.worldCamera = Camera.main;
    }


    private void OnEnable()
    {
        timeManager.PermaDeadEvent += ToggleUI;
    }

    private void OnDisable()
    {
        timeManager.PermaDeadEvent -= ToggleUI;
    }

    private void Update()
    {
        timeRewindSlider.value = timeManager.FrameCounter;        
    }

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
