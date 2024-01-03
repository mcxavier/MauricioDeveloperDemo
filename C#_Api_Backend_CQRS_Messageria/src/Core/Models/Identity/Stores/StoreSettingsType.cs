using Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Identity.Stores
{
    public class StoreSettingsType : Enumeration
    {
        public StoreSettingsType(int id, string name) : base(id, name) { }
        public static StoreSettingsType PagarMeConfig = new StoreSettingsType(1, "PagarMeConfig");

    }

    public enum StoreSettingsTypeEnum
    {
        PagarMeConfig = 1
    }
}
