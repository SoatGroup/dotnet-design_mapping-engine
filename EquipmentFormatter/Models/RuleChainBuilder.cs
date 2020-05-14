using System;
using System.Collections.Immutable;
using System.Linq;

namespace EquipmentFormatter.Models
{
  public static class RuleChainBuilder
  {
    public static TakingCriteria Case() =>
      new TakingCriteria(ImmutableStack<Rule>.Empty);

    public sealed class TakingCriteria
    {
      private ImmutableStack<Rule> Rules { get; }

      public TakingCriteria(ImmutableStack<Rule> rules)
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

      public TakingCriteria ExchangeWith(string value) =>
        Build(_ => value);

      public TakingCriteria Replace(string part, string by) =>
        Build(label => label.Replace(part, by));

      public TakingCriteria Append(string value) =>
        Build(label => label + value);

      private TakingCriteria Build(Func<string, string> operation) =>
        new TakingCriteria(Rules.Push(new Rule(Criteria, operation)));
    }
  }
}