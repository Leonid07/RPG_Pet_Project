using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using Inventory;
using UtilShowMessage;

public class CollectionItem : MonoBehaviour
{
    public MiningAndCreation mining;
    public GameManager gameManager;
    public ItemContainer itemContainer;
    public Interactor interactor;
    public GameObject CrafPanel;
    public KeyCode activationKey = KeyCode.E;

    [Header("Настройки для луча")]
    public float RayLength = 7f; // Задайте желаемую длину луча
    public LayerMask HitLayer; // Задайте слои, с которыми луч будет взаимодействовать
    public GameObject Object;
    private bool isAnimationPlaying = false; // Флаг, указывающий, проигрывается ли анимация

    private Camera mainCamera;

    Vector3 screenCenter;
    Vector3 rayOrigin;
    Vector3 rayDirection;
    Ray ray;
    RaycastHit hit;
    void Start()
    {
        mainCamera = Camera.main;
        ray = new Ray();
        hit = new RaycastHit();
    }
    void Update()
    {
        // Получаем центр экрана
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Получаем точку в мировых координатах, соответствующую центру экрана
        rayOrigin = mainCamera.ScreenToWorldPoint(screenCenter);

        // Определяем направление луча (в этом случае, направление камеры)
        rayDirection = mainCamera.transform.forward;

        // Создаем луч
        ray = new Ray(rayOrigin, rayDirection);

        // Выполняем лучевой луч
        if (Physics.Raycast(ray, out hit, RayLength, HitLayer))
        {
            if (Object != hit.collider.gameObject)
            {
                Object = hit.collider.gameObject;

                if (hit.collider.GetComponent<MiningAndCreation>())
                {
                    ShowMessagePanel.SetMessage(hit.collider.GetComponent<MiningAndCreation>().Name);
                }
                else
                {
                    ShowMessagePanel.SetMessage(hit.collider.GetComponent<NameObject>().Name);
                }

                isAnimationPlaying = false; // Сбрасываем флаг, если объект изменился
                mining = Object.GetComponent<MiningAndCreation>();
                
            }
            else
            {
                // Проверяем, не проигрывается ли анимация
                if (!isAnimationPlaying)
                {
                    ShowMessagePanel.ShowMessagee();

                    isAnimationPlaying = true; // Устанавливаем флаг, чтобы избежать повторного запуска анимации
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    ShowMessagePanel.EndAnimationMessage();
                    if (mining != null && mining.CollectiveResources)
                        mining.CollectResurce(itemContainer);
                    if (Object.tag == "Anvil")
                    {
                        gameManager.CloseAndOpenPanel(CrafPanel.transform);
                    }
                    if (Object.tag == "Item")
                    {
                        interactor.AddToInventory(Object.GetComponent<ItemCollector>().GetItem(), Object);
                    }
                }
            }
        }
        else
        {
            isAnimationPlaying = false; // Если луч не попал на объект, сбрасываем флаг
            Object = null;
        }

        Debug.DrawRay(ray.origin, ray.direction * RayLength, Color.red);
    }
}