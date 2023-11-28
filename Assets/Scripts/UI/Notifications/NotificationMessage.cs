using TMPro;
using UnityEngine;

namespace UI.Notifications
{
    public class NotificationMessage : MonoBehaviour
    {
        [SerializeField] NotificationData notificationData;
        
        private string _title;
        private string _description;
        [SerializeField] TMP_Text titleText;
        [SerializeField] TMP_Text descriptionText;


        [field: SerializeField] public RectTransform RectTransform { get; private set; }
        [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }


        private void Awake()
        {
            if(RectTransform == null) RectTransform = GetComponent<RectTransform>();
            if(CanvasGroup == null) CanvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetMessage(NotificationData data)
        {
            if(data == null)
            {
                Debug.LogError("No data was provided");
                return;
            }

            notificationData = data;

            _title = notificationData.Title;
            _description = notificationData.Description;

            SetValue(titleText, _title);
            SetValue(descriptionText, _description);
        }

        public void SetValue(TMP_Text textField, string value)
        {
            if (value != string.Empty)
            {
                textField.text = value;
            }
            else
            {
                textField.text = string.Empty;
                textField.gameObject.SetActive(false);
            }
        }
    }
}
