using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowMessage : MonoBehaviour
{
    [Header("������� ��� ���������")]
    public Image _TextObject;
    public TMP_Text _TextMessage;
    public static TMP_Text textMessage;
    [Header("��������� ������ ���������")]
    public static float _DelayMessage = 2;
    public float DelayMessage = 2;

    public static float _SpeedFadeOff = 0.5f;
    public float SpeedFadeOff = 0.5f;

    public static float _SpeedFadeOn = 0.5f;
    public float SpeedFadeOn = 0.5f;

    private static CanvasGroup canvasGroup;
    static Sequence sequence;
    private void Start()
    {
        canvasGroup = _TextObject.GetComponent<CanvasGroup>();
        textMessage = _TextMessage;
        _DelayMessage = DelayMessage;
        _SpeedFadeOff = SpeedFadeOff;
        _SpeedFadeOn = SpeedFadeOn;
    }
    public static void ShowMessagePanel()
    {
        // ������������� ��������� �������� ������������
        canvasGroup.alpha = 0f;
        // ������� ������������������ �������� � DOTween
        sequence = DOTween.Sequence();
        // ��������: ���������� ������������ �� 1 (��������� ������������)
        sequence.Append(canvasGroup.DOFade(1f, _SpeedFadeOn));
        // �������� � 3 �������
        sequence.AppendInterval(_DelayMessage);
        // ��������: ���������� ������������ �� 0 (��������� ����������)
        sequence.Append(canvasGroup.DOFade(0f, _SpeedFadeOff));
        // ������ ��������
        sequence.Play();
    }
    public static void EndAnimation()
    {    
        // ���������, ��� ������������������ ���� ������� � ��������
        if (sequence != null && sequence.IsPlaying())
        {
            sequence.Kill();            // ������������� ������������������
            canvasGroup.alpha = 0;            // ������������� ������������ � 0
        }
    }
}
namespace UtilShowMessage
{
    public abstract class ShowMessagePanel
    {
        public static void ShowMessagee()
        {
            ShowMessage.ShowMessagePanel();
        }
        public static void EndAnimationMessage()
        {
            ShowMessage.EndAnimation();
        }
        public static void SetMessage(string message)
        {
            ShowMessage.textMessage.text = message;
        }
    }
}