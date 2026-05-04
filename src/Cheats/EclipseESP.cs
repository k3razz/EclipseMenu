using UnityEngine;
using Sentry.Internal.Extensions;

namespace EclipseMenu;

public static class EclipseESP
{
    private static bool _freecamActive;
    private static bool _resolutionChangeNeeded;
    private static float _deltaTime;

    public static string PlayerColorDot(Color color)
    {
        if (!CheatToggles.showPlayerDots)
          return "";

        string hexColor = ColorUtility.ToHtmlStringRGB(color);
        return $"<size=80%><color=#{hexColor}>●</color></size>";
    }

    public static void SporeCloudVision(Mushroom mushroom)
    {
        Vector3 current = mushroom.sporeMask.transform.position;

        float targetZ = CheatToggles.noShadows ? -1f : 5f;

        mushroom.sporeMask.transform.position = new Vector3(
            current.x,
            current.y,
            targetZ
        );
    }

    public static bool IsFullbrightActive()
    {
        Camera cam = Camera.main;
        var follower = cam.GetComponent<FollowerCamera>();

        bool shadowsDisabled = CheatToggles.noShadows;
        bool zoomedOut = cam.orthographicSize > 3f;
        bool notFollowingPlayer = follower.Target != PlayerControl.LocalPlayer;

        return shadowsDisabled || zoomedOut || notFollowingPlayer;
    }

    public static void ZoomOut(HudManager hudManager)
    {
        if (!CheatToggles.zoomOut)
        {
            ResetZoom(hudManager);
            return;
        }

        bool chatOpen = hudManager.Chat.IsOpenOrOpening;
        bool customization = PlayerCustomizationMenu.Instance != null;

        bool lobbyBlock =
            Utils.isLobby &&
            (
                FriendsListUI.Instance.IsOpen ||
                GameStartManager.Instance.LobbyInfoPane.LobbyViewSettingsPane.gameObject.active ||
                GameStartManager.Instance.RulesEditPanel.active
            );

        if (chatOpen || customization || lobbyBlock)
            return;

        _resolutionChangeNeeded = true;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll == 0f)
            return;

        Camera cam = Camera.main;

        if (scroll < 0f)
        {
            cam.orthographicSize++;
            hudManager.UICamera.orthographicSize++;
        }
        else
        {
            if (cam.orthographicSize <= 3f)
                return;

            cam.orthographicSize--;
            hudManager.UICamera.orthographicSize--;
        }

