﻿using System;
 using System.Linq;

 namespace EquipmentFormatter
{
  public static class EquipmentMapper
  {
    public static string ComputeLabel(Equipment equipment, Variation variation)
    {
      var label = equipment.Label.Trim();
      return new Func<string>[]
             {
               () => TryComputeLabelTotallyNew(variation),
               () => TryComputeLabelWithReplacementBySchema(variation, label),
               () => TryComputeLabelWithReplacementByLocation(variation, label),
               () => TryComputeLabelForBattery(variation, label),
               () => TryComputeLabelWithComplementaryInfo(variation, label),
             }
             .Select(tryCompute => tryCompute())
             .DefaultIfEmpty(label)
             .First(x => x != null);
    }

    private static string TryComputeLabelTotallyNew(Variation variation) =>
      variation.Schema switch
      {
        7407 => "Nombre de cylindres",
        15304 => "Puissance (ch)",
        15305 => "Régime de puissance maxi (tr/mn)",
        _ => null,
      };

    private static string TryComputeLabelWithReplacementBySchema(Variation variation, string label)
    {
      switch (variation.Schema)
      {
        case 23502:
        case 24002:
          return label.Replace("an(s) / km", ": durée (ans)");
        case 23503:
        case 24003:
          return label.Replace("an(s) / km", ": kilométrage");
        case 7403:
          return label.Replace("litres / cm3", "litres");
        case 7402:
          return label.Replace("litres / cm3", "cm3");
        default:
          return null;
      }
    }

    private static string TryComputeLabelWithReplacementByLocation(Variation variation, string label)
    {
      switch (variation.Schema, variation.Location)
      {
        case (23301, 'F'):
          return label.Replace("AV / AR", "AV");
        case (23301, 'R'):
          return label.Replace("AV / AR", "AR");

        case (17811, 'D'):
        case (17818, 'D'):
          return label.Replace("conducteur / passager", "conducteur");

        case (17811, 'P'):
        case (17818, 'P'):
          return label.Replace("conducteur / passager", "passager");

        default:
          return null;
      }
    }

    private static string TryComputeLabelForBattery(Variation variation, string label)
    {
      var result = TryGetBatteryLabel(variation);
      if (result == null)
      {
        return null;
      }

      var rapide = variation.Location == 'F' ? " rapide" : "";
      return label.Replace("recharge (rapide) A / V / h", $"recharge{rapide} : {result}");
    }

    private static string TryGetBatteryLabel(Variation variation) =>
      variation.Schema switch
      {
        53405 => "ampérage (A)",
        53404 => "voltage (V)",
        53403 => "durée (heures)",
        _ => null,
      };

    private static string TryComputeLabelWithComplementaryInfo(Variation variation, string label) =>
      variation.Schema switch
      {
        14103 => (label + " : largeur"),
        14104 => (label + " : profil"),
        _ => null
      };
  }
}