using System;
using System.Collections.Immutable;
using System.Linq;

namespace EquipmentFormatter.Models
{
  public static class RuleChainBuilder
  {
    public static RuleChainBuilder<TCriteria, TData>.ITakingCondition Given<TCriteria, TData>(TCriteria criteria, TData data) =>
      new RuleChainBuilder<TCriteria, TData>.TakingConditionOrEnding(criteria, data, ImmutableStack<Rule<TCriteria, TData>>.Empty);
  }

  public static class RuleChainBuilder<TCriteria, TData>
  {
    public interface ITakingCondition
    {
      TakingOperation When(Condition<TCriteria> condition);
    }

    public sealed class TakingConditionOrEnding : ITakingCondition
    {
      private TCriteria Criteria { get; }
      private TData Data { get; }
      private ImmutableStack<Rule<TCriteria, TData>> Rules { get; }

      public TakingConditionOrEnding(TCriteria criteria, TData data, ImmutableStack<Rule<TCriteria, TData>> rules)
      {
        Criteria = criteria;
        Data     = data;
        Rules    = rules;
      }

      public TakingOperation When(Condition<TCriteria> condition) =>
        new TakingOperation(condition, Criteria, Data, Rules);

      public TData Else(Func<TData, TData> operation) =>
        Rules.Aggregate(seed: EndRuleChainWith(operation),
                        (chain, rule) => new RuleChain<TCriteria, TData>(rule, chain))
             .SelectOperationAdaptedTo(Criteria)
             .ApplyOn(Data);

      private static RuleChain<TCriteria, TData> EndRuleChainWith(Func<TData, TData> operation) =>
        new RuleChain<TCriteria, TData>(Rule<TCriteria, TData>.Default(operation), null);
    }

    public sealed class TakingOperation
    {
      private Condition<TCriteria> Condition { get; }
      private TCriteria Criteria { get; }
      private TData Data { get; }
      private ImmutableStack<Rule<TCriteria, TData>> Rules { get; }

      public TakingOperation(Condition<TCriteria> condition, TCriteria criteria, TData data, ImmutableStack<Rule<TCriteria, TData>> rules)
      {
        Condition = condition;
        Criteria  = criteria;
        Data      = data;
        Rules     = rules;
      }

      public TakingConditionOrEnding Then(Func<TData, TData> operation) =>
        new TakingConditionOrEnding(Criteria, Data, Rules.Push(new Rule<TCriteria, TData>(Condition, operation)));
    }
  }
}