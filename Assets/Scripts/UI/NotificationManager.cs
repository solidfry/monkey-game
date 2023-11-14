using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UI.Notifications;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NotificationManager : SingletonPersistent<NotificationManager>
    {
        private Queue<string> _notificationQueue = new ();
        [SerializeField] VerticalLayoutGroup layoutGroup;
    
        readonly List<NotificationMessage> _liveMessages = new();
        [SerializeField] private int maxMessages = 2;
    
        [Header("Message Values")]
        [SerializeField] private NotificationMessage notificationMessagePrefab;
        [SerializeField] private float messageDuration = 5f;
    
        [Header("Message Animation Values")]
        [SerializeField] private Ease easing;
        [SerializeField] private float animationDuration = 1f;
        
        public delegate void UINotificationEvent(string message);
        public event UINotificationEvent OnUINotificationEvent;

        private void OnEnable()
        {
            OnUINotificationEvent += OnNotificationEvent;
        }
    
        private void OnDisable()
        {
            OnUINotificationEvent -= OnNotificationEvent;
        }
    
        private void OnNotificationEvent(string message)
        {
            if (!_notificationQueue.Contains(message))
            {
                _notificationQueue.Enqueue(message);
                TryDisplayNextMessage();
            }
        }
    
        private void TryDisplayNextMessage()
        {
            // Only proceed if there are messages in the queue and less than the max on screen.
            if (_liveMessages.Count < maxMessages && _notificationQueue.Count > 0)
            {
                string nextMessage = _notificationQueue.Dequeue();
                var notificationMessage = CreateMessage(nextMessage, layoutGroup.transform);
                _liveMessages.Add(notificationMessage);
                AnimateMessageIn(notificationMessage);
                StartCoroutine(RemoveMessage(notificationMessage));
            }
        }

        private void DequeueDuplicateMessage(string message)
        {
            _notificationQueue = new Queue<string>(_notificationQueue.Where(m => m != message));
        }


        IEnumerator RemoveMessage(NotificationMessage notificationMessage)
        {
            yield return new WaitForSeconds(messageDuration);
            AnimateMessageOut(notificationMessage);
            _liveMessages.Remove(notificationMessage); // Remove the reference immediately
            // After fading out, try displaying the next message in the queue
            TryDisplayNextMessage();
        }

        void AnimateMessageOut(NotificationMessage notificationMessage)
        {
            notificationMessage.RectTransform.DOScaleY(0, animationDuration / 2).From(1).SetEase(easing);
            notificationMessage.CanvasGroup.DOFade(0, animationDuration).OnComplete(() =>
            {
                Destroy(notificationMessage.gameObject);
            });
        }

    
        void AnimateMessageIn(NotificationMessage notificationMessage)
        {
            notificationMessage.RectTransform.DOScaleY(1, animationDuration/2).From(0).SetEase(easing);
            notificationMessage.CanvasGroup.DOFade(1, animationDuration);
        }
    

        private NotificationMessage CreateMessage(string message, Transform parent)
        {
            var notificationMessage = Instantiate(notificationMessagePrefab, parent);
            notificationMessage.CanvasGroup.alpha = 0;
            notificationMessage.SetMessage(message);
            return notificationMessage;
        }
    
        [ContextMenu("Test Notification")]
        public void TestNotification()
        {
            DequeueDuplicateMessage("Test Notification");
            OnUINotificationEvent?.Invoke("Test Notification");
            Debug.Log("Test notification was invoked");
        }
    
        private void OnDestroy()
        {
            StopAllCoroutines();
            _notificationQueue.Clear();
            _liveMessages.Clear();
        }
    }
}
