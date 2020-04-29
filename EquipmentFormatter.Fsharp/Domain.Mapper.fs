module Vehicle.Domain.Mapper

open Vehicle.Shared
open Vehicle.Domain.Ports
open Vehicle.Domain.Operation

let private mapInsurance = function
    | Years -> "durée (ans)"
    | Mileage -> "kilométrage"

let private mapLocation = function
    | Front -> "AV"
    | Rear -> "AR"

let private mapSeat = function
    | Driver -> "conducteur"
    | Passenger -> "passager"

let private mapRechargeSpeed = function
    | Domestic -> "recharge"
    | Fast -> "recharge rapide"

let private mapRechargeMeasure = function
    | Amperage -> "ampérage (A)"
    | Voltage -> "voltage (V)"
    | Hours -> "durée (heures)"

let private mapRecharge measure speed =
    sprintf "%s : %s" (mapRechargeSpeed speed) (mapRechargeMeasure measure)

let private mapTyre = function
    | Width -> "largeur"
    | Profile -> "profil"

let private mapVolume = function
    | Liters -> "litres"
    | Cm3 -> "cm3"

let private tryMapToOperation = function
    | MaxPowerSpeed -> Some(Exchange "Régime de puissance maxi (tr/mn)")
    | NumberOfCylinders -> Some(Exchange "Nombre de cylindres")
    | PowerDin -> Some(Exchange "Puissance (ch)")
    | Insurance x -> Some(Replacement("an(s) / km", ": " + (mapInsurance x)))
    | Volume x -> Some(Replacement("litres / cm3", mapVolume x))
    | Seat x -> Some(Replacement("conducteur / passager", mapSeat x))
    | Recharge (x, y) -> Some (Replacement ("recharge (rapide) A / V / h", mapRecharge x y))
    | Location x -> Some(Replacement("AV / AR", mapLocation x))
    | Tyre x -> Some(Supplement(" : " + (mapTyre x)))
    | Other -> None

let computeLabel label schema =
    schema
    |> tryMapToOperation
    |> Option.map (proceed label)
    |> defaultIfNone label
