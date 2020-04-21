﻿namespace EquipmentFormatter
{
  public static class EquipmentMapper
  {
    public static string ComputeLabel(Equipment equipment, Variation variation)
    {
      var label = equipment.Label.Trim();

      if (TryComputeLabelTotallyNew(variation, out var result))
        return result;

      if (TryComputeLabelWithReplacementBySchema(variation, label, out result))
        return result;

      if (TryComputeLabelWithReplacementByLocation(variation, label, out result))
        return result;

      if (TryComputeLabelWithDoubleReplacement(variation, label, out result))
        return result;

      if (TryComputeLabelWithComplementaryInfo(variation, label, out result))
        return result;

      return label;
    }

    private static bool TryComputeLabelTotallyNew(Variation variation, out string result)
    {
      if (variation.Schema == 7407)
      {
        result = "Nombre de cylindres";
        return true;
      }

      if (variation.Schema == 15304)
      {
        result = "Puissance (ch)";
        return true;
      }

      if (variation.Schema == 15305)
      {
        result = "Régime de puissance maxi (tr/mn)";
        return true;
      }

      result = null;
      return false;
    }

    private static bool TryComputeLabelWithReplacementBySchema(Variation variation, string label, out string result)
    {
      if (variation.Schema == 23502 || variation.Schema == 24002)
      {
        result = label.Replace("an(s) / km", ": durée (ans)");
        return true;
      }

      if (variation.Schema == 23503 || variation.Schema == 24003)
      {
        result = label.Replace("an(s) / km", ": kilométrage");
        return true;
      }

      if (variation.Schema == 7403)
      {
        result = label.Replace("litres / cm3", "litres");
        return true;
      }

      if (variation.Schema == 7402)
      {
        result = label.Replace("litres / cm3", "cm3");
        return true;
      }

      result = null;
      return false;
    }

    private static bool TryComputeLabelWithReplacementByLocation(Variation variation, string label, out string result)
    {
      if (variation.Schema == 23301)
      {
        if (variation.Location == 'F')
        {
          result = label.Replace("AV / AR", "AV");
          return true;
        }

        if (variation.Location == 'R')
        {
          result = label.Replace("AV / AR", "AR");
          return true;
        }
      }

      if (variation.Schema == 17811 || variation.Schema == 17818)
      {
        if (variation.Location == 'D')
        {
          result = label.Replace("conducteur / passager", "conducteur");
          return true;
        }

        if (variation.Location == 'P')
        {
          result = label.Replace("conducteur / passager", "passager");
          return true;
        }
      }

      result = null;
      return false;
    }

    private static bool TryComputeLabelWithDoubleReplacement(Variation variation, string label, out string result)
    {
      if (variation.Schema == 53405)
      {
        if (variation.Location == 'D')
        {
          result = label.Replace("recharge (rapide) A / V / h", "recharge : ampérage (A)");
          return true;
        }

        if (variation.Location == 'F')
        {
          result = label.Replace("recharge (rapide) A / V / h", "recharge rapide : ampérage (A)");
          return true;
        }
      }

      if (variation.Schema == 53404)
      {
        if (variation.Location == 'D')
        {
          result = label.Replace("recharge (rapide) A / V / h", "recharge : voltage (V)");
          return true;
        }

        if (variation.Location == 'F')
        {
          result = label.Replace("recharge (rapide) A / V / h", "recharge rapide : voltage (V)");
          return true;
        }
      }

      if (variation.Schema == 53403)
      {
        if (variation.Location == 'D')
        {
          result = label.Replace("recharge (rapide) A / V / h", "recharge : durée (heures)");
          return true;
        }

        if (variation.Location == 'F')
        {
          result = label.Replace("recharge (rapide) A / V / h", "recharge rapide : durée (heures)");
          return true;
        }
      }

      result = null;
      return false;
    }

    private static bool TryComputeLabelWithComplementaryInfo(Variation variation, string label, out string result)
    {
      if (variation.Schema == 14103)
      {
        result = label + " : largeur";
        return true;
      }

      if (variation.Schema == 14104)
      {
        result = label + " : profil";
        return true;
      }

      result = null;
      return false;
    }
  }
}