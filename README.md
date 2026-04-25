<p align="center">
  # EclipseMenu
</p>
<p align="center">
  <b>An easy-to-use Among Us cheat menu with a simple GUI and lots of useful modules.</b>
</p>

<!-- omit in toc -->
# 😎 Table Of Contents

- [🎁 Releases](#-releases)
- [⬇️ Installation](#️-installation)
  - [🪟 Windows](#-windows)
  - [🐧 Linux](#-linux)
- [📋 Features](#-features)
- [❓ FAQ](#-faq)
- [⚠️ Disclaimer](#️-disclaimer)

# 🎁 Releases

| Mod Version| Among Us - Version | Link |
|----------|-------------|-----------------|
| v1.1.9 | 17.3 ( 2026.4.1 )<br>17.2.2 ( 2026.3.17 )<br>17.2.1 ( 2026.2.24 ) | [Download](https://github.com/k3razz/EclipseMenu/releases/tag/v1.1.9) |

# ⬇️ Installation

## 🪟 Windows

1. Download the latest **EclipseMenu zip pack** from [here](https://github.com/k3razz/EclipseMenu/releases/latest).
    - **For Steam or Itch.io:** Download `EclipseMenu-VERSION=steam=itchio.zip`.

2. Open the zip file you have just downloaded and copy all its contents.

3. Paste these files directly into your Among Us game folder:
    - **Steam:** Right-click Among Us in your Library → Click `Manage` → Click `Browse local files`.
    - **Epic Launcher:** Right-click Among Us in your Library → Click `Manage` → Click the folder icon in the `Installation` box.
    - **Itch.io:** Open the Itch.io app → Right-click Among Us in your Library → Click `Manage` → Click `Open folder in Explorer`.
   - **Microsoft Store:** Open the folder where Windows apps are installed (typically `C:\Program Files\WindowsApps\`) by following the tutorial [here](https://youtu.be/qCeoEIy_vrw) → In File Explorer, use the search bar to search for `Among Us.exe` → Right-click the `Among Us.exe` result → Select `Open file location`.
    - **Xbox App:** Right-click Among Us in your Library → Click `Manage` → Open the `FILES` tab → Click `BROWSE...` → Open the `Among Us` folder → Open the `Content` folder.

4. Launch Among Us as you normally would. You should see a console window appear, installing the mod's requirements.

5. Wait for the console window to finish the installation.

6. After installation, Among Us will automatically open with EclipseMenu successfully installed.
    - By default, you can toggle the cheat GUI on by pressing **DELETE** on your keyboard.

7. If the installation doesn't work, check out our [FAQ](#-faq).

## 🐧 Linux

1. Run Among Us under **Proton (or Wine)**.
   - **In Steam:** Right-click Among Us in your Library → Click `Properties` → Click `Compatibility` → Enable `Force the use of a specific Steam Play compatibility tool`.

   - Test different Proton versions if you're having issues launching the game.

2. Set up **BepInEx** (the framework EclipseMenu is built upon).
   - Follow the official Proton / Wine setup guide found [here](https://docs.bepinex.dev/articles/advanced/proton_wine.html).
   - If you are using Proton with Steam, specify the DLL override:
     - **In Steam:** Right-click Among Us in your Library → Click `Properties` → Click `General` → Click `Launch Options`.
     - Add this to your launch options:

       ```
       WINEDLLOVERRIDES="winhttp.dll=n,b" %command%
       ```

   - After that, continue with the Windows installation steps found [here](#-windows).

3. Fix crashes or errors (like `Unable to execute IL2CPP chainloader`).
   - **In Steam:** Right-click Among Us in your Library → Click `Properties` → Click `General` → Click `Launch Options`.
   - Set your launch options to:

     ```
     PROTON_NO_ESYNC=1 PROTON_USE_WINED3D=1 WINEDLLOVERRIDES="winhttp.dll=n,b" %command%
     ```

# 📋 Features

- An intuitive GUI with our latest, greatest Among Us cheats
- See ghosts & reveal the impostors
- Track every player's position using the minimap
- Teleport anywhere you want
- Change your role whenever you please
- Remove kill cooldown & spam-kill everyone
- Murder any distant player from across the map
- Unlock all of the game's cosmetics for FREE
- No more annoying disconnect penalties

# ❓ FAQ

Click to expand each topic

<details>

<summary><h2>❗ I'm having issues installing EclipseMenu</h2></summary>

First of all, make sure you are running the most recent version of Among Us (`17.3` / `2026.4.1`, `17.2.2` / `2026.3.17` OR `17.2.1` / `2026.2.24`) with the most recent version of EclipseMenu (`v1.1.9`).

Also, check if your platform is officially supported:

- ✅ Steam
- ✅ Itch.io
- ✅ Epic Games Launcher
- ✅ Microsoft Store
- ✅ Xbox App
- ✅ Cracked
- ❌ iOS App Store & Google Play
- ❌ PS & Switch & Xbox Console

Make sure you followed the installation guide precisely. This is what your `Among Us` folder should look like after a successful installation:


<br>Some antiviruses might cause issues when installing the mod, so consider temporarily deactivating your antivirus if the game isn't booting after installation.

When installing EclipseMenu for the first time, it will take **MUCH** longer than usual for the game to load. This is completely normal and expected behavior, so don't be alarmed if you have to wait a while. You can keep track of the installation progress through this useful BepInEx console window that pops up when you start the game:

<br>If you are still having issues, feel free to open a new Github issue [here](https://github.com/k3razz/EclipseMenu/issues/new), or you can ask for help in our Discord server: [will be soon].

</details>

<details>

<summary><h2>👾 I found a bug OR I would like to suggest a new feature</h2></summary>

To let me know, you can open a new Github issue [here](https://github.com/k3razz/EclipseMenu/issues/new), or you can discuss it on our Discord server: [will be soon].

If you want, you can also contribute to the project and implement the change yourself by making a pull request. All contributions are welcome!

</details>

<details>

<summary><h2>👨‍💻 I want to contribute to this project</h2></summary>

To get started, I suggest you first learn about the basics of C# and Unity, since that's what Among Us is written in. There are plenty of tutorials out there to help you with that.

You should also learn about Github forking and pull requests, since you will need to use those to make any contributions to the project. [Here](https://docs.github.com/en/get-started/exploring-projects-on-github/contributing-to-a-project) is the official documentation on the topic.

Then, I suggest you learn about Among Us modding in general. In this project, I use BepInEx and Harmony to patch the game, so I suggest you take a look at [this](https://docs.reactor.gg) great guide to learn how to work with those.

Here are some other useful resources:

- The [Reactor](https://reactor.gg/discord) discord server
    - A great community of Among Us modders where you can ask questions and get help
    - Here you can also find the most recent decompiled Among Us assemblies (the DLL files in `#resources` channel). I suggest using [dnSpy](https://github.com/dnSpy/dnSpy/releases/latest) to go through these.

- [sus.wiki](https://github.com/roobscoob/among-us-protocol)
    - Useful resource to learn more about the Among Us network protocol
    - Keep in mind that it is slightly outdated

</details>

# ⚠️ Disclaimer

This mod is not affiliated with Among Us or Innersloth LLC, and the content contained therein is not endorsed or otherwise sponsored by Innersloth LLC. Portions of the materials contained herein are property of Innersloth LLC.

Usage of this mod can violate the terms of service of Among Us, which may lead to punitive action including temporary or permanent bans from the game. The creator is not responsible for any consequences you may face due to usage. Use at your own risk.
