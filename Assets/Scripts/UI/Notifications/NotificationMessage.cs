using TMPro;
using UnityEngine;

namespace UI.Notifications
{
    public class NotificationMessage : MonoBehaviour
    {
        private string _message;
        [SerializeField] TMP_Text messageText;
        [field: SerializeField] public RectTransform RectTransform { get; private set; }
        [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }

        private void Awake()
        {
            if(RectTransform == null) RectTransform = GetComponent<RectTransform>();
            if(CanvasGroup == null) CanvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            if(messageText == null)
                messageText = GetComponentInChildren<TMP_Text>();
        }

        public void SetMessage(string message)
        {
            this._message = message;
            messageText.text = _message;
        }
    }
}
