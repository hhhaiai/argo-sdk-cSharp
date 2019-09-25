using System.ComponentModel;

namespace Analysys
{
    public enum DEBUG
    {
        [Description("CLOSE")]
        CLOSE = 0,
        [Description("OPENNOSAVE")]
        OPENNOSAVE = 1,
        [Description("OPENANDSAVE")]
        OPENANDSAVE = 2
    }
}
