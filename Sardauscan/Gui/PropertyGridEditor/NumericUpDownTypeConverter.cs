#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Sardauscan.Gui.PropertyGridEditor
{
   /// <summary>
   /// Range modification for direct edit override
   /// </summary>
   public class NumericUpDownTypeConverter : TypeConverter 
   {
      public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
      {
         // Attempt to do them all
         return true;
      }


      public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
      {
         try
         {
            string Value;
            if (!(value is string))
            {
               Value = Convert.ChangeType(value, context.PropertyDescriptor.PropertyType).ToString();
            }
            else
               Value = value as string;
            double decVal;
            if (!double.TryParse(Value,NumberStyles.Any, CultureInfo.InvariantCulture, out decVal))
               decVal = 1;
            MinMaxAttribute attr = (MinMaxAttribute)context.PropertyDescriptor.Attributes[typeof(MinMaxAttribute)];
            if (attr != null)
            {
               decVal = attr.PutInRange(decVal);
            }
            return Convert.ChangeType(decVal, context.PropertyDescriptor.PropertyType);
         }
         catch
         {
            return base.ConvertFrom(context, culture, value);
         }
      }

      public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
      {
         try
         {
            return destinationType == typeof(string) 
               ? Convert.ChangeType(value, context.PropertyDescriptor.PropertyType).ToString() 
               : Convert.ChangeType(value, destinationType);
         }
         catch { }
         return base.ConvertTo(context, culture, value, destinationType);
      }
   }

   // ReSharper disable MemberCanBePrivate.Global
   /// <summary>
   /// Attribute to allow ranges to be added to the numeric updowner
   /// </summary>
   [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
   public class MinMaxAttribute : Attribute
   {
      public double Min { get; private set; }
      public double Max { get; private set; }
      public double Increment { get; private set; }
      public int DecimalPlaces { get; private set; }

      /// <summary>
      /// Use to make a simple UInt16 max. Starts at 0, increment = 1
      /// </summary>
      /// <param name="max"></param>
      public MinMaxAttribute(UInt16 max)
          : this((double)UInt16.MinValue, max)
      {
      }

      /// <summary>
      /// Use to make a simple integer (or default conversion) based range.
      /// default inclrement is 1
      /// </summary>
      /// <param name="min"></param>
      /// <param name="max"></param>
      /// <param name="increment"></param>
      public MinMaxAttribute(int min, int max, int increment = 1)
          : this((double)min, max, increment)
      {
      }

      /// <summary>
      /// Set the Min, Max, increment, and decimal places to be used.
      /// </summary>
      /// <param name="min"></param>
      /// <param name="max"></param>
      /// <param name="increment"></param>
      /// <param name="decimalPlaces"></param>
      public MinMaxAttribute(double min, double max, double increment = 1, int decimalPlaces = 0)
      {
         Min = min;
         Max = max;
         Increment = increment;
         DecimalPlaces = decimalPlaces;
      }

      /// <summary>
      /// Validation function to check if the value is withtin the range (inclusive)
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public bool IsInRange(object value)
      {
          double checkedValue = (double)Convert.ChangeType(value, typeof(double));
         return ((checkedValue <= Max)
            && (checkedValue >= Min)
            );
      }

      /// <summary>
      /// Takes the value and adjusts if it is out of bounds.
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public double PutInRange(object value)
      {
          double checkedValue = (double)Convert.ChangeType(value, typeof(double));
         if (checkedValue > Max)
            checkedValue = Max;
         else if (checkedValue < Min)
            checkedValue = Min;
         return checkedValue;
      }
   }
   // ReSharper restore MemberCanBePrivate.Global


   public class NumericUpDownTypeEditor : UITypeEditor
   {
      public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
      {
         if (context == null || context.Instance == null)
            return base.GetEditStyle(context);
         return context.PropertyDescriptor.IsReadOnly ? UITypeEditorEditStyle.None : UITypeEditorEditStyle.DropDown;
      }

      public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
      {
         try
         {
            if (context == null || context.Instance == null || provider == null)
               return value;
            
            //use IWindowsFormsEditorService object to display a control in the dropdown area  
            IWindowsFormsEditorService frmsvr = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (frmsvr == null)
               return value;

            MinMaxAttribute attr = (MinMaxAttribute)context.PropertyDescriptor.Attributes[typeof(MinMaxAttribute)];
            if (attr != null)
            {
               NumericUpDown nmr = new NumericUpDown
                                      {
                                         Size = new Size(60, 120),
                                         Minimum = (decimal)attr.Min,
                                         Maximum = (decimal)attr.Max,
                                         Increment = (decimal)attr.Increment,
                                         DecimalPlaces = attr.DecimalPlaces,
                                         Value = (decimal)attr.PutInRange(value)
                                      };
               frmsvr.DropDownControl(nmr);
               context.OnComponentChanged();
               return Convert.ChangeType(nmr.Value, context.PropertyDescriptor.PropertyType);
            }
         }
         catch { }
         return value;
      }
   }
}
