namespace EquipmentFormatter
{
  public class EquipmentMapper
  {
    public string ComputeLabel(Equipment equipment, Variation variation)
    {
      var label = equipment.Label.Trim();

      // Totally new labels
      if (variation.Schema == 7407)
        return "Nombre de cylindres";

      if (variation.Schema == 15304)
        return "Puissance (ch)";

      if (variation.Schema == 15305)
        return "Régime de puissance maxi (tr/mn)";

      // Replacement of just a part of the label
      if (variation.Schema == 23502 || variation.Schema == 24002)
        return label.Replace("an(s) / km", ": durée (ans)");

      if (variation.Schema == 23503 || variation.Schema == 24003)
        return label.Replace("an(s) / km", ": kilométrage");

      if (variation.Schema == 7403)
        return label.Replace("litres / cm3", "litres");

      if (variation.Schema == 7402)
        return label.Replace("litres / cm3", "cm3");

      // Idem according to Location
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

      // Double replacement at once, one according to Schema, the other to Location
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

      // Add complementary info
      if (variation.Schema == 14103)
        return label + " : largeur";

      if (variation.Schema == 14104)
        return label + " : profil";

      // Default label
      return label;
    }
  }
}