using UnityEngine;
using Sentry.Internal.Extensions;
using TMPro;
using System.Linq;

namespace EclipseMenu;

public static class EclipseESP
{
    private static bool _freecamActive;
    private static bool _resolutionChangeNeeded;

    private static Rect _hostRect;

    public static string PlayerColorDot(int colorId)
    {
        if (!CheatToggles.showPlayerDots)
            return "";

        Color color = Palette.PlayerColors[colorId];

        string hexColor = ColorUtility.ToHtmlStringRGB(color);

        return $"<size=80%><color=#{hexColor}>●</color></size> ";
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
            if (hudManager.Chat.IsOpenOrOpening ||
                PlayerCustomizationMenu.Instance ||
                (Utils.isLobby &&
                (FriendsListUI.Instance.IsOpen ||
                 GameStartManager.Instance.LobbyInfoPane.LobbyViewSettingsPane.gameObject.active ||
                 GameStartManager.Instance.RulesEditPanel)))
                return;

            _resolutionChangeNeeded = true;

            float scroll = Input.GetAxis("Mouse ScrollWheel");

            Camera cam = Camera.main;

            if (scroll < 0f)
            {
                cam.orthographicSize++;
                hudManager.UICamera.orthographicSize++;
            }
            else if (scroll > 0f)
            {
                if (cam.orthographicSize <= 3f)
                    return;

                cam.orthographicSize--;
                hudManager.UICamera.orthographicSize--;
            }

            Utils.AdjustResolution();
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

                playerState.NameText.text =
                    PlayerColorDot(data.DefaultOutfit.ColorId) +
                    Utils.GetNameTag(data, data.DefaultOutfit.PlayerName);

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
            playerPhysics.myPlayer.cosmetics.SetName(
                PlayerColorDot(playerPhysics.myPlayer.Data.DefaultOutfit.ColorId) +
                Utils.GetNameTag(
                    playerPhysics.myPlayer.Data,
                    playerPhysics.myPlayer.CurrentOutfit.PlayerName
                )
            );

            if (CheatToggles.seeRoles && CheatToggles.seePlayerInfo)
                playerPhysics.myPlayer.cosmetics.nameText.transform.localPosition = new Vector3(0f, 0.186f, 0f);
            else if (CheatToggles.seeRoles || CheatToggles.seePlayerInfo)
                playerPhysics.myPlayer.cosmetics.nameText.transform.localPosition = new Vector3(0f, 0.093f, 0f);
            else
                playerPhysics.myPlayer.cosmetics.nameText.transform.localPosition = Vector3.zero;
        }
        catch { }
    }

    public static void ChatNametags(ChatBubble chatBubble)
    {
        try
        {
            chatBubble.NameText.text =
                PlayerColorDot(chatBubble.playerInfo.DefaultOutfit.ColorId) +
                Utils.GetNameTag(chatBubble.playerInfo, chatBubble.NameText.text, true);

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
    
    private static float _lastPing;

    public static void DrawHost()
    {
        if (!CheatToggles.drawHost)
            return;

        try
        {
            PlayerControl host = null;

            foreach (var p in PlayerControl.AllPlayerControls)
            {
                if (p.OwnerId == AmongUsClient.Instance.HostId)
                {
                    host = p;
                    break;
                }
            }

            string hostName =
                host == null ? "Unknown" :
                host == PlayerControl.LocalPlayer ? "You" :
                host.Data.PlayerName;

            string text = $"Host: {hostName}";

            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
            normal = { textColor = Color.white },
                wordWrap = true,
                alignment = TextAnchor.MiddleCenter
            };

            // dynamic size calculation
            Vector2 textSize = style.CalcSize(new GUIContent(text));
    
            float paddingX = 20f;
            float paddingY = 10f;
    
            float width = Mathf.Clamp(textSize.x + paddingX, 120f, 300f);
            float height = style.CalcHeight(new GUIContent(text), width) + paddingY;
    
            // init position only once
            if (_hostRect == default)
                _hostRect = new Rect(10, 10, width, height);
    
            // update size every frame (important for long names)
            _hostRect.width = width;
            _hostRect.height = height;
    
            _hostRect = GUI.Window(
                1337,
                _hostRect,
                (GUI.WindowFunction)DrawWindow,
                ""
            );

            void DrawWindow(int id)
            {
                GUI.color = new Color(0f, 0f, 0f, 0.6f);
                GUI.Box(new Rect(0, 0, _hostRect.width, _hostRect.height), "");

                GUI.color = Color.white;

                GUI.Label(
                    new Rect(5, 5, _hostRect.width - 10, _hostRect.height - 10),
                    text,
                    style
                );
    
                GUI.DragWindow(new Rect(0, 0, _hostRect.width, _hostRect.height));
            }
        }
        catch { }
    }
}