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
            _textScore.SetText("Score: "+CheckPointsManager.Instance.Point.Value);
            _textHighScore.SetText("Highest Score: " +PlayerPrefs.GetInt("HighScore"));
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

