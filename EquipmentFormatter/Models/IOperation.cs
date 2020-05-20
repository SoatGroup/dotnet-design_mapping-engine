namespace EquipmentFormatter.Models
{
  public interface IOperation<TData>
  {
    TData ApplyOn(TData data);
  }
}