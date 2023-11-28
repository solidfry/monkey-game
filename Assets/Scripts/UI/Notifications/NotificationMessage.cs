using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Notifications
{
    public class NotificationMessage : MonoBehaviour
    {
        [SerializeField] NotificationData notificationData;
        
        private string _title;
        private string _description;
        [SerializeField] TMP_Text titleText;
        [SerializeField] TMP_Text descriptionText;

        [SerializeField] Image icon;
        [SerializeField] RectTransform iconBase;


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

            SetTextValue(titleText, _title);
            SetTextValue(descriptionText, _description);
            SetIcon(icon, data.Icon, data.IconColor);
        }

        public void SetTextValue(TMP_Text textField, string value)
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

        public void SetIcon(Image iconField, Sprite icon, Color iconColor)
        {
            if (icon != null)
            {
                iconField.sprite = icon;
                iconField.color = iconColor;
            }
            else
            {
                iconBase.gameObject.SetActive(false);
            }
        }
    }
}
