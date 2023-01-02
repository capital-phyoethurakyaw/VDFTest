using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VFD1.Entity
{
   public class staticCache
    {
        //CSV cant make multi Sheets on same file so we will have to split like this.
        //Make Sure that Full permission on that Paths exist because it contains READ/WRITE access.
        public static string DataSourceComboSetting = @"C:\ComboSetting.csv";
        public static string DataSourceProjectList = @"C:\ProjectList.csv";
        public static string DataSourceInstrumentList = @"C:\InstrumentList.csv";
        public static string DataSourceCableList = @"C:\CableList.csv";
        public static string DataSourceCableDuctList = @"C:\CableDuctList.csv"; 
    }
}
