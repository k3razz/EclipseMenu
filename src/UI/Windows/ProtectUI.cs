using UnityEngine;
using Il2CppSystem.Collections.Generic;

namespace EclipseMenu;

public class ProtectUI : MonoBehaviour
{
    public static int windowHeight = 300;
    public static int windowWidth = 500;

    private Rect _windowRect;
    private Vector2 _scrollPosition = Vector2.zero;

    public static List<PlayerControl> playersToProtect = new();

    private bool _keepEveryoneProtected;

    private void Start()
    {
        _windowRect = new Rect(
            Screen.width * 0.5f - windowWidth * 0.5f,
            Screen.height * 0.5f - windowHeight * 0.5f,
            windowWidth,
            windowHeight
        );
    }

    private void OnGUI()
    {
        if (!ShouldDrawUI())
            return;

        UIHelpers.ApplyUIColor();

        _windowRect = GUI.Window(
            (int)WindowId.ProtectUI,
            _windowRect,
            (GUI.WindowFunction)((id) => ProtectWindow(id)),
            "Protect Players"
        );
    }

    private bool ShouldDrawUI()
    {
        return CheatToggles.showProtectMenu &&
               (MenuUI.isGUIActive || EclipseMenu.menuKeepSubwindowsOpen.Value) &&
               !EclipseMenu.isPanicked;
    }

    private void ProtectWindow(int windowID)
    {
        GUILayout.BeginVertical();

        DrawPlayerList();

        DrawBottomBar();

        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    private void DrawPlayerList()
    {
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, true);

        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (!IsValidPlayer(player))
            {
                CleanupInvalidPlayer(player);
                continue;
            }

            GUILayout.BeginHorizontal();

            DrawPlayerName(player);
            DrawProtectionStatus(player);
            DrawProtectButton(player);
            DrawKeepToggle(player);

            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }

    private bool IsValidPlayer(PlayerControl player)
    {
        return player.Data &&
               player.Data.Role &&
               !string.IsNullOrEmpty(player.Data.PlayerName);
    }

    private void CleanupInvalidPlayer(PlayerControl player)
    {
        if (playersToProtect.Contains(player))
            playersToProtect.Remove(player);
    }

    private void DrawPlayerName(PlayerControl player)
    {
        string color = ColorUtility.ToHtmlStringRGB(player.Data.Color);

        GUILayout.Label(
            $"<color=#{color}>{player.Data.PlayerName}</color>",
            GUILayout.Width(140f)
        );
    }

    private void DrawProtectionStatus(PlayerControl player)
    {
        if (player.protectedByGuardianId == -1)
        {
            GUILayout.Label("<color=#FF0000>Unprotected</color>", GUILayout.Width(135));
            return;
        }

        var guardian = GameData.Instance.GetPlayerById((byte)player.protectedByGuardianId);

        string color = ColorUtility.ToHtmlStringRGB(guardian.Color);

        GUILayout.Label(
            $"<color=#00FF00>Protected</color> by <color=#{color}>{guardian._object.Data.PlayerName}</color>",
            GUILayout.Width(135)
        );
    }

    private void DrawProtectButton(PlayerControl player)
    {
        if (GUILayout.Button("Protect", GUIStylePreset.NormalButton) &&
            Utils.isHost &&
            !Utils.isLobby)
        {
            PlayerControl.LocalPlayer.RpcProtectPlayer(player, player.cosmetics.ColorId);
        }
    }

    private void DrawKeepToggle(PlayerControl player)
    {
        bool keep = playersToProtect.Contains(player);

        bool newKeep = GUILayout.Toggle(
            keep,
            "Keep protected",
            GUIStylePreset.NormalToggle
        );

        if (newKeep == keep)
            return;

        if (newKeep)
            playersToProtect.Add(player);
        else
            playersToProtect.Remove(player);
    }

    private void DrawBottomBar()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Protect Everyone") &&
            Utils.isHost &&
            !Utils.isLobby)
        {
            ProtectAllPlayers();
        }

        GUILayout.FlexibleSpace();

        DrawKeepEveryoneToggle();

        GUILayout.EndHorizontal();
    }

    private void ProtectAllPlayers()
    {
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            PlayerControl.LocalPlayer.RpcProtectPlayer(
                player,
                player.cosmetics.ColorId
            );
        }
    }

    private void DrawKeepEveryoneToggle()
    {
        _keepEveryoneProtected =
            GUILayout.Toggle(_keepEveryoneProtected, "Keep Everyone Protected");

        if (_keepEveryoneProtected)
        {
            SyncAllPlayersToList();
        }
        else
        {
            ClearIfFullySynced();
        }
    }

    private void SyncAllPlayersToList()
    {
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (!playersToProtect.Contains(player))
                playersToProtect.Add(player);
        }
    }

    private void ClearIfFullySynced()
    {
        if (PlayerControl.AllPlayerControls.Count == playersToProtect.Count)
        {
            playersToProtect.Clear();
        }
    }
}
