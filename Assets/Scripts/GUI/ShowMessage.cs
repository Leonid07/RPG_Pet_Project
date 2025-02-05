using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowMessage : MonoBehaviour
{
    [Header("Объекты для сообщения")]
    public Image _TextObject;
    public TMP_Text _TextMessage;
    public static TMP_Text textMessage;
    [Header("Настройки показа сообщений")]
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
        // Устанавливаем начальное значение прозрачности
        canvasGroup.alpha = 0f;
        // Создаем последовательность анимаций с DOTween
        sequence = DOTween.Sequence();
        // Анимация: увеличение прозрачности до 1 (полностью непрозрачный)
        sequence.Append(canvasGroup.DOFade(1f, _SpeedFadeOn));
        // Задержка в 3 секунды
        sequence.AppendInterval(_DelayMessage);
        // Анимация: уменьшение прозрачности до 0 (полностью прозрачный)
        sequence.Append(canvasGroup.DOFade(0f, _SpeedFadeOff));
        // Начать анимацию
        sequence.Play();
    }
    public static void EndAnimation()
    {    
        // Проверяем, что последовательность была создана и запущена
        if (sequence != null && sequence.IsPlaying())
        {
            sequence.Kill();            // Останавливаем последовательность
            canvasGroup.alpha = 0;            // Устанавливаем прозрачность в 0
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