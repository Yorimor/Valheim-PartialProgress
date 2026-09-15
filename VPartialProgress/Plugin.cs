using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using System.Reflection;

namespace VPartialProgress
{
    [BepInPlugin(pluginGUID, pluginName, pluginVersion)]
    public class Main : BaseUnityPlugin
    {
        const string pluginGUID = "Yorimor-PartialProgress";
        const string pluginName = "PartialProgress";
        const string pluginVersion = "1.2.0";

        private readonly Harmony HarmonyInstance = new Harmony(pluginGUID);

        public static ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource(pluginName);
        
        public static ConfigEntry<bool> showNoProgress;

        public void Awake()
        {
            showNoProgress = Config.Bind("General", "showNoProgress", false, "Show achievement objectives that have 0 progress towards them"); // Description of the option to show in the config file
            
            Assembly assembly = Assembly.GetExecutingAssembly();
            HarmonyInstance.PatchAll(assembly);
            
            logger.LogInfo("Plugin loaded!");
        }
    }
}