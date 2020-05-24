using System;

namespace EquipmentFormatter.Rules
{
  public class Condition<TCriteria>
  {
    private Func<TCriteria, bool> Predicate { get; }

    protected Condition(Func<TCriteria, bool> predicate)
    {
      Predicate = predicate;
    }

    public static Condition<TCriteria> operator &(Condition<TCriteria> a, Condition<TCriteria> b) =>
      new Condition<TCriteria>(criteria => a.Predicate(criteria) && b.Predicate(criteria));

    public static implicit operator Func<TCriteria, bool>(Condition<TCriteria> condition) =>
      condition.Predicate;
  }
}