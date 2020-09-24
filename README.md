# KeepWifiConnected
Simple Windows utility that tracks network connection and reconnect to wifi when internet is down.
 
### Screenshots
![Screenshot](/Screenshots/KeepWifiConnected.jpg?raw=true "Screenshot")

### How it works?
It runs in the background and checks the network every few seconds.

### Why?
Sometimes, for some obscure reason, my notebook loses Internet connection. Disconnectiong and reconnecting to WiFi solves the problem.

### Features
 - Show internet/network status.
 - Track data into log file.
 - Reconnect to WiFi when Internet is unavailable.

### Source
It's written in C# (.NET) and uses "netsh" to manage WiFi.
