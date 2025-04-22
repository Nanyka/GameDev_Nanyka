using TheAiAlchemist;
using UnityEngine;

public class LoadingInterfaceController: MonoBehaviour
{
    [SerializeField] private GameObject _loadingInterface = default;

    [Header("Listening on")]
    [SerializeField] private BoolChannel _toggleLoadingScreen = default;

    private void OnEnable()
    {
        _toggleLoadingScreen.AddListener(ToggleLoadingScreen);
    }

    private void OnDisable()
    {
        _toggleLoadingScreen.RemoveListener(ToggleLoadingScreen);
    }

    private void ToggleLoadingScreen(bool state)
    {
        _loadingInterface.SetActive(state);
    }
}