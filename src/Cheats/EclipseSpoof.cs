using AmongUs.Data;

namespace EclipseMenu;
public static class EclipseSpoof
{
    public static void SpoofLevel()
    {
        // Parse Spoofing.Level config entry and turn it into a uint
        if (!string.IsNullOrEmpty(EclipseMenu.spoofLevel.Value) &&
            uint.TryParse(EclipseMenu.spoofLevel.Value, out uint parsedLevel) &&
            parsedLevel != DataManager.Player.Stats.Level)
        {

            // Store the spoofed level using DataManager
            DataManager.Player.stats.level = parsedLevel - 1;
            DataManager.Player.Save();
        }
    }

    public static string SpoofFriendCode()
    {
        string friendCode = EclipseMenu.guestFriendCode.Value;
        if (string.IsNullOrWhiteSpace(friendCode))
        {
            friendCode = DestroyableSingleton<AccountManager>.Instance.GetRandomName();
        }
        return friendCode;
    }
}
