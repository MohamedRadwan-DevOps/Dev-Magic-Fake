using System;
using System.Collections.Generic;

using M.Radwan.DevMagicFake.Attributes;

namespace M.Radwan.EntitiesTest
{
    
    [Fakeable]
    [Serializable]
    public class Customer
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public short Percentage { get; set; }

        public string Name { get; set; }

        public Country CountryLocation { get; set; }

        public DateTime date { get; set; }

        public List<Order> Orders { get; set; }

        public List<Feedback> Feedbacks { get; set; }
    }
}