using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace DBDECoordinateLoadBridge
{
    [BepInPlugin(GUID, Name, Version)]
    [BepInDependency(DbdeBridge.DbdeGuid, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency(DbdeBridge.CoordinateLoadOptionGuid, BepInDependency.DependencyFlags.SoftDependency)]
    public sealed class DBDECoordinateLoadBridgePlugin : BaseUnityPlugin
    {
        public const string GUID = "tomtom.kk.dbdecoordinateloadbridge";
        public const string Name = "KK_DBDECoordinateLoadBridge";
        public const string Version = "0.2.0.0";

        internal static ManualLogSource Log;

        private Harmony _harmony;

        private void Awake()
        {
            Log = Logger;
            _harmony = new Harmony(GUID);
            DbdeBridge.TryPatch(_harmony);
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
    }
}
