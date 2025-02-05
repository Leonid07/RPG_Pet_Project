using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float smoothTime = 0.2f;
        public float rotationSpeed = 3f; // �������� �������� ������
        public float cameraDistance = 10f; // ��������� ������

        private Vector3 offset;
        private Vector3 currentVelocity;

        void Start()
        {
            offset = transform.position - target.position;
        }

        void LateUpdate()
        {
            if (Input.GetMouseButton(1))
            {
                float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;

                // �������� ����������� ������� offset ������ ��� Y
                Quaternion rotation = Quaternion.Euler(0f, mouseX, 0f);
                offset = rotation * offset;
            }

            Vector3 desiredPosition = target.position + offset.normalized * cameraDistance; // ���������� ��������������� ������ offset ��� ����������� ��������� ������
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);
            transform.position = smoothedPosition;

            transform.LookAt(target);
        }
    }
}