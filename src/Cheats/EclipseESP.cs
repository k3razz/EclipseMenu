using UnityEngine;
using Sentry.Internal.Extensions;

namespace EclipseMenu;

public static class EclipseESP
{
    private static bool _freecamActive;
    private static bool _resolutionChangeNeeded;

    public static string PlayerColorDot(Color color)
    {
        string hex = ColorUtility.ToHtmlStringRGB(color);
        return $"<size=80%><color=#{hex}>●</color></size>";
    }

    public static void SporeCloudVision(Mushroom mushroom)
    {
        if (CheatToggles.noShadows)
        {
            mushroom.sporeMask.transform.position = new Vector3(
                mushroom.sporeMask.transform.position.x,
                mushroom.sporeMask.transform.position.y,
                -1
            );
            return;
        }

        mushroom.sporeMask.transform.position = new Vector3(
            mushroom.sporeMask.transform.position.x,
            mushroom.sporeMask.transform.position.y,
            5f
        );
    }

    public static bool IsFullbrightActive()
    {
        return CheatToggles.noShadows ||
               Camera.main.orthographicSize > 3f ||
               Camera.main.gameObject.GetComponent<FollowerCamera>().Target != PlayerControl.LocalPlayer;
    }

    public static void ZoomOut(HudManager hudManager)
    {
        if (CheatToggles.zoomOut)
        {
            if (
                hudManager.Chat.IsOpenOrOpening ||
                PlayerCustomizationMenu.Instance != null ||
                (Utils.isLobby && (
                    FriendsListUI.Instance.IsOpen ||
                    GameStartManager.Instance.LobbyInfoPane.LobbyViewSettingsPane.gameObject.active ||
                    GameStartManager.Instance.RulesEditPanel.active
                ))
            )
                return;

            _resolutionChangeNeeded = true;

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                Camera.main.orthographicSize++;
                hudManager.UICamera.orthographicSize++;
                Utils.AdjustResolution();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (Camera.main.orthographicSize <= 3f) return;

                Camera.main.orthographicSize--;
                hudManager.UICamera.orthographicSize--;
                Utils.AdjustResolution();
            }
        }
        else
        {
            Camera.main.orthographicSize = 3f;
            hudManager.UICamera.orthographicSize = 3f;

            if (_resolutionChangeNeeded)
            {
                Utils.AdjustResolution();
                _resolutionChangeNeeded = false;
            }
        }
    }

    public static void MeetingNametags(MeetingHud meetingHud)
    {
        try
        {
            foreach (var playerState in meetingHud.playerStates)
            {
                var data = GameData.Instance.GetPlayerById(playerState.TargetPlayerId);

                if (data.IsNull() || data.Disconnected || data.Outfits[PlayerOutfitType.Default].IsNull())
                    continue;

                Color playerColor = Palette.PlayerColors[data.DefaultOutfit.ColorId];
                string dot = PlayerColorDot(playerColor);

                playerState.NameText.text =
                    dot + " " + Utils.GetNameTag(data, data.DefaultOutfit.PlayerName);

                if (CheatToggles.seeRoles && CheatToggles.seePlayerInfo)
                {
                    playerState.NameText.transform.localPosition = new Vector3(0.33f, 0.08f, 0f);
                    playerState.NameText.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                }
                else if (CheatToggles.seeRoles || CheatToggles.seePlayerInfo)
                {
                    playerState.NameText.transform.localPosition = new Vector3(0.3384f, 0.1125f, -0.1f);
                    playerState.NameText.transform.localScale = new Vector3(0.9f, 1f, 1f);
                }
                else
                {
                    playerState.NameText.transform.localPosition = new Vector3(0.3384f, 0.0311f, -0.1f);
                    playerState.NameText.transform.localScale = new Vector3(0.9f, 1f, 1f);
                }
            }
        }
        catch { }
    }

    public static void PlayerNametags(PlayerPhysics playerPhysics)
    {
        try
        {
            var data = playerPhysics.myPlayer.Data;
            string name = playerPhysics.myPlayer.CurrentOutfit.PlayerName;

            Color playerColor = Palette.PlayerColors[data.DefaultOutfit.ColorId];
            string dot = PlayerColorDot(playerColor);

            playerPhysics.myPlayer.cosmetics.SetName(
                dot + " " + Utils.GetNameTag(data, name)
            );

            if (CheatToggles.seeRoles && CheatToggles.seePlayerInfo)
            {
                playerPhysics.myPlayer.cosmetics.nameText.transform.localPosition = new Vector3(0f, 0.186f, 0f);
            }
            else if (CheatToggles.seeRoles || CheatToggles.seePlayerInfo)
            {
                playerPhysics.myPlayer.cosmetics.nameText.transform.localPosition = new Vector3(0f, 0.093f, 0f);
            }
            else
            {
                playerPhysics.myPlayer.cosmetics.nameText.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
        }
        catch { }
    }

    public static void ChatNametags(ChatBubble chatBubble)
    {
        try
        {
            Color playerColor = Palette.PlayerColors[chatBubble.playerInfo.DefaultOutfit.ColorId];
            string dot = PlayerColorDot(playerColor);

            chatBubble.NameText.text =
                dot + " " + Utils.GetNameTag(chatBubble.playerInfo, chatBubble.NameText.text, true);

            chatBubble.NameText.ForceMeshUpdate(true, true);

            chatBubble.Background.size = new Vector2(
                5.52f,
                0.2f +
                chatBubble.NameText.GetNotDumbRenderedHeight() +
                chatBubble.TextArea.GetNotDumbRenderedHeight()
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
            if (playerPhysics.myPlayer.Data.IsDead &&
                !PlayerControl.LocalPlayer.Data.IsDead)
            {
                playerPhysics.myPlayer.Visible = CheatToggles.seeGhosts;
            }
        }
        catch { }
    }

    public static void FreecamCheat()
    {
        if (CheatToggles.freecam)
        {
            if (!_freecamActive)
            {
                var cam = Camera.main.GetComponent<FollowerCamera>();
                cam.enabled = false;
                cam.Target = null;
                _freecamActive = true;
            }

            PlayerControl.LocalPlayer.moveable = false;

            var movement = new Vector3(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"),
                0f
            );

            Camera.main.transform.position += movement * 10f * Time.deltaTime;
        }
        else
        {
            if (!_freecamActive) return;

            PlayerControl.LocalPlayer.moveable = true;

            var cam = Camera.main.GetComponent<FollowerCamera>();
            cam.enabled = true;
            cam.SetTarget(PlayerControl.LocalPlayer);

            _freecamActive = false;
        }
    }
     private static float _deltaTime;

    public static void DrawFPS()
    {
        if (!CheatToggles.showFPS) return;

        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        int fps = Mathf.CeilToInt(1.0f / _deltaTime);
    
        Color fpsColor = fps >= 60 ? Color.white :
                          fps >= 30 ? Color.yellow :
                          Color.red;
    
        string dot = PlayerColorDot(fpsColor);
    
        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 14,
            normal = { textColor = Color.white }
        };

        string text = $"{fps} FPS";

        Vector2 size = style.CalcSize(new GUIContent(text));

        GUI.color = new Color(0f, 0f, 0f, 0.6f);
        GUI.Box(new Rect(10, 10, size.x + 10, size.y + 6), "");

        GUI.color = Color.white;
        GUI.Label(new Rect(15, 13, 200, 30), text, style);
    }
}
