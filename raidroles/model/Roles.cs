[Flags]
enum Role : byte {
    NONE   = 0b0000_0000,
    Q_HEAL = 0b0000_0001,
    A_HEAL = 0b0000_0010,
    Q_DPS  = 0b0000_0100,
    A_DPS  = 0b0000_1000,
    DPS    = 0b0001_0000
}
