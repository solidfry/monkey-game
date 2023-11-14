using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class UIKeyPressEvent : MonoBehaviour
    {

        [SerializeField] KeyCode button;
    
        [SerializeField] UnityEvent doEvent;

        private void Update()
        {
            if (Input.GetKeyDown(button))
            {
                Do();
            }
        }

        private void Do()
        { 
            doEvent?.Invoke();
        }

    }
}