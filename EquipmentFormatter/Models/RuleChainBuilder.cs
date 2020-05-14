using System;
using System.Collections.Generic;
using System.Linq;

namespace EquipmentFormatter.Models
{
  public class RuleChainBuilder
  {
    public static RuleChainBuilder Case() =>
      new RuleChainBuilder();

    private Criteria Criteria { get; set; }

    private Stack<Rule> Rules { get; } = new Stack<Rule>();

    public RuleChainBuilder When(Criteria criteria)
    {
      Criteria = criteria;
      return this;
    }

    public RuleChainBuilder ExchangeWith(string value) =>
      Build(_ => value);

    public RuleChainBuilder Replace(string part, string by) =>
      Build(label => label.Replace(part, by));

    public RuleChainBuilder Append(string value) =>
      Build(label => label + value);

    private RuleChainBuilder Build(Func<string, string> operation)
    {
      Rules.Push(new Rule(Criteria, operation));
      Criteria = null;
      return this;
    }

    public RuleChain Else(string label) =>
      Rules.Aggregate(RuleChain.End(label), (chain, rule) => new RuleChain(rule, chain));
  }
}