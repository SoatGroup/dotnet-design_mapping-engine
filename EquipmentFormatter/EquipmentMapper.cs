using EquipmentFormatter.External;
using EquipmentFormatter.Models;
using static EquipmentFormatter.Models.Criteria;
using static EquipmentFormatter.Models.RuleChainBuilder;

namespace EquipmentFormatter
{
  public static class EquipmentMapper
  {
    public static string ComputeLabel(Equipment equipment, Variation variation)
    {
      var label = equipment.Label.Trim();
      var ruleChain = RuleChainEndingWith(label);
      return ruleChain.Apply(variation, label);
    }

    private static RuleChain RuleChainEndingWith(string label) =>
      Case()
        .When(SchemaIs(7407)).ExchangeWith("Nombre de cylindres")
        .When(SchemaIs(15304)).ExchangeWith("Puissance (ch)")
        .When(SchemaIs(15305)).ExchangeWith("Régime de puissance maxi (tr/mn)")
        .When(SchemaIsIn(23502, 24002)).Replace("an(s) / km", by: ": durée (ans)")
        .When(SchemaIsIn(23503, 24003)).Replace("an(s) / km", by: ": kilométrage")
        .When(SchemaIs(7403)).Replace("litres / cm3", by: "litres")
        .When(SchemaIs(7402)).Replace("litres / cm3", by: "cm3")
        .When(SchemaIs(23301) & LocationIs('F')).Replace("AV / AR", by: "AV")
        .When(SchemaIs(23301) & LocationIs('R')).Replace("AV / AR", by: "AR")
        .When(SchemaIs(17811) & LocationIs('D')).Replace("conducteur / passager", by: "conducteur")
        .When(SchemaIs(17818) & LocationIs('D')).Replace("conducteur / passager", by: "conducteur")
        .When(SchemaIs(17811) & LocationIs('P')).Replace("conducteur / passager", by: "passager")
        .When(SchemaIs(17818) & LocationIs('P')).Replace("conducteur / passager", by: "passager")
        .When(SchemaIs(53405) & LocationIs('F')).Replace("recharge (rapide) A / V / h", by: "recharge rapide : ampérage (A)")
        .When(SchemaIs(53404) & LocationIs('F')).Replace("recharge (rapide) A / V / h", by: "recharge rapide : voltage (V)")
        .When(SchemaIs(53403) & LocationIs('F')).Replace("recharge (rapide) A / V / h", by: "recharge rapide : durée (heures)")
        .When(SchemaIs(53405) & LocationIs('D')).Replace("recharge (rapide) A / V / h", by: "recharge : ampérage (A)")
        .When(SchemaIs(53404) & LocationIs('D')).Replace("recharge (rapide) A / V / h", by: "recharge : voltage (V)")
        .When(SchemaIs(53403) & LocationIs('D')).Replace("recharge (rapide) A / V / h", by: "recharge : durée (heures)")
        .When(SchemaIs(14103)).Append(" : largeur")
        .When(SchemaIs(14104)).Append(" : profil")
        .Else(label);
  }
}