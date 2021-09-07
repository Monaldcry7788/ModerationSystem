

### **ModerationSystem Plugin**<br />

Welcome to ModerationSystem plugin!

**What is ModerationSystem plugin?**: ModerationSystem plugin is a moderation plugin, you can find many commands to moderate your server: mute, ban, kick, warn, playerinfo, unmute and unwarn.

- **Warn Command:** You can warn a player, and everything will be stored in the database
- **Mute Command:** You can mute a player and decide the mute time, and everything will be stored in the database
- **Kick Command:** You can kick a player, and everything will be stored in the database
- **Ban Command:** You can ban a player and decide the time, and everything is stored in the database
- **PlayerInfo Command:** You can see the penalties of the selected player
- **Unmute Command:** You can remove mute of player
- **Unwarn Command:** You can remove warn of player
- **Unban Command:** You can remove ban of player
- **Warn Message:** You can set private broadcast to warned player.
- **Mute Message:** You can set private broadcast to muted player.
- **Kick Message:** You can set kick reason.
- **Ban Message**: You can set ban reason.
- **Webhook support**: Webhook is supported
- **Warn Webhook Message**: You can set message to diplay on discord when player will be warned
- **Mute Webhook Message**: You can set message to diplay on discord when player will be muted
- **Kick Webhook Message**: You can set message to diplay on discord when player will be kicked
- **Ban Webhook Message**: You can set message to diplay on discord when player will be banned

### Variables:
The variable avariable in all sanction private broadcast is:
- **{reason}**

The variable avariable in warn saction private broadcast is:
- **{reason}**: reason of warn

The variable avariable in kick saction disconnect reason is:
- **{reason}**: reason of kick

The variable avariable in ban saction disconnect reason is:
- **{reason}**: reason of ban

The variable avariable in mute saction private broadcast is:
- **{duration}**: duration of mute (in minutes)
- **{reason}**: reason of mute




The variables avariable in all webhook messages are:
- **{staffer}**: name and SteamID of staffer
- **{target.Name}**: name of player sanctioned
- **{target.Id}**: steamid of player sanctioned
- **{reason}**: reason of sanction

The variable avariable in warn webhook message is:
- **{warnid}**: id of warn

The variable avariable in kick webhook message is:
- **{kickid}**: id of kick

The variables avariable in mute webhook message are:
- **{duration}**: mute duration (in minutes)
- **{muteid}**: id of mute

The variables avariable in ban webhook message are:
- **{duration}**: ban duration (in minutes)
- **{banid}**: id of ban



### Installation:

Download latest version of plugin and put ModerationSystem.dll into Plugin Exiled folder, place LiteDB.dll and Newtonsoft.Json.dll into dependeces folder and restart the server!

### Minium Required: 
- Exiled ![2.8.0](https://github.com/Exiled-Team/EXILED/releases/tag/2.8.0)
- LiteDB ![5.0.9.0](https://github.com/mbdavid/LiteDB/releases/tag/v5.0.9)
- Newtonsoft.Json ![12.0.3](https://github.com/JamesNK/Newtonsoft.Json/releases/tag/12.0.3)

### Important:
each server must have it's own database, you cannot have multiple server on same database.
If you run multiple server just change the database name in configs so each server will have it's own db.
If in your first server config the database name is Warns
In the second server config you must set database name with another name

**Example**:

First server config: 
  databaseName: Warns
  
Second server config:
  databaseName: Warns1

### To do:
- broadcast to staff when player will be warned/kicked/muted/banned
- Anti team kill

### Config

You can see settings and edit them inside your Exiled config.

**Config**

| Name  | Type | Description | 
| ------------- | ------------- | ------------- |
| IsEnabled  | bool  | Enable or Disable the plugin |
| DatabaseName | string | The name of the database |
| WarnMessage | Broadcast  | The private Broadcast to warned player |
| KickMessage | string  | The message when player will be kicked |
| BanMessage | string  | The message when player will be banned  |
| MuteMessage | Broadcast  | The private broadcast to muted player  |
| WebHookURL | string | Webhook URL |
| WebHookName | string | Name of Webhook |
| WarnedMessageWebHook | string | The Message to display to discord when player will be warned |
| MutedMessageWebHook | string | The Message to display to discord when player will be muted |
| BanMessageWebHook | string | The Message to display to discord when player will be banned |
| KickedMessageWebHook | string | The Message to display to discord when player will be kicked |


**Commands**

| Commands  | Args | Permission | Description | 
| ------------- | ------------- | ------------- | ------------- |
| ModerationSystem / ms  | none  | none | show sub-command |
| ms warn / w  | player name or id - reason  | ms.warn | Warn a player |
| ms mute / m  | Player name or ID - time ( in minutes) - reason | ms.mute | Mute a player |
| ms kick / k | player name or ID - reason | ms.kick | Kick a player |
| ms ban / b | player name or ID - time (in minutes) - reason | ms.ban | Ban a player |
| ms playerInfo / pi | Player name or ID | ms.playerinfo | Show infractions of player |
| ms unmute / um | player name or ID - mute ID | ms.unmute | Remove mute of player |
| ms unwarn / uw | player name or ID - warn ID | ms.unwarn | Remove warn of player |
| ms unban / ub | player name or ID - ban ID | ms.unban | Remove ban of player |

 

If you found bug please contact me on discord: **Twitch.tv/Monaldcry7788#9248** .<br /><br />

If you like the plugin, you can make a donation on paypal: ![PayPal](https://paypal.me/monaldcry7788)

### Downloads
![img](https://img.shields.io/github/downloads/Monaldcry7788/ModerationSystem/total?style=for-the-badge)
