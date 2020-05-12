using System.Linq;
using EquipmentFormatter.External;

namespace EquipmentFormatter
{
  public static class EquipmentMapper
  {
    public static string ComputeLabel(Equipment equipment, Variation variation)
    {
      var label = equipment.Label.Trim();
      return ComputeLabel(variation, label);
    }

    private static string ComputeLabel(Variation variation, string label) =>
      new[]
      {
        new Rule(Criteria.BySchema(7407),  Operation.Exchange("Nombre de cylindres")),
        new Rule(Criteria.BySchema(15304), Operation.Exchange("Puissance (ch)")),
        new Rule(Criteria.BySchema(15305), Operation.Exchange("Régime de puissance maxi (tr/mn)")),

        new Rule(Criteria.BySchemas(23502, 24002), Operation.Replace("an(s) / km", ": durée (ans)")),
        new Rule(Criteria.BySchemas(23503, 24003), Operation.Replace("an(s) / km", ": kilométrage")),

        new Rule(Criteria.BySchema(7403),  Operation.Replace("litres / cm3", "litres")),
        new Rule(Criteria.BySchema(7402),  Operation.Replace("litres / cm3", "cm3")),

        new Rule(Criteria.BySchemaAndLocation(23301, 'F'), Operation.Replace("AV / AR", "AV")),
        new Rule(Criteria.BySchemaAndLocation(23301, 'R'), Operation.Replace("AV / AR", "AR")),

        new Rule(Criteria.BySchemaAndLocation(17811, 'D'), Operation.Replace("conducteur / passager", "conducteur")),
        new Rule(Criteria.BySchemaAndLocation(17818, 'D'), Operation.Replace("conducteur / passager", "conducteur")),
        new Rule(Criteria.BySchemaAndLocation(17811, 'P'), Operation.Replace("conducteur / passager", "passager")),
        new Rule(Criteria.BySchemaAndLocation(17818, 'P'), Operation.Replace("conducteur / passager", "passager")),

        new Rule(Criteria.BySchemaAndLocation(53405, 'F'), Operation.Replace("recharge (rapide) A / V / h", "recharge rapide : ampérage (A)")),
        new Rule(Criteria.BySchemaAndLocation(53404, 'F'), Operation.Replace("recharge (rapide) A / V / h", "recharge rapide : voltage (V)")),
        new Rule(Criteria.BySchemaAndLocation(53403, 'F'), Operation.Replace("recharge (rapide) A / V / h", "recharge rapide : durée (heures)")),
        new Rule(Criteria.BySchemaAndLocation(53405, 'D'), Operation.Replace("recharge (rapide) A / V / h", "recharge : ampérage (A)")),
        new Rule(Criteria.BySchemaAndLocation(53404, 'D'), Operation.Replace("recharge (rapide) A / V / h", "recharge : voltage (V)")),
        new Rule(Criteria.BySchemaAndLocation(53403, 'D'), Operation.Replace("recharge (rapide) A / V / h", "recharge : durée (heures)")),

        new Rule(Criteria.BySchema(14103), Operation.Supplement(" : largeur")),
        new Rule(Criteria.BySchema(14104), Operation.Supplement(" : profil")),
      }
      .Where(rule => rule.IsSatisfiedBy(variation))
      .Select(rule => rule.ApplyOn(label))
      .Take(1)
      .DefaultIfEmpty(label)
      .First();
  }
}