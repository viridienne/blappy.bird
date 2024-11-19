using UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHandle : Button
{
    private AutoPlayButton _autoPlayButton;
    protected override void Awake()
    {
        base.Awake();
        _autoPlayButton = GetComponent<AutoPlayButton>();
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        _autoPlayButton.OnClick(() =>
        {
            base.OnSubmit(eventData);
        });
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        _autoPlayButton.OnClick(() =>
        {
            base.OnPointerClick(eventData);
        });
    }
}
