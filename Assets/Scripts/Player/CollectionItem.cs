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

    [Header("��������� ��� ����")]
    public float RayLength = 7f; // ������� �������� ����� ����
    public LayerMask HitLayer; // ������� ����, � �������� ��� ����� �����������������
    public GameObject Object;
    private bool isAnimationPlaying = false; // ����, �����������, ������������� �� ��������

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
        // �������� ����� ������
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // �������� ����� � ������� �����������, ��������������� ������ ������
        rayOrigin = mainCamera.ScreenToWorldPoint(screenCenter);

        // ���������� ����������� ���� (� ���� ������, ����������� ������)
        rayDirection = mainCamera.transform.forward;

        // ������� ���
        ray = new Ray(rayOrigin, rayDirection);

        // ��������� ������� ���
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

                isAnimationPlaying = false; // ���������� ����, ���� ������ ���������
                mining = Object.GetComponent<MiningAndCreation>();
                
            }
            else
            {
                // ���������, �� ������������� �� ��������
                if (!isAnimationPlaying)
                {
                    ShowMessagePanel.ShowMessagee();

                    isAnimationPlaying = true; // ������������� ����, ����� �������� ���������� ������� ��������
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
            isAnimationPlaying = false; // ���� ��� �� ����� �� ������, ���������� ����
            Object = null;
        }

        Debug.DrawRay(ray.origin, ray.direction * RayLength, Color.red);
    }
}