        Utils.AdjustResolution();
    }

    private static void ResetZoom(HudManager hudManager)
    {
        Camera cam = Camera.main;

        cam.orthographicSize = 3f;
        hudManager.UICamera.orthographicSize = 3f;

        if (_resolutionChangeNeeded)
        {
            Utils.AdjustResolution();
            _resolutionChangeNeeded = false;
        }
    }

    public static void MeetingNametags(MeetingHud meetingHud)
    {
        try
        {
            foreach (var playerState in meetingHud.playerStates)
            {
                var data = GameData.Instance.GetPlayerById(playerState.TargetPlayerId);

                if (data.IsNull() ||
                    data.Disconnected ||
                    data.Outfits[PlayerOutfitType.Default].IsNull())
                    continue;

                Color color = Palette.PlayerColors[data.DefaultOutfit.ColorId];
                string dot = PlayerColorDot(color);

                string name = Utils.GetNameTag(data, data.DefaultOutfit.PlayerName);

                playerState.NameText.text = dot + " " + name;

                ApplyMeetingNameLayout(playerState.NameText.transform);
            }
        }
        catch { }
    }

    private static void ApplyMeetingNameLayout(Transform t)
    {
        if (CheatToggles.seeRoles && CheatToggles.seePlayerInfo)
        {
            t.localPosition = new Vector3(0.33f, 0.08f, 0f);
            t.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        }
        else if (CheatToggles.seeRoles || CheatToggles.seePlayerInfo)
        {
            t.localPosition = new Vector3(0.3384f, 0.1125f, -0.1f);
            t.localScale = new Vector3(0.9f, 1f, 1f);
        }
        else
        {
            t.localPosition = new Vector3(0.3384f, 0.0311f, -0.1f);
            t.localScale = new Vector3(0.9f, 1f, 1f);
        }
    }

    public static void PlayerNametags(PlayerPhysics playerPhysics)
    {
        try
        {
            var data = playerPhysics.myPlayer.Data;
            string name = playerPhysics.myPlayer.CurrentOutfit.PlayerName;

            Color color = Palette.PlayerColors[data.DefaultOutfit.ColorId];
            string dot = PlayerColorDot(color);

            playerPhysics.myPlayer.cosmetics.SetName(
                dot + " " + Utils.GetNameTag(data, name)
            );

            ApplyPlayerNameLayout(playerPhysics.myPlayer.cosmetics.nameText.transform);
        }
        catch { }
    }

    private static void ApplyPlayerNameLayout(Transform t)
    {
        if (CheatToggles.seeRoles && CheatToggles.seePlayerInfo)
            t.localPosition = new Vector3(0f, 0.186f, 0f);
        else if (CheatToggles.seeRoles || CheatToggles.seePlayerInfo)
            t.localPosition = new Vector3(0f, 0.093f, 0f);
        else
            t.localPosition = Vector3.zero;
    }

    public static void ChatNametags(ChatBubble chatBubble)
    {
        try
        {
            Color color = Palette.PlayerColors[chatBubble.playerInfo.DefaultOutfit.ColorId];
            string dot = PlayerColorDot(color);

            string name = Utils.GetNameTag(
                chatBubble.playerInfo,
                chatBubble.NameText.text,
                true
            );

            chatBubble.NameText.text = dot + " " + name;

            chatBubble.NameText.ForceMeshUpdate(true, true);

            float height =
                chatBubble.NameText.GetNotDumbRenderedHeight() +
                chatBubble.TextArea.GetNotDumbRenderedHeight();

            chatBubble.Background.size = new Vector2(
                5.52f,
                0.2f + height
            );

            chatBubble.MaskArea.size =
                chatBubble.Background.size - new Vector2(0f, 0.03f);
        }
        catch { }
    }

    public static void SeeGhostsCheat(PlayerPhysics playerPhysics)
    {
        try
        {
            bool isDead = playerPhysics.myPlayer.Data.IsDead;
            bool localAlive = !PlayerControl.LocalPlayer.Data.IsDead;

            if (isDead && localAlive)
            {
                playerPhysics.myPlayer.Visible = CheatToggles.seeGhosts;
            }
        }
        catch { }
    }

    public static void FreecamCheat()
    {
        Camera cam = Camera.main;
        FollowerCamera follower = cam.GetComponent<FollowerCamera>();

        if (CheatToggles.freecam)
        {
            if (!_freecamActive)
            {
                follower.enabled = false;
                follower.Target = null;
                _freecamActive = true;
            }

            PlayerControl.LocalPlayer.moveable = false;

            Vector3 move = new Vector3(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"),
                0f
            );
    
            cam.transform.position += move * (10f * Time.deltaTime);
        }
        else
        {
            if (!_freecamActive)
                return;
    
            PlayerControl.LocalPlayer.moveable = true;
    
            follower.enabled = true;
            follower.SetTarget(PlayerControl.LocalPlayer);

            _freecamActive = false;
        }
    }

    public static void DrawFPS()
    {
        if (!CheatToggles.showFPS)
            return;

        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;

        int fps = Mathf.CeilToInt(1.0f / _deltaTime);

        Color fpsColor =
            fps >= 60 ? Color.white :
            fps >= 30 ? Color.yellow :
            Color.red;

        string text = $"{fps} FPS";

        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 14,
            normal = { textColor = Color.white }
        };

        Vector2 size = style.CalcSize(new GUIContent(text));

        Rect bg = new Rect(10, 10, size.x + 10, size.y + 6);
        Rect label = new Rect(15, 13, 200, 30);

        GUI.color = new Color(0f, 0f, 0f, 0.6f);
        GUI.Box(bg, "");

        GUI.color = Color.white;
        GUI.Label(label, text, style);
    }
}
