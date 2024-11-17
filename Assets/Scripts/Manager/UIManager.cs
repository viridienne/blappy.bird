using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Manager
{
    public class UIManager : BaseSingletonMono<UIManager>
    {
        [SerializeField] private UIDatabase _uiDatabase;
        [SerializeField] private Transform _overlayContainer;
        [SerializeField] private Transform _popupContainer;
        
        private readonly Dictionary<UIKey, BaseUI> _uiDictionary = new();
        
        private BaseUI CreateUI(UIKey uiKey)
        {
            if (!_uiDatabase.UDictionary.ContainsKey(uiKey))
            {
                Debug.LogError($"UIKey {uiKey} not found in UIDatabase");
                return null;
            }

            var ui = Instantiate(_uiDatabase.UDictionary[uiKey], _overlayContainer);
            ui.gameObject.SetActive(false);

            var type = ui.UIType;
            Transform parent;
            switch (type)
            {
                case UIType.Popup:
                    parent = _popupContainer;
                    break;
                case UIType.FullScreen:
                default: 
                    parent = _overlayContainer;
                    break;
            }
            var tf = ui.transform;
            tf.SetParent(parent);
            tf.localScale = Vector3.one;
            tf.localPosition = Vector3.zero;
            tf.localRotation = Quaternion.identity;
            return ui;
        }
        public void ShowUI(UIKey uiKey, Action<BaseUI> onComplete = null)
        {
            if (!_uiDictionary.ContainsKey(uiKey))
            {
                var newUI = CreateUI(uiKey);
                if (newUI == null)
                {
                    onComplete?.Invoke(null);
                    return;
                }
                _uiDictionary.TryAdd(uiKey, newUI);
            }
            
            var ui = _uiDictionary[uiKey];
            ui.Show();
            ui.transform.SetAsLastSibling();
            onComplete?.Invoke(ui);
        }
        
        public void HideUI(UIKey uiKey, Action<BaseUI> onComplete = null)
        {
            if (!_uiDictionary.ContainsKey(uiKey))
            {
                Debug.LogError($"UIKey {uiKey} not found in UIDatabase");
                onComplete?.Invoke(null);
                return;
            }
            
            var ui = _uiDictionary[uiKey];
            ui.Hide();
            onComplete?.Invoke(ui);
        }
    }
}

