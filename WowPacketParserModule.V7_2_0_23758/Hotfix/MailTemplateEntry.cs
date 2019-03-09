using WowPacketParser.Enums;
using WowPacketParser.Hotfix;

namespace WowPacketParserModule.V7_2_0_23758.Hotfix
{
    [HotfixStructure(DB2Hash.MailTemplate, HasIndexInData = false)]
    public class MailTemplateEntry
    {
        public string Body { get; set; }
    }
}
