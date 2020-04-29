module Vehicle.Domain.Ports

type Label = Label of string

type InsuranceFeature = Years | Mileage
type Location = Front | Rear
type RechargeMeasure = Amperage | Voltage | Hours
type RechargeSpeed = Domestic | Fast
type SeatType = Driver | Passenger
type TyreTrait = Width | Profile
type VolumeUnit = Liters | Cm3

type Schema =
    | MaxPowerSpeed
    | NumberOfCylinders
    | PowerDin
    | Insurance of InsuranceFeature
    | Location of Location
    | Recharge of RechargeMeasure * RechargeSpeed
    | Seat of SeatType
    | Tyre of TyreTrait
    | Volume of VolumeUnit
    | Other
