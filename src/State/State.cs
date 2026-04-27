using System;
using System.Collections.Generic;
using UnityEngine;

namespace EclipseMenu
{
    public static class State
    {
        // =========================
        // UI / MENU
        // =========================
        public static bool ShowReplay = false;
        public static bool ShowConsole = false;

        // =========================
        // REPLAY STATE
        // =========================
        public static bool Replay_IsLive = true;
        public static bool Replay_IsPlaying = false;

        public static DateTime MatchStart = DateTime.Now;
        public static DateTime MatchCurrent = DateTime.Now;
        public static DateTime MatchLive = DateTime.Now;

        // =========================
        // REPLAY DATA
        // =========================
        public static Dictionary<int, WalkEvent_LineData> replayWalkPolylineByPlayer
            = new Dictionary<int, WalkEvent_LineData>();

        public static List<object> liveReplayEvents
            = new List<object>();

        public static Vector2[] lastWalkEventPosPerPlayer = new Vector2[64];

        public static DateTime[] replayDeathTimePerPlayer = new DateTime[64];

        // =========================
        // SETTINGS
        // =========================
        public static bool Replay_DrawIcons = true;
        public static bool Replay_ShowOnlyLastSeconds = false;
        public static int Replay_LastSecondsValue = 30;

        public static float dpiScale = 1f;

        // =========================
        // MAP
        // =========================
        public static int mapType = 0;

        public static Color MenuThemeColor = Color.white;
        public static Color SelectedReplayMapColor = Color.white;
    }

    // =========================
    // SUPPORT STRUCT (ВАЖНО)
    // =========================
    /*public class WalkEvent_LineData
    {
        public int playerId;
        public int colorId;

        public List<Vector2> pendingPoints = new List<Vector2>();
        public List<DateTime> pendingTimeStamps = new List<DateTime>();

        public List<Vector2> simplifiedPoints = new List<Vector2>();
        public List<DateTime> simplifiedTimeStamps = new List<DateTime>();
    }*/
}