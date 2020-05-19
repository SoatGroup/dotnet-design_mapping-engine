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

    public IOperation SelectOperationAdaptedTo(Variation variation) =>
      rules.Where(rule => rule.IsSatisfiedBy(variation))
           .DefaultIfEmpty(new Rule(_ => true, label => label))
           .First();
  }
}