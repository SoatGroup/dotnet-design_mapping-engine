using System;

namespace EquipmentFormatter.Models
{
  public class Rule<TCriteria, TData> : IOperation<TData>
  {
    public static Rule<TCriteria, TData> Default(Func<TData, TData> operation) =>
      new Rule<TCriteria, TData>(_ => true, operation);

    private readonly Func<TCriteria, bool> isSatisfiedBy;
    private readonly Func<TData, TData>  applyOn;

    public Rule(Func<TCriteria, bool> isSatisfiedBy, Func<TData, TData> applyOn)
    {
      this.isSatisfiedBy = isSatisfiedBy;
      this.applyOn       = applyOn;
    }

    public bool IsSatisfiedBy(TCriteria criteria) =>
      isSatisfiedBy(criteria);

    public TData ApplyOn(TData data) =>
      applyOn(data);
  }
}