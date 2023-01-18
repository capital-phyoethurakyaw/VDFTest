using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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
        public static string DataSourceCableDuctSchedule = @"C:\CableDuctSchedule\";
        //Combo
        public static DataTable DataSourceInstrumentType()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Key");
            dt.Columns.Add("Name");
            dt.Columns.Add("Display");

            dt.Rows.Add(new object[] { "0_121", "Pressure Transmitter 121 ", "Pressure Transmitter 121" } );//121
            dt.Rows.Add(new object[] { "1_132", "Temp. Sensor(RTD) 132", "Temp. Sensor(RTD) 132" } );//132
            dt.Rows.Add(new object[] { "2_156", "Humidity & Temp. Sensor 156", "Humidity & Temp. Sensor 156" } );//156 Control Valve(Pneumatic)
            dt.Rows.Add(new object[] { "3_121", "Control Valve(Pneumatic) 121", "156 Control Valve(Pneumatic) 121" } );//121 
            dt.Rows.Add(new object[] { "4_121", "Analyzer 121", "Analyzer 121" } );//121

            return dt;
        }
    }
}
