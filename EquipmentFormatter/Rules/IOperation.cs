namespace EquipmentFormatter.Rules
{
  public interface IOperation<TData>
  {
    TData ApplyOn(TData data);
  }
}