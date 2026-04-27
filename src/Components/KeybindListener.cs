using UnityEngine;

namespace EclipseMenu;

public class KeybindListener : MonoBehaviour
{
    public void Update()
    {
        if (EclipseMenu.isPanicked)
            return;

        if (IsTypingInChat())
            return;

        ProcessKeybinds();
    }

    private bool IsTypingInChat()
    {
        if (!HudManager.InstanceExists)
            return false;

        var chat = HudManager.Instance.Chat;

        if (chat == null)
            return false;

        return chat.IsOpenOrOpening;
    }

    private void ProcessKeybinds()
    {
        foreach (var entry in CheatToggles.Keybinds)
        {
            var name = entry.Key;
            var key = entry.Value;

            if (key == KeyCode.None)
                continue;

            if (!Input.GetKeyDown(key))
                continue;

            if (!CheatToggles.ToggleFields.TryGetValue(name, out var field))
                continue;

            ToggleField(field);
        }
    }

    private void ToggleField(System.Reflection.FieldInfo field)
    {
        bool currentValue = (bool)field.GetValue(null);
        field.SetValue(null, !currentValue);
    }
}
