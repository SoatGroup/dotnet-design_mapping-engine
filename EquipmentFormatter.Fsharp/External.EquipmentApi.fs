module External.EquipmentApi

type Equipment = { Schema: int; Label: string; Variations: Variation[] }
and  Variation = { Schema: int; Location: char; Value: string }
