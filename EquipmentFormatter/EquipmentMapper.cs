﻿namespace EquipmentFormatter
{
  public static class EquipmentMapper
  {
    public static string ComputeLabel(Equipment equipment, Variation variation)
    {
      var label = equipment.Label.Trim();

      var result = TryComputeLabelTotallyNew(variation);
      if (result.Success) return result.Value;

      result = TryComputeLabelWithReplacementBySchema(variation, label);
      if (result.Success) return result.Value;

      result = TryComputeLabelWithReplacementByLocation(variation, label);
      if (result.Success) return result.Value;

      result = TryComputeLabelWithDoubleReplacement(variation, label);
      if (result.Success) return result.Value;

      result = TryComputeLabelWithComplementaryInfo(variation, label);
      if (result.Success) return result.Value;

      return label;
    }

    private static (bool Success, string Value) TryComputeLabelTotallyNew(Variation variation)
    {
      if (variation.Schema == 7407)
      {
        return (true, "Nombre de cylindres");
      }

      if (variation.Schema == 15304)
      {
        return (true, "Puissance (ch)");
      }

      if (variation.Schema == 15305)
      {
        return (true, "Régime de puissance maxi (tr/mn)");
      }

      return (false, null);
    }

    private static (bool Success, string Result) TryComputeLabelWithReplacementBySchema(Variation variation, string label)
    {
      if (variation.Schema == 23502 || variation.Schema == 24002)
      {
        return (true, label.Replace("an(s) / km", ": durée (ans)"));
      }

      if (variation.Schema == 23503 || variation.Schema == 24003)
      {
        return (true, label.Replace("an(s) / km", ": kilométrage"));
      }

      if (variation.Schema == 7403)
      {
        return (true, label.Replace("litres / cm3", "litres"));
      }

      if (variation.Schema == 7402)
      {
        return (true, label.Replace("litres / cm3", "cm3"));
      }

      return (false, null);
    }

    private static (bool Success, string Result) TryComputeLabelWithReplacementByLocation(Variation variation, string label)
    {
      if (variation.Schema == 23301)
      {
        if (variation.Location == 'F')
        {
          return (true, label.Replace("AV / AR", "AV"));
        }

        if (variation.Location == 'R')
        {
          return (true, label.Replace("AV / AR", "AR"));
        }
      }

      if (variation.Schema == 17811 || variation.Schema == 17818)
      {
        if (variation.Location == 'D')
        {
          return (true, label.Replace("conducteur / passager", "conducteur"));
        }

        if (variation.Location == 'P')
        {
          return (true, label.Replace("conducteur / passager", "passager"));
        }
      }

      return (false, null);
    }

    private static (bool Success, string Result) TryComputeLabelWithDoubleReplacement(Variation variation, string label)
    {
      if (variation.Schema == 53405)
      {
        if (variation.Location == 'D')
        {
          return (true, label.Replace("recharge (rapide) A / V / h", "recharge : ampérage (A)"));
        }

        if (variation.Location == 'F')
        {
          return (true, label.Replace("recharge (rapide) A / V / h", "recharge rapide : ampérage (A)"));
        }
      }

      if (variation.Schema == 53404)
      {
        if (variation.Location == 'D')
        {
          return (true, label.Replace("recharge (rapide) A / V / h", "recharge : voltage (V)"));
        }

        if (variation.Location == 'F')
        {
          return (true, label.Replace("recharge (rapide) A / V / h", "recharge rapide : voltage (V)"));
        }
      }

      if (variation.Schema == 53403)
      {
        if (variation.Location == 'D')
        {
          return (true, label.Replace("recharge (rapide) A / V / h", "recharge : durée (heures)"));
        }

        if (variation.Location == 'F')
        {
          return (true, label.Replace("recharge (rapide) A / V / h", "recharge rapide : durée (heures)"));
        }
      }

      return (false, null);
    }

    private static (bool Success, string Result) TryComputeLabelWithComplementaryInfo(Variation variation, string label)
    {
      if (variation.Schema == 14103)
      {
        return (true, label + " : largeur");
      }

      if (variation.Schema == 14104)
      {
        return (true, label + " : profil");
      }

      return (false, null);
    }
  }
}