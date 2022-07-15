using System;
using System.Collections.Generic;

namespace TAG_DECODER.Devices.Common.Helpers
{
    [Serializable]
    public sealed class TagDataSection
    {
        public List<TagData> CapturedTagData { get; } = new List<TagData>();
    }
}
