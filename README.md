

### **ModerationSystem Plugin**<br />

Welcome to ModerationSystem plugin!

**What is ModerationSystem plugin?**: ModerationSystem plugin is a moderation plugin, you can find many commands to moderate your server: mute, ban, kick, warn, playerinfo, unmute and unwarn.

- **Warn Command:** You can warn a player, and everything will be stored in the database
- **Mute Command:** You can mute a player and decide the mute time, and everything will be stored in the database
- **Kick Command:** You can kick a player, and everything will be stored in the database
- **Ban Command:** You can ban a player and decide the time, and everything is stored in the database
- **PlayerInfo Command:** You can see the penalties of the selected player
- **Clear command:** Able to clear player's punishement!
- **SoftBan:** Player can't spawn!
- **SoftWarn**
- **Watchlist:** Watchlist for all players
- **Global Database**

### Variables:
- **{reason}**: reason
- **{player.name} and {player.userid}: player name and steam/discord id
- **{duration}**: duration


### Installation:

Download latest version of plugin and put ModerationSystem.dll into Plugin Exiled folder, place LiteDB.dll and Newtonsoft.Json.dll into dependeces folder and restart the server!

### Minium Required: 
- Exiled ![4.1.2](https://github.com/Exiled-Team/EXILED/releases/tag/4.1.2)
- LiteDB ![5.0.11.0](https://github.com/mbdavid/LiteDB/releases/tag/v5.0.11)
- Newtonsoft.Json ![13.0.1](https://github.com/JamesNK/Newtonsoft.Json/releases/tag/13.0.1)


**Commands**

| Commands  | Args | Permission | Description | 
| ------------- | ------------- | ------------- | ------------- |
| ModerationSystem / ms  | none  | none | show sub-command |
| ms warn / w  | player name or id - reason  | ms.warn | Warn a player |
| ms mute / m  | Player name or ID - time ( in minutes) - reason | ms.mute | Mute a player |
| ms kick / k | player name or ID - reason | ms.kick | Kick a player |
| ms ban / b | player name or ID - time (in minutes) - reason | ms.ban | Ban a player |
| ms playerInfo / pi | Player name or ID | ms.playerinfo | Show infractions of player |
| ms clear / cl | Player name or ID | ms.clear | clear player punishement |
| ms softwarn / sw | Player name or ID | ms.softwarn | Softwarn a player |
| ms softban / sb | Player name or ID | ms.softban | Softban a player |
| ms watchlist / wl | Player name or ID | ms.watchlist | add a player into watchlist |

If you found bug please contact me on discord: **Monald#9248** .<br /><br />

If you like the plugin, you can make a donation on paypal: ![PayPal](https://www.paypal.com/paypalme/monaldcry7788)

### Downloads
![img](https://img.shields.io/github/downloads/Monaldcry7788/ModerationSystem/total?style=for-the-badge)
