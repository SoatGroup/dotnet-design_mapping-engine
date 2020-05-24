using System;
using System.Collections.Generic;
using System.Linq;

namespace EquipmentFormatter.Models
{
  public static class RuleChainBuilder
  {
    public static ITakingCondition<TCriteria, TData> Given<TCriteria, TData>(TCriteria criteria, TData data) =>
      new RuleChainBuilder<TCriteria, TData>(criteria, data);
  }

  public interface ITakingCondition<TCriteria, TData>
  {
    ITakingOperation<TCriteria, TData> When(Condition<TCriteria> condition);
  }

  public interface ITakingConditionOrEnding<TCriteria, TData> : ITakingCondition<TCriteria, TData>
  {
    TData Else(Func<TData, TData> operation);
  }

  public interface ITakingOperation<TCriteria, TData>
  {
    ITakingConditionOrEnding<TCriteria, TData> Then(Func<TData, TData> operation);
  }

  public sealed class RuleChainBuilder<TCriteria, TData> :
    ITakingConditionOrEnding<TCriteria, TData>,
    ITakingOperation<TCriteria, TData>
  {
    private TCriteria Criteria { get; }
    private TData Data { get; }
    private Stack<Rule<TCriteria, TData>> Rules { get; } = new Stack<Rule<TCriteria, TData>>();
    private Condition<TCriteria> Condition { get; set; }

    public RuleChainBuilder(TCriteria criteria, TData data)
    {
      Criteria = criteria;
      Data     = data;
    }

    public ITakingOperation<TCriteria, TData> When(Condition<TCriteria> condition)
    {
      Condition = condition;
      return this;
    }

    public ITakingConditionOrEnding<TCriteria, TData> Then(Func<TData, TData> operation)
    {
      Rules.Push(new Rule<TCriteria, TData>(Condition, operation));
      return this;
    }

    public TData Else(Func<TData, TData> operation) =>
      Rules.Aggregate(seed: EndRuleChainWith(operation),
                      (chain, rule) => new RuleChain<TCriteria, TData>(rule, chain))
           .SelectOperationAdaptedTo(Criteria)
           .ApplyOn(Data);

    private static RuleChain<TCriteria, TData> EndRuleChainWith(Func<TData, TData> operation) =>
      new RuleChain<TCriteria, TData>(Rule<TCriteria, TData>.Default(operation), null);
  }
}