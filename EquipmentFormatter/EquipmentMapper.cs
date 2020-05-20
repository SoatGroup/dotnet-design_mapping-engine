using EquipmentFormatter.External;
using EquipmentFormatter.Models;
using static EquipmentFormatter.LabelOperation;
using static EquipmentFormatter.Models.RuleChainBuilder;
using static EquipmentFormatter.VariationCondition;

namespace EquipmentFormatter
{
  public static class EquipmentMapper
  {
    private static readonly RuleChain<Variation, string> Rules =
      Given<Variation, string>()
        .When(SchemaIs(7407)).Then(ExchangeWith("Nombre de cylindres"))
        .When(SchemaIs(15304)).Then(ExchangeWith("Puissance (ch)"))
        .When(SchemaIs(15305)).Then(ExchangeWith("Régime de puissance maxi (tr/mn)"))
        .When(SchemaIsIn(23502, 24002)).Then(Replace("an(s) / km", by: ": durée (ans)"))
        .When(SchemaIsIn(23503, 24003)).Then(Replace("an(s) / km", by: ": kilométrage"))
        .When(SchemaIs(7403)).Then(Replace("litres / cm3", by: "litres"))
        .When(SchemaIs(7402)).Then(Replace("litres / cm3", by: "cm3"))
        .When(SchemaIs(23301) & LocationIs('F')).Then(Replace("AV / AR", by: "AV"))
        .When(SchemaIs(23301) & LocationIs('R')).Then(Replace("AV / AR", by: "AR"))
        .When(SchemaIs(17811) & LocationIs('D')).Then(Replace("conducteur / passager", by: "conducteur"))
        .When(SchemaIs(17818) & LocationIs('D')).Then(Replace("conducteur / passager", by: "conducteur"))
        .When(SchemaIs(17811) & LocationIs('P')).Then(Replace("conducteur / passager", by: "passager"))
        .When(SchemaIs(17818) & LocationIs('P')).Then(Replace("conducteur / passager", by: "passager"))
        .When(SchemaIs(53405) & LocationIs('F')).Then(Replace("recharge (rapide) A / V / h", by: "recharge rapide : ampérage (A)"))
        .When(SchemaIs(53404) & LocationIs('F')).Then(Replace("recharge (rapide) A / V / h", by: "recharge rapide : voltage (V)"))
        .When(SchemaIs(53403) & LocationIs('F')).Then(Replace("recharge (rapide) A / V / h", by: "recharge rapide : durée (heures)"))
        .When(SchemaIs(53405) & LocationIs('D')).Then(Replace("recharge (rapide) A / V / h", by: "recharge : ampérage (A)"))
        .When(SchemaIs(53404) & LocationIs('D')).Then(Replace("recharge (rapide) A / V / h", by: "recharge : voltage (V)"))
        .When(SchemaIs(53403) & LocationIs('D')).Then(Replace("recharge (rapide) A / V / h", by: "recharge : durée (heures)"))
        .When(SchemaIs(14103)).Then(Append(" : largeur"))
        .When(SchemaIs(14104)).Then(Append(" : profil"))
        .Else(label => label);

    public static string ComputeLabel(Equipment equipment, Variation variation) =>
      Rules.SelectOperationAdaptedTo(variation)
           .ApplyOn(equipment.Label.Trim());
  }
}