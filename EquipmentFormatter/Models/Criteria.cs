using System;
using System.Collections.Immutable;
using EquipmentFormatter.External;

namespace EquipmentFormatter.Models
{
  public class Criteria
  {
    public static Criteria LocationIs(char location) =>
      new Criteria(variation => variation.Location == location);

    public static Criteria SchemaIs(int schema) =>
      new Criteria(variation => variation.Schema == schema);

    public static Criteria SchemaIsIn(params int[] schemas)
    {
      var schemaSet = schemas.ToImmutableHashSet();
      return new Criteria(variation => schemaSet.Contains(variation.Schema));
    }

    private Func<Variation, bool> Predicate { get; }

    private Criteria(Func<Variation, bool> predicate)
    {
      Predicate = predicate;
    }

    public static Criteria operator &(Criteria a, Criteria b) =>
      new Criteria(variation => a.Predicate(variation) && b.Predicate(variation));

    public static implicit operator Func<Variation, bool>(Criteria criteria) => criteria.Predicate;
  }
}
