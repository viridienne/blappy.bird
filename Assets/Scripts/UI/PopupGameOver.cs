using System;
using Manager;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PopupGameOver : BaseUI
    {
        [SerializeField] private TMP_Text _textHighScore;
        [SerializeField] private TMP_Text _textScore;
        public Action OnRetry;
        public override void Show()
        {
            base.Show();
            _textScore.SetText(CheckPointsManager.Instance.Point.Value.ToString());
            _textHighScore.SetText(PlayerPrefs.GetInt("HighScore").ToString());
        }

        public void OnButtonRetry()
        {
            OnRetry?.Invoke();
            Hide();
        }

        public override void Hide()
        {
            base.Hide();
            OnRetry = null;
        }
    }
}

