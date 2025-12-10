using DanielLochner.Assets.SimpleSideMenu;
using UnityEngine;

// Controls the debug side menu. Hook button events to these public methods.
// References are assigned in the Inspector; no runtime lookups are used.
public class DebugMenuController : MonoBehaviour
{
    [Header("Required")]
    [SerializeField] private SimpleSideMenu sideMenu;

    [Header("State")]
    [SerializeField] private bool startOpen = false;

    private void Awake()
    {
        if (sideMenu == null)
        {
            Debug.LogError("DebugMenuController is missing a SimpleSideMenu reference.", this);
            enabled = false;
        }
    }

    private void Start()
    {
        if (!enabled)
        {
            return;
        }

        // Ensure the menu starts in the desired state.
        if (startOpen)
        {
            sideMenu.Open();
        }
        else
        {
            sideMenu.Close();
        }
    }

    // Call from UI Button or UnityEvent to open the menu.
    public void OpenMenu()
    {
        if (!enabled) return;
        sideMenu.Open();
    }

    // Call from UI Button or UnityEvent to close the menu.
    public void CloseMenu()
    {
        if (!enabled) return;
        sideMenu.Close();
    }

    // Call from UI Button or UnityEvent to toggle the menu.
    public void ToggleMenu()
    {
        if (!enabled) return;
        sideMenu.ToggleState();
    }
}
