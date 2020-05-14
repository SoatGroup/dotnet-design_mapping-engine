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

      public RuleChain Else(string label) =>
        Rules.Aggregate(EndChain(label), (chain, rule) => new RuleChain(rule, chain));

      private static RuleChain EndChain(string label) =>
        new RuleChain(new Rule(_ => true, _ => label), null);
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