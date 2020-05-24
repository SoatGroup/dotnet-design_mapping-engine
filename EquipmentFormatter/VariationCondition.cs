using System;
using System.Collections.Immutable;
using EquipmentFormatter.External;
using EquipmentFormatter.Rules;

namespace EquipmentFormatter
{
  public sealed class VariationCondition : Condition<Variation>
  {
    public static VariationCondition LocationIs(char location) =>
      new VariationCondition(variation => variation.Location == location);

    public static VariationCondition SchemaIs(int schema) =>
      new VariationCondition(variation => variation.Schema == schema);

    public static VariationCondition SchemaIsIn(params int[] schemas)
    {
      var schemaSet = schemas.ToImmutableHashSet();
      return new VariationCondition(variation => schemaSet.Contains(variation.Schema));
    }

    private VariationCondition(Func<Variation, bool> predicate) : base(predicate) { }
  }
}