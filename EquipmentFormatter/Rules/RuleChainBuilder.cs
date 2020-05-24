using System;
using System.Collections.Generic;
using System.Linq;

namespace EquipmentFormatter.Rules
{
  public static class RuleChainBuilder
  {
    public static IRuleChainBuilderTakingCondition<TCriteria, TData> Given<TCriteria, TData>(TCriteria criteria, TData data) =>
      new RuleChainBuilder<TCriteria, TData>(criteria, data);
  }

  public interface IRuleChainBuilderTakingCondition<TCriteria, TData>
  {
    IRuleChainBuilderTakingOperation<TCriteria, TData> When(Condition<TCriteria> condition);
  }

  public interface IRuleChainBuilderTakingOperation<TCriteria, TData>
  {
    IRuleChainBuilderTakingConditionOrEnding<TCriteria, TData> Then(Func<TData, TData> operation);
  }

  public interface IRuleChainBuilderEnding<TData>
  {
    TData Else(Func<TData, TData> operation);
  }

  public interface IRuleChainBuilderTakingConditionOrEnding<TCriteria, TData> :
    IRuleChainBuilderTakingCondition<TCriteria, TData>,
    IRuleChainBuilderEnding<TData> {}

  public sealed class RuleChainBuilder<TCriteria, TData> :
    IRuleChainBuilderTakingConditionOrEnding<TCriteria, TData>,
    IRuleChainBuilderTakingOperation<TCriteria, TData>
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

    public IRuleChainBuilderTakingOperation<TCriteria, TData> When(Condition<TCriteria> condition)
    {
      Condition = condition;
      return this;
    }

    public IRuleChainBuilderTakingConditionOrEnding<TCriteria, TData> Then(Func<TData, TData> operation)
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