using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
public class ItemCollectionArea : MonoBehaviour
{
    public KeyCode activationKey = KeyCode.Z;

    public Interactor interactor;
    private SphereCollider isColliderActive;
    private void Start()
    {
        isColliderActive = GetComponent<SphereCollider>();
    }
    private void Update()
    {
        if (Input.GetKey(activationKey))
        {
            isColliderActive.enabled = true;
        }
        else
        {
            isColliderActive.enabled = false;
        }
    }
}
