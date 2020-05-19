using System;
using EquipmentFormatter.External;

namespace EquipmentFormatter.Models
{
  public sealed class Rule : IOperation
  {
    public static Rule Default(Func<string, string> operation) =>
      new Rule(_ => true, operation);

    private readonly Func<Variation, bool> isSatisfiedBy;
    private readonly Func<string, string>  applyOn;

    public Rule(Func<Variation, bool> isSatisfiedBy, Func<string, string> applyOn)
    {
      this.isSatisfiedBy = isSatisfiedBy;
      this.applyOn       = applyOn;
    }

    public bool IsSatisfiedBy(Variation variation) =>
      isSatisfiedBy(variation);

    public string ApplyOn(string label) =>
      applyOn(label);
  }
}