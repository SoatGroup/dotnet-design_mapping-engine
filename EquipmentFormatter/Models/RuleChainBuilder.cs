using System;
using System.Collections.Immutable;
using System.Linq;

namespace EquipmentFormatter.Models
{
  public static class RuleChainBuilder
  {
    public static ITakingCriteria Case() =>
      new TakingCriteriaOrEnding(ImmutableStack<Rule>.Empty);

    public interface ITakingCriteria
    {
      TakingOperation When(Criteria criteria);
    }

    public sealed class TakingCriteriaOrEnding : ITakingCriteria
    {
      private ImmutableStack<Rule> Rules { get; }

      public TakingCriteriaOrEnding(ImmutableStack<Rule> rules)
      {
        Rules = rules;
      }

      public TakingOperation When(Criteria criteria) =>
        new TakingOperation(criteria, Rules);

      public RuleChain Else(Func<string, string> operation) =>
        Rules.Aggregate(
          seed: EndRuleChainWith(operation),
          (chain, rule) => new RuleChain(rule, chain));

      private static RuleChain EndRuleChainWith(Func<string, string> operation) =>
        new RuleChain(Rule.Default(operation), null);
    }

    public sealed class TakingOperation
    {
      private Criteria Criteria { get; }

      private ImmutableStack<Rule> Rules { get; }

      public TakingOperation(Criteria criteria, ImmutableStack<Rule> rules)
      {
        Criteria = criteria;
        Rules    = rules;
      }

      public TakingCriteriaOrEnding ExchangeWith(string value) =>
        BuildRule(_ => value);

      public TakingCriteriaOrEnding Replace(string part, string by) =>
        BuildRule(label => label.Replace(part, by));

      public TakingCriteriaOrEnding Append(string value) =>
        BuildRule(label => label + value);

      private TakingCriteriaOrEnding BuildRule(Func<string, string> operation) =>
        new TakingCriteriaOrEnding(Rules.Push(new Rule(Criteria, operation)));
    }
  }
}