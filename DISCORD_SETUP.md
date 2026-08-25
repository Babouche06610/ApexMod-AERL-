# AERL Discord setup — already configured

This build is already connected to the official AERL Discord application and server.

Application ID: 1539959425236865114
Guild / Server ID: 1539616936525176963
Invite: https://discord.gg/aerl
OAuth redirect: http://127.0.0.1:53682/callback/

OAuth scopes used by AERL:
- identify
- guilds

Users only need to click "Connect Discord".
AERL opens Discord OAuth in the browser, reads the authenticated account identity,
checks whether Guild ID 1539616936525176963 is present in the user's guild list, and only then
opens the main client.

No bot token, client secret, or webhook is embedded in AERL.
