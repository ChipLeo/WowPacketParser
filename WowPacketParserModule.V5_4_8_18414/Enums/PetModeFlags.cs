using System;

namespace WowPacketParserModule.V5_4_8_18414.Enums
{
    [Flags]
    public enum PetModeFlags
    {
        Unknown1       = 0x001,
        Unknown2       = 0x002,
        Unknown3       = 0x004,
        Unknown4       = 0x008,
        Unknown5       = 0x010,
        Unknown6       = 0x020,
        Unknown7       = 0x040,
        Unknown8       = 0x080,
        Unknown9       = 0x100,
        Unknown10      = 0x200,
        Unknown11      = 0x400,
        DisableActions = 0x800
    }

    public enum ReactState
    {
        Passive    = 0,
        Defensive  = 1,
        Aggressive = 2,
        Assist     = 3
    }

    public enum CommandState
    {
        Stay    = 0,
        Follow  = 1,
        Attack  = 2,
        Abandon = 3,
        MoveTo  = 4
    }
}
