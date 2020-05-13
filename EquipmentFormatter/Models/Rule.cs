using System;
using EquipmentFormatter.External;

namespace EquipmentFormatter.Models
{
  public class Rule
  {
    private Func<Variation, bool> Criteria { get; }

    private Func<string, string> Operation { get; }

    public Rule(Func<Variation, bool> criteria, Func<string, string> operation)
    {
      Criteria  = criteria;
      Operation = operation;
    }

    public bool IsSatisfiedBy(Variation variation) => Criteria(variation);

    public string ApplyOn(string label) => Operation(label);
  }
}
