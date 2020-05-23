using System;
using System.Collections.Immutable;
using System.Linq;

namespace EquipmentFormatter.Models
{
  public static class RuleChainBuilder
  {
    public static RuleChainBuilder<TCriteria, TData>.ITakingCondition Given<TCriteria, TData>() =>
      new RuleChainBuilder<TCriteria, TData>.TakingConditionOrEnding(ImmutableStack<Rule<TCriteria, TData>>.Empty);
  }

  public static class RuleChainBuilder<TCriteria, TData>
  {
    public interface ITakingCondition
    {
      TakingOperation When(Condition<TCriteria> condition);
    }

    public sealed class TakingConditionOrEnding : ITakingCondition
    {
      private ImmutableStack<Rule<TCriteria, TData>> Rules { get; }

      public TakingConditionOrEnding(ImmutableStack<Rule<TCriteria, TData>> rules)
      {
        Rules = rules;
      }

      public TakingOperation When(Condition<TCriteria> condition) =>
        new TakingOperation(condition, Rules);

      public RuleChain<TCriteria, TData> Else(Func<TData, TData> operation) =>
        Rules.Aggregate(
          seed: EndRuleChainWith(operation),
          (chain, rule) => new RuleChain<TCriteria, TData>(rule, chain));

      private static RuleChain<TCriteria, TData> EndRuleChainWith(Func<TData, TData> operation) =>
        new RuleChain<TCriteria, TData>(Rule<TCriteria, TData>.Default(operation), null);
    }

    public sealed class TakingOperation
    {
      private Condition<TCriteria> Condition { get; }

      private ImmutableStack<Rule<TCriteria, TData>> Rules { get; }

      public TakingOperation(Condition<TCriteria> condition, ImmutableStack<Rule<TCriteria, TData>> rules)
      {
        Condition = condition;
        Rules     = rules;
      }

      public TakingConditionOrEnding Then(Func<TData, TData> operation) =>
        new TakingConditionOrEnding(Rules.Push(new Rule<TCriteria, TData>(Condition, operation)));
    }
  }
}