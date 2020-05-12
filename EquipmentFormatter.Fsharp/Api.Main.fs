module Vehicle.Api.Main

open External.EquipmentApi
open Vehicle.Api.Adapters
open Vehicle.Domain.Mapping

let computeLabel (equipment: Equipment) (variation: Variation) =
    let label  = parseEquipment equipment
    let schema = parseVariation variation

    computeLabel label schema
    |> labelValue
