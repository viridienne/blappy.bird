using System;
using Manager;
using UnityEngine;

public class AutoScrolling : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _speed;
    private Material _material;
    private static readonly int ScrollSpeed = Shader.PropertyToID("_ScrollSpeed");
    void Start()
    {
        _material = _spriteRenderer.material;
        SetScrollSpeed(_speed);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnAfterGameStateChanged += OnAfterGameStateChanged;
    }

    private void OnDisable()
    {
        if (GameManager.Instance) GameManager.Instance.OnAfterGameStateChanged -= OnAfterGameStateChanged;
    }

    private void OnAfterGameStateChanged(GameState state)
    {
        switch (state)
        {
            default:
            case GameState.Playing:
                SetScrollSpeed(_speed);
                break;
            case GameState.Lose:
                SetScrollSpeed(0);
                break;
        }
    }

    public void SetScrollSpeed(float speed)
    {
        if(Math.Abs(_material.GetVector(ScrollSpeed).x - speed) < 0.1f) return;
        _material.SetVector(ScrollSpeed, new Vector4(speed, 0, 0, 0));
    }
}
