using System;
using HarmonyLib;
using UnityEngine;

namespace EclipseMenu;

[HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.LateUpdate))]
public static class PlayerPhysics_LateUpdate
{
    public static void Postfix(PlayerPhysics __instance)
    {
        EclipseESP.PlayerNametags(__instance);
        EclipseESP.SeeGhostsCheat(__instance);

        EclipseCheats.NoClipCheat();
        EclipseCheats.ProtectCheat();
        EclipseCheats.KillAllCheat();
        EclipseCheats.KillAllCrewCheat();
        EclipseCheats.KillAllImpsCheat();
        EclipseCheats.ForceStartGameCheat();
        EclipseCheats.TeleportCursorCheat();
        EclipseCheats.CompleteMyTasksCheat();
        EclipseCheats.PlayAnimationCheat();
        EclipseCheats.PlayScannerCheat();

        EclipsePPMCheats.EjectPlayerPPM();
        EclipsePPMCheats.SpectatePPM();
        EclipsePPMCheats.KillPlayerPPM();
        EclipsePPMCheats.TelekillPlayerPPM();
        EclipsePPMCheats.TeleportPlayerPPM();
        EclipsePPMCheats.SetFakeRolePPM();
        EclipsePPMCheats.SetFakeAlivePPM();
        // EclipsePPMCheats.ForceRolePPM();

        // This check ensures there is only one run per frame
        // so that OverloadHandler._timer progression remains accurate
        if (__instance.AmOwner)
        {
            OverloadHandler.Run();
        }

        TracersHandler.DrawPlayerTracer(__instance);

        GameObject[] bodyObjects = GameObject.FindGameObjectsWithTag("DeadBody");
        foreach(GameObject bodyObject in bodyObjects) // Finds and loops through all dead bodies
        {
            DeadBody deadBody = bodyObject.GetComponent<DeadBody>();

            if (!deadBody || deadBody.Reported) continue;  // Only draw tracers for unreported dead bodies
            TracersHandler.DrawBodyTracer(deadBody);
        }

        try
        {
            if (CheatToggles.invertControls)
            {
                PlayerControl.LocalPlayer.MyPhysics.Speed = -Mathf.Abs(PlayerControl.LocalPlayer.MyPhysics.Speed);
                PlayerControl.LocalPlayer.MyPhysics.GhostSpeed = -Mathf.Abs(PlayerControl.LocalPlayer.MyPhysics.GhostSpeed);
            }
            else
            {
                PlayerControl.LocalPlayer.MyPhysics.Speed = Mathf.Abs(PlayerControl.LocalPlayer.MyPhysics.Speed);
                PlayerControl.LocalPlayer.MyPhysics.GhostSpeed = Mathf.Abs(PlayerControl.LocalPlayer.MyPhysics.GhostSpeed);
            }
        } catch (NullReferenceException) { }
    }
}

[HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.HandleAnimation))]
public static class PlayerPhysics_HandleAnimation
{
    // Prefix patch of PlayerPhysics.HandleAnimation to disable walking animation
    public static bool Prefix(PlayerPhysics __instance)
    {
        if (CheatToggles.moonWalk && __instance.AmOwner)
        {
            __instance.ResetAnimState();

            return false;
        }

        return true;
    }
}
