> [!IMPORTANT]
> **Legacy standalone repository / 旧版独立仓库**
>
> Active development for the KK and KKS versions has moved to
> [TomPlugin](https://github.com/tomTom1010-IEE/TomPlugin). This repository
> remains available for historical source and releases only.
>
> KK 与 KKS 版本的后续开发已迁移至
> [TomPlugin](https://github.com/tomTom1010-IEE/TomPlugin)。
> 本仓库仅保留旧版源码和历史发布。

# KKS_DBDECoordinateLoadBridge

KKS_DBDECoordinateLoadBridge fixes DynamicBoneDistributionEditor data transfer when a Koikatsu Sunshine coordinate card is selectively loaded through Coordinate Load Option.

Without the bridge, DBDE data may load when Coordinate Load Option's selection panel is disabled but disappear when `Show Selection` is enabled. The bridge preserves DBDE's temporary transfer data until the real Maker character consumes it.

## Requirements

- Koikatsu Sunshine
- BepInEx
- DynamicBoneDistributionEditor (DBDE) 1.5.1 or 2.0.0
- Coordinate Load Option

DBDE and Coordinate Load Option are declared as soft dependencies so this plugin never prevents the game from starting. Both plugins must be installed for the bridge to do useful work. KKSAPI is not required by this bridge.

## Installation

Place `KKS_DBDECoordinateLoadBridge.dll` in:

```text
BepInEx/plugins/
```

There is no configuration file and no in-game UI. When both supported plugins are detected, the compatibility patches are enabled automatically.

## How to Use

1. Edit the accessory or clothing Dynamic Bone parameters with DBDE.
2. Save a coordinate card normally.
3. Load the destination character in Maker.
4. Open the coordinate card loader and enable Coordinate Load Option's `Show Selection` panel.
5. Enable the accessory selection and the relevant extended-data selection.
6. Keep accessory loading in `Replace Mode`.
7. Load the coordinate card.

DBDE should deserialize and apply the coordinate data to the real character just as it does during an ordinary coordinate load.

## What the Bridge Does

Coordinate Load Option performs selective loading through a temporary character. DBDE also uses static transfer fields and a short cleanup coroutine. In the affected sequence, DBDE can clear its transfer data before the real character receives it.

The bridge:

- detects the Coordinate Load Option Replace Mode transfer;
- captures DBDE's existing coordinate `PluginData` from the temporary character;
- prevents DBDE's generated cleanup coroutine from clearing it too early;
- restores the transfer data before the real character's DBDE load callback;
- temporarily preserves the native Maker accessory-load flag required by DBDE;
- lets the installed DBDE version deserialize and apply its own data normally;
- clears the bridge-owned references after the transfer completes or times out.

The plugin does not parse or rewrite DBDE serialization and does not add its own card-data key.

## Compatibility

- DBDE 1.5.1 is supported through its lowercase cleanup method/state-machine name.
- DBDE 2.0.0 is supported through its renamed uppercase method/state-machine name.
- Coordinate Load Option accessory `Replace Mode` is supported.
- Accessory `Add Mode` is not supported because DBDE does not expose a stable destination-slot mapping for that operation.
- This bridge transfers coordinate-card data only. It does not convert character-wide DBDE data into coordinate data.

## Troubleshooting

Check `output_log.txt` or the BepInEx log. A working transfer normally includes messages similar to:

```text
Enabled DBDE ... Coordinate Load Option compatibility.
Started a DBDE Coordinate Load Option transfer session.
Captured DBDE coordinate data from the Coordinate Load Option temporary character.
Restored DBDE transfer data before the real character load.
Transferred DBDE coordinate data from the temporary character to the real character.
```

If the bridge reports that DBDE and Coordinate Load Option were not both found, verify their DLLs are loaded and check their plugin GUIDs in the earlier BepInEx log entries.

## Building

The KKS project targets .NET Framework 4.6.2 and intentionally uses reflection instead of compile-time DBDE or Coordinate Load Option references.

```powershell
dotnet build KKS_DBDECoordinateLoadBridge.csproj -c Release
```

Local game assemblies are expected under `dlls/` and are not committed.

## License

Licensed under the GNU GPL-3.0 license.
