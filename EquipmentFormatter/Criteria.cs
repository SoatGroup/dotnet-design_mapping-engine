using System;
using System.Collections.Immutable;
using EquipmentFormatter.External;

namespace EquipmentFormatter
{
  public static class Criteria
  {
    public static Func<Variation, bool> BySchema(int schema) =>
      variation => variation.Schema == schema;

    public static Func<Variation, bool> BySchemaAndLocation(int schema, char location) =>
      variation => variation.Schema == schema && variation.Location == location;

    public static Func<Variation, bool> BySchemas(params int[] schemas)
    {
      var schemaSet = schemas.ToImmutableHashSet();
      return variation => schemaSet.Contains(variation.Schema);
    }
  }
}
