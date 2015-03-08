#region COPYRIGHT
/****************************************************************************
 *  Copyright (c) 2015 Fabio Ferretti <https://plus.google.com/+FabioFerretti3D>                 *
 *  This file is part of Sardauscan.                                        *
 *                                                                          *
 *  Sardauscan is free software: you can redistribute it and/or modify      *
 *  it under the terms of the GNU General Public License as published by    *
 *  the Free Software Foundation, either version 3 of the License, or       *
 *  (at your option) any later version.                                     *
 *                                                                          *
 *  Sardauscan is distributed in the hope that it will be useful,           *
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of          *
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the           *
 *  GNU General Public License for more details.                            *
 *                                                                          *
 *  You are not allowed to Sell in any form this code                       * 
 *  or any compiled version. This code is free and for free purpose only    *
 *                                                                          *
 *  You should have received a copy of the GNU General Public License       *
 *  along with Sardaukar.  If not, see <http://www.gnu.org/licenses/>       *
 ****************************************************************************
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
            float decVal;
            if (!float.TryParse(Value,NumberStyles.Any, CultureInfo.InvariantCulture, out decVal))
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
      public float Min { get; private set; }
      public float Max { get; private set; }
      public float Increment { get; private set; }
      public int DecimalPlaces { get; private set; }

      /// <summary>
      /// Use to make a simple UInt16 max. Starts at 0, increment = 1
      /// </summary>
      /// <param name="max"></param>
      public MinMaxAttribute(UInt16 max)
          : this((float)UInt16.MinValue, max)
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
          : this((float)min, max, increment)
      {
      }

      /// <summary>
      /// Set the Min, Max, increment, and decimal places to be used.
      /// </summary>
      /// <param name="min"></param>
      /// <param name="max"></param>
      /// <param name="increment"></param>
      /// <param name="decimalPlaces"></param>
      public MinMaxAttribute(float min, float max, float increment = 1, int decimalPlaces = 0)
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
          float checkedValue = (float)Convert.ChangeType(value, typeof(float));
         return ((checkedValue <= Max)
            && (checkedValue >= Min)
            );
      }

      /// <summary>
      /// Takes the value and adjusts if it is out of bounds.
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public float PutInRange(object value)
      {
          float checkedValue = (float)Convert.ChangeType(value, typeof(float));
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
