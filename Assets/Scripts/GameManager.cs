using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public float BgmVolume = 0.5f;
    public float SEVolume = 0.5f;

    [SerializeField] private GameObject configWindow;
    [SerializeField] public Slider difficultySlider;
    [SerializeField] private bool isPause = false;
    [SerializeField] public Slider xSpeedSlider;
    [SerializeField] public Slider ySpeedSlider;
    [SerializeField] public Toggle flipToggle;
    [SerializeField] public Toggle KeyboardToggle;
    [SerializeField] public Toggle XboxToggle;
    [SerializeField] public Toggle PsToggle;
    [SerializeField] private Slider seSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Button backButton;
    [HideInInspector] public int difficultyValue=1;

    public static GameManager instance;
    public static int Score = 0;

    void Awake()
    {
        if (instance) Destroy(this.gameObject.transform.parent.gameObject);
        instance = this;
        DontDestroyOnLoad(this.transform.parent.gameObject);
    }

    private void Start()
    {
        seSlider.value = this.SEVolume;
        bgmSlider.value = this.BgmVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPause) OpenConfig();
            else CloseConfig();
        }
    }

    public void StartGame()
    {
        Score = 0;
        UpdateValue();
        SceneManager.LoadScene("main");
    }

    public void EndGame()
    {
        SceneManager.LoadScene("StartMenu");
        Player.instance = null;
    }

    public void OpenConfig()
    {
        if (isPause) return;
        isPause = true;
        Time.timeScale = 0f;
        configWindow.SetActive(true);
    }
    
    public void CloseConfig()
    {
        isPause = false;
        Time.timeScale = 1f;
        configWindow.SetActive(false);
    }

    public void UpdateValue()
    {
        if (Player.instance)
        {
            Player.instance.xAngleSpeed = xSpeedSlider.value;
            Player.instance.yAngleSpeed = ySpeedSlider.value;
            Player.instance.cameraReverse = flipToggle.isOn ? 1 : -1;
            Player.instance.inputController.isUseKeyBoard = KeyboardToggle.isOn;
            Player.instance.inputController.isUseXboxPad = XboxToggle.isOn;
            Player.instance.inputController.isUsePsPad = PsToggle.isOn;
            
        }
        this.SEVolume = seSlider.value;
        this.BgmVolume = bgmSlider.value;
        if (difficultySlider) difficultyValue = (int)difficultySlider.value;
    }
}
