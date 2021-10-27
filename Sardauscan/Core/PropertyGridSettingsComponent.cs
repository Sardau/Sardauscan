#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Sardauscan.Core
{
	/// <summary>
	/// Class for helping with PropertyGrid properties edition
	/// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class PropertyGridSettingsComponent : Component, ICustomTypeDescriptor
        {

            //Private members
            private PropertyDescriptorCollection propertyCollection;
            //Max length will be used help control the layout of the Windows Form and PropertyGrid
            //or the table of controls in an ASP.Net page. The Form or Webpage layout will be adjusted
            //dynamically depending on length of these values.
            private int _maxLength;
            public int MaxLength
            {
                get
                {
                    return _maxLength;
                }
                set
                {
                    if (value > _maxLength)
                        _maxLength = value;
                }
            }

            /// <summary>
            /// Constructor of CustomClass which initializes the new PropertyDescriptorCollection.
            /// </summary>
            public PropertyGridSettingsComponent()
            {
                propertyCollection = new PropertyDescriptorCollection(new PropertyDescriptor[] { });
            }


            /// <summary>
            /// Adds a property into the CustomClass.
            /// </summary>
            /// <param name="propName">Name of the property that needs to be added.</param>
            /// <param name="propValue">Value of the property that needs to be added.</param>
            /// <param name="propDesc">Description of the property that needs to be added.</param>
            /// <param name="propCat">The category to display this property in.</param>
            /// <param name="isReadOnly">Sets the property value to readonly in the property grid.</param>
            /// <param name="isExpandable">Tells the property grid that this property is expandable.</param>
            /// <param name="propType">DataType of the property that needs to be added.</param>
            public void AddProperty(string propName, object propValue, string propDesc,
                string propCat, System.Type propType, bool isReadOnly, bool isExpandable)
            {
                DynamicProperty p = new DynamicProperty(propName, propValue, propDesc, propCat,
                    propType, isReadOnly, isExpandable);
                propertyCollection.Add(p);
                //Set our layout helper value.
                this.MaxLength = propName.Length;
                this.MaxLength = propValue.ToString().Length;
            }

            //Indexer for this class - returns a DynamicProperty by index position.
            public DynamicProperty this[int index]
            {
                get
                {
                    return (DynamicProperty)propertyCollection[index];
                }
            }

            //Overloaded Indexer for this class - returns a DynamicProperty by name.
            public DynamicProperty this[string name]
            {
                get
                {
                    return (DynamicProperty)propertyCollection[name];
                }
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            public string GetClassName()
            {
                return (TypeDescriptor.GetClassName(this, true));
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            public AttributeCollection GetAttributes()
            {
                return (TypeDescriptor.GetAttributes(this, true));
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            public string GetComponentName()
            {
                return (TypeDescriptor.GetComponentName(this, true));
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            public TypeConverter GetConverter()
            {
                return (TypeDescriptor.GetConverter(this, true));
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            public EventDescriptor GetDefaultEvent()
            {
                return (TypeDescriptor.GetDefaultEvent(this, true));
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            public PropertyDescriptor GetDefaultProperty()
            {
                PropertyDescriptorCollection props = GetAllProperties();

                if (props.Count > 0)
                    return (props[0]);
                else
                    return (null);
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="editorBaseType"></param>
            /// <returns></returns>
            public object GetEditor(Type editorBaseType)
            {
                return (TypeDescriptor.GetEditor(this, editorBaseType, true));
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="attributes"></param>
            /// <returns></returns>
            public EventDescriptorCollection GetEvents(Attribute[] attributes)
            {
                return (TypeDescriptor.GetEvents(this, attributes, true));
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            public EventDescriptorCollection GetEvents()
            {
                return (TypeDescriptor.GetEvents(this, true));
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="attributes"></param>
            /// <returns></returns>
            public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
            {
                return (GetAllProperties());
            }

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            public PropertyDescriptorCollection GetProperties()
            {
                return (GetAllProperties());
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="pd"></param>
            /// <returns></returns>
            public object GetPropertyOwner(PropertyDescriptor pd)
            {
                return (this);
            }


            /// <summary>
            ///	Helper method to return the PropertyDescriptorCollection or our Dynamic Properties
            /// </summary>
            /// <param name="pd"></param>
            /// <returns></returns>
            private PropertyDescriptorCollection GetAllProperties()
            {
                return propertyCollection;
            }



            /// <summary>
            ///	This is the Property class this will be dynamically added to the class at runtime.
            ///	These classes are returned in the PropertyDescriptorCollection of the GetAllProperties
            ///	method of the custom class.
            /// </summary>
            /// <param name="pd"></param>
            /// <returns></returns>
            public class DynamicProperty : PropertyDescriptor
            {
                private string propName;
                private object propValue;
                private string propDescription;
                private string propCategory;
                private Type propType;
                private bool isReadOnly;
                private bool isExpandable;

                public DynamicProperty(string pName, object pValue, string pDesc, string pCat, Type pType, bool readOnly, bool expandable)
                    : base(pName, new Attribute[] { })
                {
                    propName = pName;
                    propValue = pValue;
                    propDescription = pDesc;
                    propCategory = pCat;
                    propType = pType;
                    isReadOnly = readOnly;
                    isExpandable = expandable;
                }

                public override System.Type ComponentType
                {
                    get
                    {
                        return null;
                    }
                }

                public override string Category
                {
                    get
                    {
                        return propCategory;
                    }
                }

                public override bool IsReadOnly
                {
                    get
                    {
                        return isReadOnly;
                    }
                }

                public override System.Type PropertyType
                {
                    get
                    {
                        return propType;
                    }
                }

                public override bool CanResetValue(object component)
                {
                    return true;
                }

                public override object GetValue(object component)
                {
                    return propValue;
                }

                public override void SetValue(object component, object value)
                {
                    propValue = value;
                }

                public override void ResetValue(object component)
                {
                    propValue = null;
                }

                public override bool ShouldSerializeValue(object component)
                {
                    return false;
                }

                public override string Description
                {
                    get
                    {
                        return propDescription;
                    }
                }
            }
        }
}
