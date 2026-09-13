using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace VPartialProgress
{
    [BepInPlugin(pluginGUID, pluginName, pluginVersion)]
    public class Main : BaseUnityPlugin
    {
        const string pluginGUID = "Yorimor-PartialProgress";
        const string pluginName = "PartialProgress";
        const string pluginVersion = "1.0.1";

        private readonly Harmony HarmonyInstance = new Harmony(pluginGUID);

        public static ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource(pluginName);

        public void Awake()
        {
            logger.LogInfo("Plugin loaded!");

            Assembly assembly = Assembly.GetExecutingAssembly();
            HarmonyInstance.PatchAll(assembly);
        }
    }
}