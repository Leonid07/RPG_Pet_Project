using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using Player;
using Inventory;
using System.Linq;
using UnityEngine.InputSystem;
using UtilShowMessage;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] PanelManager _panelManager;
    [SerializeField] PlayerCharacter _playerCharacter;
    [SerializeField] GameObject _characterPanel;
    [SerializeField] Text _menu;
    [SerializeField] GameObject _menuComand;
    [SerializeField] GameObject _characteristicPanel;
    [SerializeField] GameObject _panelCreation;
    [SerializeField] GameObject _panelTalentTree;
    [SerializeField] GameObject _mainMenu;

    [Header("Музыка для сцены")]
    [Range(0, 1)] public float MusicVolume = 0.5f;
    [SerializeField] AudioClip[] AudioMusic;

    public StarterAssetsInputs starterAssetsInputs;
    [Space(50)]
    public AddItems[] addItems;
    public ItemContainer itemContainer;

    public GameObject musicBox;
    AudioSource musicBoxAudioSource;

    PlayerInput playerInput;
    float startTime;
    void Awake()
    {
        startTime = Time.realtimeSinceStartup;
        starterAssetsInputs.SetCursorState(true);
    }

    void Start()
    {
        musicBoxAudioSource = musicBox.GetComponent<AudioSource>();
        playerInput = Player.GetComponent<PlayerInput>();
        float buildTime = Time.realtimeSinceStartup - startTime;
        Debug.Log("Время сборки сцены: " + buildTime.ToString("F2") + " сек."); 
        for (int i = 0; i < addItems.Length; i++)
        {
            for (int j = 0; j < addItems[i].Amount; j++)
            {
                itemContainer.AddItem(addItems[i].item);
            }
        }
        StartCoroutine(Music());
    }

    void Update()
    {
        // команды для управления игрой
        switch (Input.inputString)
        {
            //Добавление предметов в инвентарь
            case "t":
                for (int i = 0; i < addItems.Length; i++)
                {
                    for (int j = 0; j < addItems[i].Amount; j++)
                    {
                        itemContainer.AddItem(addItems[i].item);
                    }
                }
                break;
            case "i":
                CloseAndOpenPanel(_characteristicPanel.transform);
                break;
            case "k":
                CloseAndOpenPanel(_panelCreation.transform);
                break;
            case "l":
                CloseAndOpenPanel(_panelTalentTree.transform);
                break;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_panelManager.PanelUI.All(panel => panel.transform.localPosition != Vector3.zero))
                CloseAndOpenPanel(_mainMenu.transform);
            else
                CloseAllPanel();
        }
    }

    public void CloseAndOpenPanel(Transform Panel)
    {
        starterAssetsInputs.SetCursorState(false);
        ShowMessagePanel.EndAnimationMessage();
        Time.timeScale = 0f;
        playerInput.enabled = false;
        foreach (GameObject panel in _panelManager.PanelUI)
        {
            if (panel.name != Panel.name)
                Utils.HidePanel(panel.transform);
        }
        Utils.ShowAndHidePanel(Panel.transform);

        if (_panelManager.PanelUI.All(panel => panel.transform.localPosition != Vector3.zero))
        {
            Time.timeScale = 1f;
            starterAssetsInputs.SetCursorState(true);
            playerInput.enabled = true;
        }
    }
    public void CloseAllPanel()
    {
        foreach (GameObject Panel in _panelManager.PanelUI)
        {
            Utils.HidePanel(Panel.transform);
        }
        Time.timeScale = 1f;
        ShowMessagePanel.EndAnimationMessage();
        starterAssetsInputs.SetCursorState(true);
        playerInput.enabled = true;
    }

    IEnumerator Music()
    {
        int musicIndex = 0;
        while (true)
        {
            musicBoxAudioSource.clip = AudioMusic[musicIndex];
            musicBoxAudioSource.volume = MusicVolume;
            musicBoxAudioSource.Play();
            yield return new WaitForSeconds(AudioMusic[musicIndex].length);
            musicIndex++;
            if (musicIndex >= AudioMusic.Length)
            {
                musicIndex = 0;
            }
        }
    }

    [System.Serializable]
    public struct AddItems
    {
        public int Amount;
        public Item item;
    }
}