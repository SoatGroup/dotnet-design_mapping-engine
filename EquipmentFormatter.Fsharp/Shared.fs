module Vehicle.Shared

/// `defaultArg` pipeable
let defaultIfNone defaultValue option =
    defaultArg option defaultValue
