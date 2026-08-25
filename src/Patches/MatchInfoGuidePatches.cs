using HarmonyLib;
using UnityEngine;

namespace EclipseMenu;

[HarmonyPatch(typeof(MatchInfoGuide), nameof(MatchInfoGuide.Open))]
public static class MatchInfoGuide_Open
{
    public static void Prefix(MatchInfoGuide __instance)
    {
        if (__instance.NormalModeSettings.Count > 0 || __instance.HnSModeSettings.Count > 0)
        {
            __instance.ControllerSelectable.Clear();
            __instance.CreatePlayerEntries();
        }
    }
}

[HarmonyPatch(typeof(MatchInfoGuide), nameof(MatchInfoGuide.CreatePlayerEntries))]
public static class MatchInfoGuide_CreatePlayerEntries
{
    private static Vector2 _anchoredPosition = new Vector2(0f, 0f);

    public static bool Prefix(MatchInfoGuide __instance)
    {
        __instance.PlayerPool.ReclaimAll();
		int num = 51;
		foreach (NetworkedPlayerInfo networkedPlayerInfo in GameData.Instance.AllPlayers)
		{
			PlayerIdentifierButton component = __instance.PlayerPool.Get<PoolableBehavior>().GetComponent<PlayerIdentifierButton>();
			component.transform.localPosition = new Vector3(0f, 0f, -1f);
			component.Populate(networkedPlayerInfo);

            component.NameText.text = Utils.GetNameTag(networkedPlayerInfo, networkedPlayerInfo.PlayerName, false, true);

            if (_anchoredPosition == Vector2.zero)
            {
                _anchoredPosition = component.NameText.rectTransform.anchoredPosition;
            }
            component.NameText.rectTransform.anchoredPosition = new Vector2(_anchoredPosition.x, _anchoredPosition.y + 0.05f);

            __instance.ControllerSelectable.Add(component.Button);
			component.SetTextStencil(num++);
		}

        return false;
    }
}