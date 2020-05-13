using EquipmentFormatter.External;
using FluentAssertions;
using FluentAssertions.Primitives;
using Xunit;

namespace EquipmentFormatter.Tests
{
  public class EquipmentFormatterShould
  {
    [Theory]
    [InlineData(7407, "Nombre de cylindres")]
    [InlineData(15304, "Puissance (ch)")]
    [InlineData(15305, "Régime de puissance maxi (tr/mn)")]
    public void Exchange_Label(int schema, string expected) =>
      ComputeLabelShould(schema)
        .Be(expected);

    [Theory]
    [InlineData(23502, "Garantie an(s) / km",      "Garantie : durée (ans)")]
    [InlineData(23503, "Garantie an(s) / km",      "Garantie : kilométrage")]
    [InlineData(24002, "Assistance an(s) / km",    "Assistance : durée (ans)")]
    [InlineData(24003, "Assistance an(s) / km",    "Assistance : kilométrage")]
    [InlineData(7403,  "Cylindrée (litres / cm3)", "Cylindrée (litres)")]
    [InlineData(7402,  "Cylindrée (litres / cm3)", "Cylindrée (cm3)")]
    public void Replace_Label_Part_Given_Schema_Only(int schema, string label, string expected) =>
      ComputeLabelShould(schema, label)
        .Be(expected);

    [Theory]
    [InlineData(23301, 'F', "Vitres électriques AV / AR", "Vitres électriques AV")]
    [InlineData(23301, 'R', "Vitres électriques AV / AR", "Vitres électriques AR")]
    [InlineData(17811, 'D', "Siège conducteur / passager chauffant", "Siège conducteur chauffant")]
    [InlineData(17811, 'P', "Siège conducteur / passager chauffant", "Siège passager chauffant")]
    [InlineData(17818, 'D', "Siège conducteur / passager massant", "Siège conducteur massant")]
    [InlineData(17818, 'P', "Siège conducteur / passager massant", "Siège passager massant")]
    [InlineData(53405, 'D', "Informations de recharge (rapide) A / V / h", "Informations de recharge : ampérage (A)")]
    [InlineData(53404, 'D', "Informations de recharge (rapide) A / V / h", "Informations de recharge : voltage (V)")]
    [InlineData(53403, 'D', "Informations de recharge (rapide) A / V / h", "Informations de recharge : durée (heures)")]
    [InlineData(53405, 'F', "Informations de recharge (rapide) A / V / h", "Informations de recharge rapide : ampérage (A)")]
    [InlineData(53404, 'F', "Informations de recharge (rapide) A / V / h", "Informations de recharge rapide : voltage (V)")]
    [InlineData(53403, 'F', "Informations de recharge (rapide) A / V / h", "Informations de recharge rapide : durée (heures)")]
    public void Replace_Label_Part_Given_Schema_And_Location(int schema, char location, string label, string expected) =>
      ComputeLabelShould(schema, label, location)
        .Be(expected);

    [Theory]
    [InlineData(14103, 'F', "Pneus AV", "Pneus AV : largeur")]
    [InlineData(14104, 'F', "Pneus AV", "Pneus AV : profil")]
    [InlineData(14103, 'R', "Pneus AR", "Pneus AR : largeur")]
    [InlineData(14104, 'R', "Pneus AR", "Pneus AR : profil")]
    public void Append_Info_To_Label(int schema, char location, string label, string expected) =>
      ComputeLabelShould(schema, label, location)
        .Be(expected);

    private static StringAssertions ComputeLabelShould(int schema, string label = "AnyLabel", char location = default)
    {
      var equipment = new Equipment { Label = label };
      var variation = new Variation { Schema = schema, Location = location };
      return ComputeLabelShould(equipment, variation);
    }

    private static StringAssertions ComputeLabelShould(Equipment equipment, Variation variation) =>
      EquipmentMapper.ComputeLabel(equipment, variation)
                     .Should();
  }
}