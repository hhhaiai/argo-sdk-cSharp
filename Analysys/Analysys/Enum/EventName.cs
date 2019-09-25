using System.ComponentModel;

namespace Analysys
{
    public enum EventName
    {
        [Description("$profile_set")]
        P_SET,
        [Description("$profile_set_once")]
        P_SET_ONE,
        [Description("$profile_increment")]
        P_IN,
        [Description("$profile_append")]
        P_APP,
        [Description("$profile_unset")]
        P_UN,
        [Description("$profile_delete")]
        P_DEL,
        [Description("$alias")]
        ALIAS
    }
}
