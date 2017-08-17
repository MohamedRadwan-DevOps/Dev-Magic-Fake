using System;

using M.Radwan.DevMagicFake.Attributes;

namespace M.Radwan.EntitiesTest
{
    [Fakeable]
    [Serializable]
    public class Feedback
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public DateTime IssueDate { get; set; }

        public string Note { get; set; }
    }
}
