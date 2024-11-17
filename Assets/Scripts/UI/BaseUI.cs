using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public enum UIType
    {
        FullScreen,
        Popup,
    }

    public class BaseUI : MonoBehaviour
    {
        [field: SerializeField] public UIType UIType { get; private set; }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

