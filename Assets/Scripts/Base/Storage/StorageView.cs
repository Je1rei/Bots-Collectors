using UnityEngine;
using TMPro;

public class AppleStorageView : MonoBehaviour 
{
    [SerializeField] private AppleStorage _storage;
    [SerializeField] private TMP_Text _countText;

    private void OnEnable()
    {
        _storage.Changed += SetCountText;
    }

    private void OnDisable()
    {
        _storage.Changed -= SetCountText;
    }

    private void SetCountText(int value)
    {
        _countText.text = value.ToString();
    }
}
