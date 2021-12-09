using System.IO;
using Exiled.API.Features;
using ModerationSystem.Collections;
using ModerationSystem.Enums;
using Newtonsoft.Json;
using Player = ModerationSystem.Collections.Player;

namespace ModerationSystem.GlobalDatabase
{
    public static class JsonManager
    {
        internal static void PunishToCache(PunishType? type, string json, Player dPlayer, ActionType actionType)
        {
            switch (type)
            {
                case PunishType.Ban:
                    var ban = JsonConvert.DeserializeObject<Ban>(json);
                    if (actionType == ActionType.Remove)
                    {
                        ban.Clear = true;
                        ban.Update();
                    }
                    File.WriteAllText(Path.Combine(Database.CacheFolder, $"cache-ban-{dPlayer.Id}@{dPlayer.Authentication}-{dPlayer.Name}-{Server.Port}.json"), JsonConvert.SerializeObject(ban, Formatting.Indented));
                    break;
                case PunishType.Kick:
                    var kick = JsonConvert.DeserializeObject<Kick>(json);
                    if (actionType == ActionType.Remove)
                    {
                        kick.Clear = true;
                        kick.Update();
                    }
                    File.WriteAllText(Path.Combine(Database.CacheFolder, $"cache-kick-{dPlayer.Id}@{dPlayer.Authentication}-{dPlayer.Name}-{Server.Port}.json"), JsonConvert.SerializeObject(kick, Formatting.Indented));
                    break;
                case PunishType.Mute:
                    var mute = JsonConvert.DeserializeObject<Mute>(json);
                    if (actionType == ActionType.Remove)
                    {
                        mute.Clear = true;
                        mute.Update();
                    }
                    File.WriteAllText(Path.Combine(Database.CacheFolder, $"cache-mute-{dPlayer.Id}@{dPlayer.Authentication}-{dPlayer.Name}-{Server.Port}.json"), JsonConvert.SerializeObject(mute, Formatting.Indented));
                    break;
                case PunishType.Warn:
                    var warn = JsonConvert.DeserializeObject<Warn>(json);
                    if (actionType == ActionType.Remove)
                    {
                        warn.Clear = true;
                        warn.Update();
                    }
                    File.WriteAllText(Path.Combine(Database.CacheFolder, $"cache-warn-{dPlayer.Id}@{dPlayer.Authentication}-{dPlayer.Name}-{Server.Port}.json"), JsonConvert.SerializeObject(warn, Formatting.Indented));
                    break;
                case PunishType.SoftBan:
                    var softBan = JsonConvert.DeserializeObject<SoftBan>(json);
                    if (actionType == ActionType.Remove)
                    {
                        softBan.Clear = true;
                        softBan.Update();
                    }
                    File.WriteAllText(Path.Combine(Database.CacheFolder, $"cache-softban-{dPlayer.Id}@{dPlayer.Authentication}-{dPlayer.Name}-{Server.Port}.json"), JsonConvert.SerializeObject(softBan, Formatting.Indented));
                    break;
                case PunishType.SoftWarn:
                    var softWarn = JsonConvert.DeserializeObject<SoftWarn>(json);
                    if (actionType == ActionType.Remove)
                    {
                        softWarn.Clear = true;
                        softWarn.Update();
                    }
                    File.WriteAllText(Path.Combine(Database.CacheFolder, $"cache-softwarn-{dPlayer.Id}@{dPlayer.Authentication}-{dPlayer.Name}-{Server.Port}.json"), JsonConvert.SerializeObject(softWarn, Formatting.Indented));
                    break;
                case PunishType.WatchList:
                    var watchList = JsonConvert.DeserializeObject<WatchList>(json);
                    if (actionType == ActionType.Remove)
                    {
                        watchList.Clear = true;
                        watchList.Update();
                    }
                    File.WriteAllText(Path.Combine(Database.CacheFolder, $"cache-watchlist-{dPlayer.Id}@{dPlayer.Authentication}-{dPlayer.Name}-{Server.Port}.json"), JsonConvert.SerializeObject(watchList, Formatting.Indented));
                    break;
                case PunishType.All:
                    File.WriteAllText(Path.Combine(Database.CacheFolder, $"cache-all-{dPlayer.Id}@{dPlayer.Authentication}-{dPlayer.Name}-{Server.Port}.json"), json);
                    break;
            }
        }
    }
}
