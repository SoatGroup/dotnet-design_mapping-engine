using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using EquipmentFormatter.External;

namespace EquipmentFormatter.Models
{
  public class Rules
  {
    private readonly IReadOnlyList<Rule> rules;

    public Rules(params Rule[] rules)
    {
      this.rules = rules.ToImmutableList();
    }

    public string Apply(Variation variation, string label) =>
      rules.Where(rule => rule.IsSatisfiedBy(variation))
           .Select(rule => rule.ApplyOn(label))
           .Take(1)
           .DefaultIfEmpty(label)
           .First();
  }
}