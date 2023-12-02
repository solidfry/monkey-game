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
            CheckButtonForNone();
        }

        private void Do()
        { 
            doEvent?.Invoke();
        }
        
        private void CheckButtonForNone()
        {
            if (button == KeyCode.None)
            {
                if(Input.anyKeyDown)
                    Do();
            }
            else if (Input.GetKeyDown(button))
            {
                Do();
            }
        }

    }
}