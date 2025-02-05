using UnityEngine;

public class BasicRigidBodyPush : MonoBehaviour
{
    [Tooltip("����, � �������� ��������������� ������")]
    public LayerMask pushLayers;
    [Tooltip("����, ������������, ����� �� ������ ������������ ������")]
    public bool canPush;
    [Tooltip("���� ������ (������������ �������� �� 0.5 �� 5)")]
    [Range(0.5f, 5f)] public float strength = 1.1f;

    // ���������� ��� ������������ ����������� � ������ �����������
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // ���� ������ ����� ������������ ������, ���������� ����� PushRigidBodies
        if (canPush) PushRigidBodies(hit);
    }

    // ������������ ������ �������� � �������� ������
    void PushRigidBodies(ControllerColliderHit hit)
    {
        // �������������, ��� �� ����������� � ��������� �����
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic) return;

        // �������������, ��� �� ������� ������ ������� ������� ���� (����)
        var bodyLayerMask = 1 << body.gameObject.layer;
        if ((bodyLayerMask & pushLayers.value) == 0) return;

        // �� ������� ������� ��� ����
        if (hit.moveDirection.y < -0.3f) return;

        // ��������� ����������� ������ �� ����������� ��������, ������ �������������� ��������
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);

        // ��������� ������, �������� ����
        body.AddForce(pushDir * strength, ForceMode.Impulse);
    }
}