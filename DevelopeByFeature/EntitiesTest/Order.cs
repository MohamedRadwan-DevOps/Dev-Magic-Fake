using System;

using M.Radwan.DevMagicFake.Attributes;

namespace M.Radwan.EntitiesTest
{
    [Fakeable]
    [Serializable]
    public class Order
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public DateTime OrderDate { get; set; }

        public string OrderName { get; set; }
    }
}
