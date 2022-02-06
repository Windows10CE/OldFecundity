using System.Collections;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;

namespace OldFecundity;

[BepInPlugin(ModGUID, ModName, ModVer)]
[HarmonyPatch]
public class OldFecundityPlugin : BaseUnityPlugin
{
    public const string ModGUID = "com.Windows10CE.OldFecundity";
    public const string ModName = "OldFecundity";
    public const string ModVer = "1.0.0";

    new private static ManualLogSource Logger;

    private static Harmony HarmonyInstance;
    
    private void OnEnable()
    {
        Logger = base.Logger;
        HarmonyInstance = Harmony.CreateAndPatchAll(typeof(OldFecundityPlugin).Assembly, ModGUID);
        
        Logger.LogMessage("Restored old behavior of fecundity");
    }

    private void OnDisable()
    {
        HarmonyInstance.UnpatchSelf();
        
        Logger.LogMessage("Reverted to new fecundity behavior");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(DrawCopy), nameof(DrawCopy.OnResolveOnBoard))]
    private static IEnumerator GetRidOfTutorialPrompt(IEnumerator original, DrawCopy __instance)
    {
        yield return __instance.PreSuccessfulTriggerSequence();
        yield return __instance.CreateDrawnCard();
        yield return __instance.LearnAbility();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(DrawCopy), nameof(DrawCopy.CardToDrawTempMods), MethodType.Getter)]
    private static bool NoCardMods(out List<CardModificationInfo> __result)
    {
        __result = null;
        return false;
    }
}
