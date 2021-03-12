

### **ModerationSystem Plugin**<br />

Welcome to ModerationSystem plugin!

**What is MS plugin?**: MS plugin is a moderation plugin, you can find many commands to moderate your server: mute, ban, kick, warn and playerinfo.

- **Warn Command:** You can warn a player, and everything will be stored in the database
- **Mute Command:** You can mute a player and decide the mute time, and everything will be stored in the database
- **Kick Command:** You can kick a player, and everything will be stored in the database
- **Ban Command:** You can ban a player and decide the time, and everything is stored in the database
- **PlayerInfo Command:** You can see the penalties of the selected player
- **Unmute Command:** You can remove mute of player
- **Unwarn Command:** You can remove mute of player
- **Warn Message:** You can set private broadcast to warned player. (you can use {reason} variable for reason)
- **Mute Message:** You can set private broadcast to muted player. (you can use {reason} and {duration} variables for reason and duration)
- **Kick Message:** You can set kick reason. (you can use {reason} variable for reason)
- **Ban Message**: You can set ban reason. (you can use {reason} variable for reason)

### Required: 
- Exiled 2.8.0
- LiteDB 5.0.9.0

### Important:
each server must have it's own database, you cannot have multiple server on same database.
If you run multiple server just change the database name in configs so each server will have it's own db.
If in your first server in config the database name is Warns
In the second server config you must set database name with another name
Example :
First server config: databaseName: Warns
Second server config: databaseName: Warns1

### Config

You can see settings and edit them inside your Exiled config.

**Config**

| Name  | Type | Description | 
| ------------- | ------------- | ------------- |
| IsEnabled  | bool  | Enable or Disable the plugin |
| EnableCreatorCommand | bool | Enable or Disable the creator command |
| DatabaseName | string | The name of the database |
| WarnMessage | Broadcast  | The private Broadcast to warned player |
| KickMessage | string  | The message when player will be kicked |
| BanMessage | string  | The message when player will be banned  |
| MuteMessage | Broadcast  | The private broadcast to muted player  |

**Commands**

| Commands  | Args | Permission | Description | 
| ------------- | ------------- | ------------- | ------------- |
| ModerationSystem / ms  | none  | none | show sub-command |
| ms warn / w  | player name or id - reason  | ms.warn | Warn a player |
| ms mute / m  | Player name or ID - time (minute) - reason | ms.mute | Mute a player |
| ms kick / k | player name or ID - reason | ms.kick | Kick a player |
| ms ban / b | player name or ID - time (minute) - reason | ms.ban | Ban a player |
| ms playerInfo / pi | Player name or id | ms.playerinfo | Show infractions of player |


If you found bug please contact me on discord: **Twitch.tv/Monaldcry7788#9248** .<br /><br />

For the database system, I took a cue from this plugin: ![TextChat](https://github.com/iopietro/TextChat)

If you like the plugin, you can make a donation on paypal: https://paypal.me/monaldcry7788

### Downloads
![img](https://img.shields.io/github/downloads/Monaldcry7788/ModerationSystem/total?style=for-the-badge)
