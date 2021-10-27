#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
# endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using Sardauscan.Core.IO;
using System.Drawing;
using Sardauscan.Core.Interface;


namespace Sardauscan.Core
{
	/// <summary>
	/// Application Settings
	/// </summary>
    public class Settings : IDisposable
    {
        #region DEFINES
			/// <summary></summary>
        public const string CAMERA = "CAMERA";
				/// <summary></summary>
				public const string X = "X";
				/// <summary></summary>
				public const string Y = "Y";
				/// <summary></summary>
				public const string Z = "Z";

				/// <summary></summary>
				public const string TABLE = "TABLE";
				/// <summary></summary>
				public const string HEIGHT = "HEIGHT";
				/// <summary></summary>
				public const string DIAMETER = "DIAMETER";


				/// <summary></summary>
				public static string LASER(int index) { return String.Format("LASER_{0}", index); }
				/// <summary></summary>
				public static string LASER_COMMON = "LASER";
				/// <summary></summary>
				public const string MAGNITUDE_THRESHOLD = "MAGNITUDE_THRESHOLD";
				/// <summary></summary>
				/// <summary></summary>
				public const string MAX_WIDTH = "MAX_WIDTH";
				/// <summary></summary>
                public const string MIN_WIDTH = "MIN_WIDTH";
                /// <summary></summary>
                public const string FADE_DELAY = "FADE_DELAY";
                /// <summary></summary>
				public const string ROTATION = "ROTATION";
				/// <summary></summary>
				public const string TRANSLATIONX = "TRANSLATIONX";
				/// <summary></summary>
				public const string TRANSLATIONY = "TRANSLATIONY";
				/// <summary></summary>
				public const string SCALE = "SCALE";



				/// <summary></summary>
				public const string COM = "COM";
				/// <summary></summary>
				public const string LASER_TIMEOUT = "LASER_TIMEOUT";
				/// <summary></summary>
				public const string TABLE_TIMEOUT = "TABLE_TIMEOUT";
				/// <summary></summary>
				/// <summary></summary>
				public const string CONNECTION_TIMEOUT = "CONNECTION_TIMEOUT";
				/// <summary></summary>
				public const string INFO_TIMEOUT = "INFO_TIMEOUT";


				/// <summary></summary>
				public const string SCANNER = "SCANNER";
				/// <summary></summary>
				public const string PRECISION = "PRECISION";
				/// <summary></summary>
				public const string CALIBRATIONPRECISION = "CALIBRATIONPRECISION";
				/// <summary></summary>
				public const string DEFAULTCOLOR = "DEFAULTCOLOR";
				/// <summary></summary>
				public const string USEBITMAP = "USEBITMAP";
				/// <summary></summary>
				public const string CENTERDETECTIONMODE = "CENTERDETECTIONMODE";

				/// <summary></summary>
				public const string LAST_USED = "LAST_USED";

				static string HardwareName<T>()
				{
					if (typeof(ICameraProxy).IsAssignableFrom(typeof(T)))
						return "ICameraProxy";
					if (typeof(ITurnTableProxy).IsAssignableFrom(typeof(T)))
						return "ITurnTableProxy";
					if (typeof(ILaserProxy).IsAssignableFrom(typeof(T)))
						return "ILaserProxy";
					return string.Empty;
				}
				/// <summary>
				/// </summary>
				/// <param name="type"></param>
				/// <returns></returns>
				public static string PROXY<T>() { return HardwareName<T>() + "_PROXY"; }
				/// <summary>
				/// </summary>
				/// <param name="type"></param>
				/// <returns></returns>
				public static string HARDWAREID<T>() { return HardwareName<T>() + "_HARDWAREID"; }
				/// <summary></summary>
				/// <summary></summary>
				public const string COM_PORT = "COM_PORT";
				/// <summary></summary>
				public const string CAMERA_DEVICE = "CAMERA_DEVICE";
				/// <summary></summary>
				public const string CAMERA_RESOLUTION = "CAMERA_RESOLUTION";

				/// <summary></summary>
				public const string SYSTEM = "SYSTEM";
				/// <summary></summary>
				public const string MAXTHREADS = "MAXTHREADS";

				/// <summary></summary>
				public const String OPENGL = "OPENGL";
				/// <summary></summary>
				public const String LIGHTNING = "LIGHTNING";
				/// <summary></summary>
				public const String SHOWSCENECOLOR = "SHOWSCENECOLOR";
				/// <summary></summary>
				public const String SHOWBOUNDINGBOX = "SHOWBOUNDINGBOX";
				/// <summary></summary>
				public const String DEFAULTMATERIAL = "DEFAULTMATERIAL";
				/// <summary></summary>
				public const String SMOOTH = "SMOOTH";
				/// <summary></summary>
				public const String PROJECTION = "PROJECTION";
				/// <summary></summary>
				public const String WIREFRAME = "WIREFRAME";
        
        #endregion

        private XmlConfig xmlConfig;


        public Settings(string filename)
        {
            FileName = filename;
            xmlConfig = new XmlConfig(filename, true);
        }

        public String FileName { get; private set; }


				/// <summary>
				/// Dispose object
				/// </summary>
				public void Dispose()
        {
            xmlConfig.Commit();
        }

