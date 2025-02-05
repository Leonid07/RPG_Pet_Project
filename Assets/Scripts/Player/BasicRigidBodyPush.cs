using UnityEngine;

public class BasicRigidBodyPush : MonoBehaviour
{
    [Tooltip("Слои, с которыми взаимодействует скрипт")]
    public LayerMask pushLayers;
    [Tooltip("Флаг, определяющий, может ли объект осуществлять толчок")]
    public bool canPush;
    [Tooltip("Сила толчка (регулируемая величина от 0.5 до 5)")]
    [Range(0.5f, 5f)] public float strength = 1.1f;

    // Вызывается при столкновении контроллера с другим коллайдером
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Если объект может осуществлять толчок, вызывается метод PushRigidBodies
        if (canPush) PushRigidBodies(hit);
    }

    // Осуществляет толчок объектов с жесткими телами
    void PushRigidBodies(ControllerColliderHit hit)
    {
        // Удостоверимся, что мы столкнулись с нежестким телом
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic) return;

        // Удостоверимся, что мы толкаем только объекты нужного слоя (слои)
        var bodyLayerMask = 1 << body.gameObject.layer;
        if ((bodyLayerMask & pushLayers.value) == 0) return;

        // Не толкаем объекты под нами
        if (hit.moveDirection.y < -0.3f) return;

        // Вычисляем направление толчка от направления движения, только горизонтальное движение
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);

        // Применяем толчок, учитывая силу
        body.AddForce(pushDir * strength, ForceMode.Impulse);
    }
}