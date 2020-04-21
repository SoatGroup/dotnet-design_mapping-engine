namespace EquipmentFormatter
{
  public static class EquipmentMapper
  {
    public static string ComputeLabel(Equipment equipment, Variation variation)
    {
      var label = equipment.Label.Trim();

      var result = ComputeLabelTotallyNew(variation);
      if (result != null) return result;

      result = ComputeLabelWithReplacementBySchema(variation, label);
      if (result != null) return result;

      result = ComputeLabelWithReplacementByLocation(variation, label);
      if (result != null) return result;

      result = ComputeLabelWithDoubleReplacement(variation, label);
      if (result != null) return result;

      result = ComputeLabelWithComplementaryInfo(variation, label);
      if (result != null) return result;

      return label;
    }

    private static string ComputeLabelTotallyNew(Variation variation)
    {
      if (variation.Schema == 7407)
        return "Nombre de cylindres";

      if (variation.Schema == 15304)
        return "Puissance (ch)";

      if (variation.Schema == 15305)
        return "Régime de puissance maxi (tr/mn)";

      return null;
    }

    private static string ComputeLabelWithReplacementBySchema(Variation variation, string label)
    {
      if (variation.Schema == 23502 || variation.Schema == 24002)
        return label.Replace("an(s) / km", ": durée (ans)");

      if (variation.Schema == 23503 || variation.Schema == 24003)
        return label.Replace("an(s) / km", ": kilométrage");

      if (variation.Schema == 7403)
        return label.Replace("litres / cm3", "litres");

      if (variation.Schema == 7402)
        return label.Replace("litres / cm3", "cm3");
      return null;
    }

    private static string ComputeLabelWithReplacementByLocation(Variation variation, string label)
    {
      if (variation.Schema == 23301)
      {
        if (variation.Location == 'F')
          return label.Replace("AV / AR", "AV");

        if (variation.Location == 'R')
          return label.Replace("AV / AR", "AR");
      }

      if (variation.Schema == 17811 || variation.Schema == 17818)
      {
        if (variation.Location == 'D')
          return label.Replace("conducteur / passager", "conducteur");

        if (variation.Location == 'P')
          return label.Replace("conducteur / passager", "passager");
      }

      return null;
    }

    private static string ComputeLabelWithDoubleReplacement(Variation variation, string label)
    {
      if (variation.Schema == 53405)
      {
        if (variation.Location == 'D')
          return label.Replace("recharge (rapide) A / V / h", "recharge : ampérage (A)");

        if (variation.Location == 'F')
          return label.Replace("recharge (rapide) A / V / h", "recharge rapide : ampérage (A)");
      }

      if (variation.Schema == 53404)
      {
        if (variation.Location == 'D')
          return label.Replace("recharge (rapide) A / V / h", "recharge : voltage (V)");

        if (variation.Location == 'F')
          return label.Replace("recharge (rapide) A / V / h", "recharge rapide : voltage (V)");
      }

      if (variation.Schema == 53403)
      {
        if (variation.Location == 'D')
          return label.Replace("recharge (rapide) A / V / h", "recharge : durée (heures)");

        if (variation.Location == 'F')
          return label.Replace("recharge (rapide) A / V / h", "recharge rapide : durée (heures)");
      }

      return null;
    }

    private static string ComputeLabelWithComplementaryInfo(Variation variation, string label)
    {
      if (variation.Schema == 14103)
        return label + " : largeur";

      if (variation.Schema == 14104)
        return label + " : profil";

      return null;
    }
  }
}