using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Unity.Netcode;
using ZetasTweaks;
using ZetasTweaks.Patches;

namespace ZetasTweaks.Patches
{
    [HarmonyPatch(typeof(Terminal))]
    internal class ShopPricesBPatch
    {
        [HarmonyPatch("BeginUsingTerminal")]
        [HarmonyPostfix]
        static void StorePrices(ref Item[] ___buyableItemsList)
        {
            ___buyableItemsList[0].creditsWorth = ConfigSync.instance.WalkieTalkieCost;
            ZetasTweaksBase.Log("Walkie shop price was overriden to: " + ___buyableItemsList[0].creditsWorth);
            ___buyableItemsList[1].creditsWorth = ConfigSync.instance.FlashlightCost;
            ___buyableItemsList[2].creditsWorth = ConfigSync.instance.ShovelCost;
            ___buyableItemsList[3].creditsWorth = ConfigSync.instance.LockpickerCost;
            ___buyableItemsList[4].creditsWorth = ConfigSync.instance.ProFlashlightCost;
            ___buyableItemsList[5].creditsWorth = ConfigSync.instance.StunGrenadeCost;
            ___buyableItemsList[6].creditsWorth = ConfigSync.instance.BoomboxCost;
            ___buyableItemsList[7].creditsWorth = ConfigSync.instance.TZPCost;
            ___buyableItemsList[8].creditsWorth = ConfigSync.instance.ZapGunCost;
            ___buyableItemsList[9].creditsWorth = ConfigSync.instance.JetpackCost;
            ___buyableItemsList[10].creditsWorth = ConfigSync.instance.ExtensionLadderCost;
            ___buyableItemsList[11].creditsWorth = ConfigSync.instance.RadarBoosterCost;
            ___buyableItemsList[12].creditsWorth = ConfigSync.instance.SprayPaintCost;
        }
    }
}
