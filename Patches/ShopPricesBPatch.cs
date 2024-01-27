using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Unity.Netcode;
using ZetasTweaks;
using ZetasTweaks.Patches;


namespace ZetasTweaks.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundBPatch
    {
        [HarmonyPatch("firstDayAnimation")]
        [HarmonyPostfix]
        static void RetrieveUnlockables(ref UnlockablesList ___unlockablesList)
        {
            ConfigSettings.SaveUnlockables(___unlockablesList); //Mostly for debugging, saves all the unlockables to a file
        }
    }

    [HarmonyPatch(typeof(Terminal))]
    internal class ShopPricesBPatch
    {
        [HarmonyPatch("BeginUsingTerminal")]
        [HarmonyPostfix] //Overwrites all the costs once the terminal is first interacted with
        static void StorePrices(ref Item[] ___buyableItemsList, ref List<TerminalNode> ___ShipDecorSelection)
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
            StartOfRound.Instance.unlockablesList.unlockables[1].shopSelectionNode.itemCost = ConfigSync.instance.GreenSuitCost;
            StartOfRound.Instance.unlockablesList.unlockables[2].shopSelectionNode.itemCost = ConfigSync.instance.HazardSuitCost;
            StartOfRound.Instance.unlockablesList.unlockables[3].shopSelectionNode.itemCost = ConfigSync.instance.PajamaSuitCost;
            StartOfRound.Instance.unlockablesList.unlockables[4].shopSelectionNode.itemCost = ConfigSync.instance.CozyLightsCost;
            //StartOfRound.Instance.unlockablesList.unlockables[5].shopSelectionNode.itemCost = ConfigSync.instance.TeleporterCost; //Causes an error as no shopSelectionNode exists for upgrades
            StartOfRound.Instance.unlockablesList.unlockables[6].shopSelectionNode.itemCost = ConfigSync.instance.TelevisionCost;
            StartOfRound.Instance.unlockablesList.unlockables[9].shopSelectionNode.itemCost = ConfigSync.instance.ToiletCost;
            StartOfRound.Instance.unlockablesList.unlockables[10].shopSelectionNode.itemCost = ConfigSync.instance.ShowerCost;
            StartOfRound.Instance.unlockablesList.unlockables[12].shopSelectionNode.itemCost = ConfigSync.instance.RecordPlayerCost;
            StartOfRound.Instance.unlockablesList.unlockables[13].shopSelectionNode.itemCost = ConfigSync.instance.TableCost;
            StartOfRound.Instance.unlockablesList.unlockables[14].shopSelectionNode.itemCost = ConfigSync.instance.RomanticTableCost;
            StartOfRound.Instance.unlockablesList.unlockables[17].shopSelectionNode.itemCost = ConfigSync.instance.SignalTranslatorCost; //This method does work, but it doesnt show correctly on screen???
            //StartOfRound.Instance.unlockablesList.unlockables[18].shopSelectionNode.itemCost = ConfigSync.instance.LoudHornCost;
            //StartOfRound.Instance.unlockablesList.unlockables[19].shopSelectionNode.itemCost = ConfigSync.instance.InverseTeleporterCost;
            StartOfRound.Instance.unlockablesList.unlockables[20].shopSelectionNode.itemCost = ConfigSync.instance.JackOLanternCost;
            StartOfRound.Instance.unlockablesList.unlockables[21].shopSelectionNode.itemCost = ConfigSync.instance.WelcomeMatCost;
            StartOfRound.Instance.unlockablesList.unlockables[22].shopSelectionNode.itemCost = ConfigSync.instance.GoldfishCost;
            StartOfRound.Instance.unlockablesList.unlockables[23].shopSelectionNode.itemCost = ConfigSync.instance.PlushiePajamaManCost;
        }
    }
}