using System;

namespace EquipmentFormatter.Models
{
  public class RuleBuilder
  {
    public static RuleBuilder When(Criteria criteria) =>
      new RuleBuilder(criteria);

    private readonly Criteria criteria;

    private RuleBuilder(Criteria criteria)
    {
      this.criteria = criteria;
    }

    public Rule ExchangeWith(string value) =>
      Build(_ => value);

    public Rule Replace(string part, string by) =>
      Build(label => label.Replace(part, by));

    public Rule Append(string value) =>
      Build(label => label + value);

    private Rule Build(Func<string, string> operation) =>
      new Rule(criteria, operation);
  }
}