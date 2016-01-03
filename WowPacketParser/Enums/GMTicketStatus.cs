namespace WowPacketParser.Enums
{
    public enum GMTicketStatus
    {
        GMTICKET_STATUS_HASTEXT = 0x06,
        GMTICKET_STATUS_DEFAULT = 0x0A
    };

    // from blizzard lua
    public enum GMTicketSystemStatus
    {
        GMTICKET_QUEUE_STATUS_DISABLED = 0,
        GMTICKET_QUEUE_STATUS_ENABLED = 1
    };

    // from Blizzard LUA:
    // GMTICKET_ASSIGNEDTOGM_STATUS_NOT_ASSIGNED = 0;    -- ticket is not currently assigned to a gm
    // GMTICKET_ASSIGNEDTOGM_STATUS_ASSIGNED = 1;        -- ticket is assigned to a normal gm
    // GMTICKET_ASSIGNEDTOGM_STATUS_ESCALATED = 2;        -- ticket is in the escalation queue
    // 3 is a custom value and should never actually be sent
    public enum GMTicketEscalationStatus
    {
        TICKET_UNASSIGNED = 0,
        TICKET_ASSIGNED = 1,
        TICKET_IN_ESCALATION_QUEUE = 2,
        TICKET_ESCALATED_ASSIGNED = 3
    };

    // from blizzard lua
    public enum GMTicketOpenedByGMStatus
    {
        GMTICKET_OPENEDBYGM_STATUS_NOT_OPENED = 0,      // ticket has never been opened by a gm
        GMTICKET_OPENEDBYGM_STATUS_OPENED = 1       // ticket has been opened by a gm
    };
}
