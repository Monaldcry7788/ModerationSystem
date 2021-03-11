

### **ModerationSystem Plugin**<br />

Welcome to ModerationSystem plugin!

**What's is MS plugin?**: MS plugin is a moderation plugin, you can find many commands to moderate your server: mute, ban, kick, warn and playerinfo.

- **Warn Command:** You can warn a player, and everything will be stored in the database
- **Mute Command:** You can mute a player and decide the mute time, and everything will be stored in the database
- **Kick Command:** You can kick a player, and everything will be stored in the database
- **Ban Command:** You can ban a player and decide the time, and everything is stored in the database
- **PlayerInfo Command:** You can see the penalties of the selected player
- **Warn Message:** You can set private broadcast to warned player. (you can use {reason} variable for reason)
- **Mute Message:** You can set private broadcast to muted player. (you can use {reason} and {duration} variables for reason and duration)
- **Kick Message:** You can set kick reason. (you can use {reason} variable for reason)
- **Ban Message**: You can set ban reason. (you can use {reason} variable for reason)

### Required: 
- Exiled 2.8.0
- LiteDB 5.0.9.0

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
| Warn / w  | player name or id - reason  | ms.warn | Warn a player |
| Mute / m  | Player name or ID - time (minute) - reason | ms.mute | Mute a player |
| Kick / k | player name or ID - reason | ms.kick | Kick a player |
| Ban / b | player name or ID - time (minute) - reason | ms.ban | Ban a player |
| PlayerInfo / pi | Player name or id | ms.playerinfo | Show infractions of player |


If you found bug please contact me on discord: **Twitch.tv/Monaldcry7788#9248** .<br /><br />

### Downloads
![img](https://img.shields.io/github/downloads/Monaldcry7788/ModerationSystem/total?style=for-the-badge)