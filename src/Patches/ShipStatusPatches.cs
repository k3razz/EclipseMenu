using HarmonyLib;

namespace EclipseMenu;

[HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.FixedUpdate))]
public static class ShipStatus_FixedUpdate
{
    public static void Postfix(ShipStatus __instance)
    {
        EclipseSabotageCheats.Process(__instance);
        EclipseCheats.OpenSabotageMapCheat();

        EclipseCheats.CloseMeetingCheat();
        EclipseCheats.SkipMeetingCheat();
        EclipseCheats.CallMeetingCheat();
        EclipseCheats.WalkInVentCheat();
        EclipseCheats.KickVentsCheat();

        EclipsePPMCheats.ReportBodyPPM();
    }
}

[HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.FixedUpdate))]
public static class FungleShipStatus_FixedUpdate
{
    public static void Postfix(FungleShipStatus __instance)
    {
        EclipseSabotageCheats.ProcessFungle(__instance);
    }
}
