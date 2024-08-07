using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;


public class ControlSettingsManager : MonoBehaviour
{
    public TextMeshProUGUI actionText; // Текстовое поле для отображения текущей настройки
    public Button setKeyButton; // Кнопка для назначения новой клавиши
    public Slider sensitivitySlider;

    public TextMeshProUGUI fwdButton;
    public TextMeshProUGUI backButton;
    public TextMeshProUGUI leftButton;
    public TextMeshProUGUI rightButton;
    public TextMeshProUGUI crouchButton;
    public TextMeshProUGUI shootButton;
    public TextMeshProUGUI lightButton;

    private string currentAction; // Текущая настраиваемая клавиша
    private Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();
    private Dictionary<string, TextMeshProUGUI> actionTexts = new Dictionary<string, TextMeshProUGUI>();

    private const string Sensitivity = "Sensitivity";

    void Awake()
    {
        // Загрузка сохраненных настроек
        keyBindings["Shoot"] = (KeyCode)PlayerPrefs.GetInt("ShootKey", (int)KeyCode.Space);
        keyBindings["Flashlight"] = (KeyCode)PlayerPrefs.GetInt("FlashlightKey", (int)KeyCode.F);
        keyBindings["MoveForward"] = (KeyCode)PlayerPrefs.GetInt("MoveForwardKey", (int)KeyCode.W);
        keyBindings["MoveBackward"] = (KeyCode)PlayerPrefs.GetInt("MoveBackwardKey", (int)KeyCode.S);
        keyBindings["MoveLeft"] = (KeyCode)PlayerPrefs.GetInt("MoveLeftKey", (int)KeyCode.A);
        keyBindings["MoveRight"] = (KeyCode)PlayerPrefs.GetInt("MoveRightKey", (int)KeyCode.D);
        keyBindings["Crouch"] = (KeyCode)PlayerPrefs.GetInt("CrouchKey", (int)KeyCode.LeftControl);
        keyBindings["Run"] = (KeyCode)PlayerPrefs.GetInt("RunKey", (int)KeyCode.LeftShift);
        keyBindings["Interact"] = (KeyCode)PlayerPrefs.GetInt("InteractKey", (int)KeyCode.E);
        sensitivitySlider.value = PlayerPrefs.GetFloat(Sensitivity, 1.0f);

        // Инициализация отображения текста для каждой кнопки
        actionTexts["Shoot"] = shootButton;
        actionTexts["Flashlight"] = lightButton;
        actionTexts["MoveForward"] = fwdButton;
        actionTexts["MoveBackward"] = backButton;
        actionTexts["MoveLeft"] = leftButton;
        actionTexts["MoveRight"] = rightButton;
        actionTexts["Crouch"] = crouchButton;
       

        // Установка обработчика события для кнопки
        setKeyButton.onClick.AddListener(SetKey);

        // Обновление текста на кнопках в UI
        UpdateButtonLabels();
    }

    void Update()
    {
        PlayerPrefs.SetFloat(Sensitivity, sensitivitySlider.value);
        if (currentAction != null)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    keyBindings[currentAction] = key;
                    SaveSettings();
                    Debug.Log(key);
                    currentAction = null;
                    actionText.text = "";
                    UpdateButtonLabels(); // Обновляем текст на кнопках
                    break;
                }
            }
        }
    }

    public void SetKeyForAction(string action, TextMeshProUGUI text)
    {
        currentAction = action;
        actionText.text = $"Press a key for {action}";
    }

    private void SetKey()
    {
        if (currentAction != null)
        {
            actionText.text = $"Press a key for {currentAction}";
        }
    }

    public void OnShootButtonClicked()
    {
        SetKeyForAction("Shoot", actionTexts["Shoot"]);
    }

    public void OnFlashlightButtonClicked()
    {
        SetKeyForAction("Flashlight", actionTexts["Flashlight"]);
    }

    public void OnMoveForwardButtonClicked()
    {
        SetKeyForAction("MoveForward", actionTexts["MoveForward"]);
    }
    public void OnMoveBackwardButtonClicked()
    {
        SetKeyForAction("MoveBackward", actionTexts["MoveBackward"]);
    }

    public void OnMoveLeftButtonClicked()
    {
        SetKeyForAction("MoveLeft", actionTexts["MoveLeft"]);
    }

    public void OnMoveRightButtonClicked()
    {
        SetKeyForAction("MoveRight", actionTexts["MoveRight"]);
    }

    public void OnCrouchButtonClicked()
    {
        SetKeyForAction("Crouch", actionTexts["Crouch"]);
    }

    public void OnRunButtonClicked()
    {
        SetKeyForAction("Run", actionTexts["Run"]);
    }

    public void OnInteractButtonClicked()
    {
        SetKeyForAction("Interact", actionTexts["Interact"]);
    }

    public void SaveSettings()
    {
        foreach (var kvp in keyBindings)
        {
            PlayerPrefs.SetInt($"{kvp.Key}Key", (int)kvp.Value);
        }
        PlayerPrefs.SetFloat(Sensitivity, sensitivitySlider.value);
        PlayerPrefs.Save();
    }

    private void UpdateButtonLabels()
    {
        foreach (var kvp in keyBindings)
        {
            if (actionTexts.TryGetValue(kvp.Key, out TextMeshProUGUI text))
            {
                text.text = $"{kvp.Value}";
            }
        }
    }
}
