module Vehicle.Api.Adapters

open External.EquipmentApi
open Vehicle.Domain.Ports

let labelValue (Label value) = value

let parseEquipment (equipment: Equipment) = Label (equipment.Label.Trim())

let parseVariation (variation: Variation) =
    let parseBySchema next =
        match variation.Schema with
        | 7407 -> NumberOfCylinders
        | 15304 -> PowerDin
        | 15305 -> MaxPowerSpeed
        | 23502 | 24002 -> Insurance Years
        | 23503 | 24003 -> Insurance Mileage
        | 7403 -> Volume Liters
        | 7402 -> Volume Cm3
        | 14103 -> Tyre Width
        | 14104 -> Tyre Profile
        | _ -> next()

    let parseBySchemaAndLocation () =
        match variation.Schema, variation.Location with
        | 17811, 'D' | 17818, 'D' -> Seat Driver
        | 17811, 'P' | 17818, 'P' -> Seat Passenger
        | 53405, 'D' -> Recharge (Amperage, Domestic)
        | 53405, 'F' -> Recharge (Amperage, Fast)
        | 53404, 'D' -> Recharge (Voltage, Domestic)
        | 53404, 'F' -> Recharge (Voltage, Fast)
        | 53403, 'D' -> Recharge (Hours, Domestic)
        | 53403, 'F' -> Recharge (Hours, Fast)
        | 23301, 'F' -> Location Front
        | 23301, 'R' -> Location Rear
        | _ -> Other

    parseBySchemaAndLocation
    |> parseBySchema