        private bool Exist(string tableName, string columnName)
        {
            return (
                xmlConfig.Settings.ChildrenNames != null &&
                xmlConfig.Settings.ChildrenNames.Contains(tableName) &&
                xmlConfig.Settings[tableName] != null &&
                xmlConfig.Settings[tableName].ChildrenNames != null &&
                xmlConfig.Settings[tableName].ChildrenNames.Contains(columnName)
                );
        }
        #region COLOR
         public Color Read(string tableName, string columnName, Color? def)
        {
            Color defCol = Color.Transparent;
            if(def!=null)
                defCol = (Color)def;
            try
            {
                if (Exist(tableName, columnName))
                    return xmlConfig.Settings[tableName][columnName].ColorValue;
                Write(tableName, columnName, defCol);
                return xmlConfig.Settings[tableName][columnName].ColorValue;
            }
            catch
            {
                return defCol;
            }
        }
        public void Write(string tableName, string columnName, Color val)
        {
            xmlConfig.Settings[tableName][columnName].ColorValue = val;
            xmlConfig.Commit();
        }
        #endregion
				#region Enum
				public Enum Read(string tableName, string columnName, Enum def) 
				{
					try
					{
						if (Exist(tableName, columnName))
							return (Enum) Enum.Parse(def.GetType(), xmlConfig.Settings[tableName][columnName].Value);
						Write(tableName, columnName, def);
						return (Enum)Enum.Parse(def.GetType(), xmlConfig.Settings[tableName][columnName].Value);
					}
					catch
					{
						return def;
					}
				}
				public void Write(string tableName, string columnName, Enum val)
				{
					xmlConfig.Settings[tableName][columnName].Value = val.ToString();
					xmlConfig.Settings[tableName][columnName].ValueType = val.GetType();
					xmlConfig.Commit();
				}
				#endregion
				#region BOOL
				public bool Read(string tableName, string columnName, bool def)
        {
            try
            {
                if (Exist(tableName, columnName))
                    return xmlConfig.Settings[tableName][columnName].BoolValue;
                Write(tableName, columnName, def);
                return xmlConfig.Settings[tableName][columnName].BoolValue;
            }
            catch
            {
                return def;
            }
        }
        public void Write(string tableName, string columnName, bool val)
        {
            xmlConfig.Settings[tableName][columnName].BoolValue = val;
            xmlConfig.Commit();
        }
        #endregion
        #region Int
        /** Read an integer from the settings database */
        public int Read(string tableName, string columnName, int def)
        {
            try
            {
                if (Exist(tableName,columnName))
                    return xmlConfig.Settings[tableName][columnName].IntValue;
                Write(tableName, columnName, def);
                return xmlConfig.Settings[tableName][columnName].IntValue;
            }
            catch
            {
                return def;
            }
        }
        /** Updates the given integer value in the settings database */
        public void Write(string tableName, string columnName, int value)
        {
            xmlConfig.Settings[tableName][columnName].IntValue = value;
            xmlConfig.Commit();
        }
        #endregion
        #region String
        public string Read(string tableName, string columnName,string def)
        {
            try
            {
                if (Exist(tableName, columnName))
                    return xmlConfig.Settings[tableName][columnName].Value;
                Write(tableName, columnName, def);
                return xmlConfig.Settings[tableName][columnName].Value;
            }
            catch
            {
                return def;
            }
        }
        public void Write(string tableName, string columnName, string value)
        {
            xmlConfig.Settings[tableName][columnName].Value = value;
            xmlConfig.Commit();
        }
        #endregion
        #region double
        /** Read a doubleing point value from the settings database */
        public double Read(string tableName, string columnName,double def )
        {
            try
            {
                if (Exist(tableName, columnName))
                    return xmlConfig.Settings[tableName][columnName].doubleValue;
                Write(tableName, columnName,def);
                return xmlConfig.Settings[tableName][columnName].doubleValue;
            }
            catch
            {
                return def;
            }

        }

        /** Updates the given real value in the settings database */
        public void Write(string tableName, string columnName, double value)
        {
            xmlConfig.Settings[tableName][columnName].doubleValue = (double)value;
            xmlConfig.Commit();
        }
        #endregion


        #region GridComponent
        public PropertyGridSettingsComponent GetPropertyGridSettingsComponent()
        {
            return xmlConfig.GetPropertyGridSettingsComponent();
        }

        public void Update(PropertyGridSettingsComponent compo, bool commit = true)
        {
            xmlConfig.UpdateConfiguration(compo,commit);
        }
        #endregion

			/// <summary>
			/// Static function to get/set the Global object Reference
			/// </summary>
        #region INSTANCE<T> MANAGEMENT
        static List<object> m_Instances = new List<object>();

        public static void RegisterInstance(object obj, bool replaceIfExist = false)
        {
            bool add = true;
            if (obj is IRegistrableInstance)
                add = ((IRegistrableInstance)obj).OnRegister();
            if (!add)
                return;
            object o = Get(obj.GetType());
            if (o != null)
            {
                if (replaceIfExist)
                    m_Instances.Remove(o);
                else
                    throw new Exception(obj.GetType() + " instance Register twice");
            }

            m_Instances.Add(obj);
        }

				public static void DisposeAllRegistred()
				{
					for (int i = 0; i < m_Instances.Count; i++)
					{
						try
						{
							IDisposable disp = m_Instances as IDisposable;
							if (disp != null)
								disp.Dispose();
						}
						catch { }
					}
				}
				public static void UnRegisterInstance(object obj,bool dispose=false)
        {
            m_Instances.Remove(obj);
						if (obj is IDisposable)
						{
							try { ((IDisposable)obj).Dispose(); }
							catch { }
						}
        }

        public static object Get(Type type)
        {
            foreach (object o in m_Instances)
            {
                if (type.IsAssignableFrom(o.GetType()))
                    return o;
            }
            return null;
        }
        public static bool Exist<T>()
        {
            return Get<T>() != null;
        }
        public static T Get<T>()
        {
            return (T)Get(typeof(T));
        }
        #endregion
    }
}
