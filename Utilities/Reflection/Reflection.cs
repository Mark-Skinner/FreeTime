using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utilities.Reflection
{
    public static class Reflection
    {
        #region Properties

        #region GetProperty

        /// <summary>
        /// Gets the property with the specified name in
        /// the class of <paramref name="Instance"/>.
        /// </summary>
        /// <param name="Instance">An instance of an object with a property named <paramref name="PropertyName"/>.</param>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The property with the name <paramref name="PropertyName"/>.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="PropertyName"/> is an empty string or not specific enough.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="Instance"/> or <paramref name="PropertyName"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the property with name <paramref name="PropertyName"/> could not be found.
        /// </exception>
        public static PropertyInfo GetProperty(object Instance, string PropertyName)
        {
            if (Instance == null)
                throw new ArgumentNullException("Instance", "The provided instance was null.");
            if (PropertyName == null)
                throw new ArgumentNullException("PropertyName", "The provided property name was null.");
            if (PropertyName.Equals(string.Empty))
                throw new ArgumentException("The provided property name was an empty string.", "PropertyName");
            try
            {
                PropertyInfo property = Instance.GetType().GetProperty(PropertyName);
                if (property == null)
                    throw new ArgumentOutOfRangeException("PropertyName", PropertyName, "The provided property name does not exist. See the ParamName and ActualValue properties for more details");
                return property;
            }
            catch (AmbiguousMatchException amex)
            {
                throw new ArgumentException("The provided PropertyName was not specific enough.", "PropertyName", amex);
            }
        }

        /// <summary>
        /// Gets the property with the name <paramref name="PropertyName"/>
        /// in the class <typeparamref name="Class"/>.
        /// </summary>
        /// <typeparam name="Class">The class to search for the property.</typeparam>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The property with the name <paramref name="PropertyName"/>.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="PropertyName"/> is an empty string or not specific enough.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="PropertyName"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the property with name <paramref name="PropertyName"/> could not be found.
        /// </exception>
        public static PropertyInfo GetProperty<Class>(string PropertyName)
        {
            if (PropertyName == null)
                throw new ArgumentNullException("PropertyName", "The provided property name was null.");
            if (PropertyName.Equals(string.Empty))
                throw new ArgumentException("The provided property name was an empty string.", "PropertyName");
            try
            {
                PropertyInfo property = typeof(Class).GetProperty(PropertyName);
                if (property == null)
                    throw new ArgumentOutOfRangeException("PropertyName", PropertyName, "The provided property name does not exist. See the ParamName and ActualValue properties for more details");
                return property;
            }
            catch (AmbiguousMatchException amex)
            {
                throw new ArgumentException("The provided PropertyName was not specific enough.", "PropertyName", amex);
            }
        }

        /// <summary>
        /// Tries to get the property with the specified name in
        /// the class of <paramref name="Instance"/>.
        /// </summary>
        /// <param name="Instance">An instance of an object with a property named <paramref name="PropertyName"/>.</param>
        /// <param name="PropertyName">The name of the property.</param>
        /// <param name="PropertyInfo">The property information that will be populated.</param>
        /// <returns>True if the property with name <paramref name="PropertyName"/> is found, false otherwise.</returns>
        public static bool TryGetProperty(object Instance, string PropertyName, out PropertyInfo PropertyInfo)
        {
            PropertyInfo = default(PropertyInfo);
            if (Instance == null || string.IsNullOrEmpty(PropertyName))
                return false;
            try
            {
                PropertyInfo = Instance.GetType().GetProperty(PropertyName);
                if (PropertyInfo == null)
                    return false;
                return true;
            }
            catch (AmbiguousMatchException)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to get the property with the name <paramref name="PropertyName"/>
        /// in the class <typeparamref name="Class"/>.
        /// </summary>
        /// <typeparam name="Class">The class to search for the property.</typeparam>
        /// <param name="PropertyName">The name of the property.</param>
        /// <param name="PropertyInfo">The property information that will be populated.</param>
        /// <returns>True if the property with the name <paramref name="PropertyName"/> is found, false otherwise.</returns>
        public static bool TryGetProperty<Class>(string PropertyName, out PropertyInfo PropertyInfo)
        {
            PropertyInfo = default(PropertyInfo);
            if (string.IsNullOrEmpty(PropertyName))
                return false;
            try
            {
                PropertyInfo = typeof(Class).GetProperty(PropertyName);
                if (PropertyInfo == null)
                    return false;
                return true;
            }
            catch (AmbiguousMatchException)
            {
                return false;
            }
        }

        #endregion

        #region GetProperties

        /// <summary>
        /// Gets the properties of the provided <paramref name="Instance"/>.
        /// </summary>
        /// <param name="Instance">An instance of an object.</param>
        /// <returns>The properties of the provided <paramref name="Instance"/>.</returns>
        /// <exception cref="ArgumentNullException">Throw if provided instance is null.</exception>
        public static PropertyInfo[] GetProperties(object Instance)
        {
            if (Instance == null)
                throw new ArgumentNullException("Instance","The provided instance was null.");
            return Instance.GetType().GetProperties();
        }

        /// <summary>
        /// Gets the properties of the class <typeparamref name="Class"/>.
        /// </summary>
        /// <typeparam name="Class">The class to search for the property.</typeparam>
        /// <returns>The properties of the class <typeparamref name="Class"/>.</returns>
        public static PropertyInfo[] GetProperties<Class>()
        {
            return typeof(Class).GetProperties();
        }

        /// <summary>
        /// Tries to get the properties of the provided <paramref name="Instance"/>.
        /// </summary>
        /// <param name="Instance">An instance of an object.</param>
        /// <param name="Properties">The array to populate with the properties of the provided <paramref name="Instance"/>.</param>
        /// <returns>True if the properties of the provided <paramref name="Instance"/> are found, false otherwise.</returns>
        public static bool TryGetProperties(object Instance, out PropertyInfo[] Properties)
        {
            Properties = new PropertyInfo[0];
            if (Instance == null)
                return false;
            Properties = Instance.GetType().GetProperties();
            return true;
        }

        #endregion

        #region GetPropertyValue

        /// <summary>
        /// Gets the value of the property with the name <paramref name="PropertyName"/>
        /// in the class of <paramref name="Instance"/>.
        /// </summary>
        /// <param name="Instance">An instance of an object with a property named <paramref name="PropertyName"/>.</param>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The value of the property with the name <paramref name="PropertyName"/> in the class of <paramref name="Instance"/>.</returns>
        /// <exception cref="ArgumentException">Thrown by <see cref="GetProperty(object, string)"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown by <see cref="GetProperty(object, string)"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown by <see cref="GetProperty(object, string)"/>.</exception>
        public static object GetPropertyValue(object Instance, string PropertyName)
        {
            return GetPropertyValue(Instance, GetProperty(Instance, PropertyName));
        }

        /// <summary>
        /// Gets the value of the property <paramref name="Property"/>
        /// in the class of <paramref name="Instance"/>.
        /// </summary>
        /// <param name="Instance">An instance of an object with a property <paramref name="Property"/>.</param>
        /// <param name="Property">The property that you want the vaue of.</param>
        /// <returns>The value of the property <paramref name="Property"/> in the class of <paramref name="Instance"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="Instance"/> or <paramref name="Property"/> is null.
        /// </exception>
        public static object GetPropertyValue(object Instance, PropertyInfo Property)
        {
            if (Instance == null)
                throw new ArgumentNullException("Instance", "The provided instance was null.");
            if (Property == null)
                throw new ArgumentNullException("Property", "The provided property was null.");
            return Property.GetValue(Instance);
        }

        /// <summary>
        /// Gets the value of the property with the name <paramref name="PropertyName"/>
        /// in the class of <typeparamref name="Class"/>. This may be used for static classes
        /// because is it impossible to have an instance of a static class. If the class is not
        /// static, it is recommended to use method <see cref="GetPropertyValue(object, string)"/>.
        /// </summary>
        /// <typeparam name="Class">The class that contains the property with the name <paramref name="PropertyName"/>.</typeparam>
        /// <param name="PropertyName">The name of the property.</param>
        /// <returns>The value of the property with the name <paramref name="PropertyName"/> in the class of <typeparamref name="Class"/>.</returns>
        /// <exception cref="ArgumentException">Thrown by <see cref="GetPropertyValue(object, string)"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown by <see cref="GetPropertyValue(object, string)"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown by <see cref="GetPropertyValue(object, string)"/>.</exception>
        public static object GetPropertyValue<Class>(string PropertyName)
        {
            return GetPropertyValue(typeof(Class), PropertyName);
        }

        /// <summary>
        /// Gets the value of the property <paramref name="Property"/>
        /// in the class of <typeparamref name="Class"/>. This may be used for static classes
        /// because is it impossible to have an instance of a static class. If the class is not
        /// static, it is recommended to use method <see cref="GetPropertyValue(object, PropertyInfo)"/>.
        /// </summary>
        /// <typeparam name="Class">The class that contains the property <paramref name="Property"/>.</typeparam>
        /// <param name="Property">The property that you want the vaue of.</param>
        /// <returns>The value of the property <paramref name="Property"/> in the class of <typeparamref name="Class"/>.</returns>
        /// <exception cref="ArgumentException">Thrown by <see cref="GetPropertyValue(object, PropertyInfo)"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown by <see cref="GetPropertyValue(object, PropertyInfo)"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown by <see cref="GetPropertyValue(object, PropertyInfo)"/>.</exception>
        public static object GetPropertyValue<Class>(PropertyInfo Property)
        {
            return GetPropertyValue(typeof(Class), Property);
        }

        /// <summary>
        /// Tries to get the value of the property with the name <paramref name="PropertyName"/>
        /// in the class of <paramref name="Instance"/>.
        /// </summary>
        /// <param name="Instance">An instance of an object with a property named <paramref name="PropertyName"/>.</param>
        /// <param name="PropertyName">The name of the property.</param>
        /// <param name="Value">The value of the property to be returned.</param>
        /// <returns>True if the value of the property with the name <paramref name="PropertyName"/> in the class of <paramref name="Instance"/> is found, false otherwise.</returns>
        public static bool TryGetPropertyValue(object Instance, string PropertyName, out object Value)
        {
            Value = default(object);
            PropertyInfo property;
            if (!TryGetProperty(Instance, PropertyName, out property))
                return false;
            return TryGetPropertyValue(Instance, property, out Value);
        }

        /// <summary>
        /// Tries to get the value of the property <paramref name="Property"/>
        /// in the class of <paramref name="Instance"/>.
        /// </summary>
        /// <param name="Instance">An instance of an object with a property <paramref name="Property"/>.</param>
        /// <param name="Property">The property that you want the vaue of.</param>
        /// <param name="Value">The value of the property to be returned.</param>
        /// <returns>True if the value of the property <paramref name="Property"/> in the class of <paramref name="Instance"/> is found, false otherwise.</returns>
        public static bool TryGetPropertyValue(object Instance, PropertyInfo Property, out object Value)
        {
            Value = default(object);
            if (Instance == null || Property == null)
                return false;
            Value = Property.GetValue(Instance);
            return true;
        }

        /// <summary>
        /// Tries to get the value of the property with the name <paramref name="PropertyName"/>
        /// in the class of <paramref name="Class"/>. This may be used for static classes
        /// because is it impossible to have an instance of a static class. If the class is not
        /// static, it is recommended to use method <see cref="TryGetPropertyValue(object, string, out object)"/>.
        /// </summary>
        /// <typeparam name="Class">The class that contains the property with the name <paramref name="PropertyName"/>.</typeparam>
        /// <param name="PropertyName">The name of the property.</param>
        /// <param name="Value">The value of the property to be returned.</param>
        /// <returns>True if the value of the property with the name <paramref name="PropertyName"/> in the class of <paramref name="Class"/> is found, false otherwise.</returns>
        public static bool TryGetPropertyValue<Class>(string PropertyName, out object Value)
        {
            return TryGetPropertyValue(typeof(Class), PropertyName, out Value);
        }
        
        /// <summary>
        /// Tries to get the value of the property <paramref name="Property"/>
        /// in the class of <typeparamref name="Class"/>. This may be used for static classes
        /// because is it impossible to have an instance of a static class. If the class is not
        /// static, it is recommended to use method <see cref="TryGetPropertyValue(object, PropertyInfo, out object)"/>.
        /// </summary>
        /// <typeparam name="Class">The class that contains the property <paramref name="Property"/>.</typeparam>
        /// <param name="Property">The property that you want the vaue of.</param>
        /// <param name="Value">The value of the property to be returned.</param>
        /// <returns>The value of the property <paramref name="Property"/> in the class of <typeparamref name="Class"/>.</returns>
        public static bool TryGetPropertyValue<Class>(PropertyInfo Property, out object Value)
        {
            return TryGetPropertyValue(typeof(Class), Property, out Value);
        }

        #endregion

        #endregion

        #region Attributes

        #region GetAttribute

        /// <summary>
        /// Gets the attribute of type <paramref name="AttributeType"/> applied to the specified
        /// property with the name <paramref name="PropertyName"/> in the class of the supplied <paramref name="Instance"/>.
        /// </summary>
        /// <param name="Instance">An object with the specified property that has the attributes.</param>
        /// <param name="PropertyName">The name of the property that contains the attribute.</param>
        /// <param name="AttributeType">The type of the attribute to find.</param>
        /// <returns>A <see cref="Attribute"/> object of type <typeparamref name="Attribute"/>.</returns>
        /// <exception cref="ArgumentException">Thrown by <see cref="GetAttribute(PropertyInfo, Type)"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown by <see cref="GetAttribute(PropertyInfo, Type)"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown by <see cref="GetProperty(object, string)"/>.</exception>
        public static Attribute GetAttribute(object Instance, string PropertyName, Type AttributeType)
        {
            return GetAttribute(GetProperty(Instance, PropertyName), AttributeType);
        }

        /// <summary>
        /// Gets the attribute of type <paramref name="AttributeType"/> applied to the specified
        /// property <paramref name="Property"/>.
        /// </summary>
        /// <param name="Property">The property that contains the attribute.</param>
        /// <param name="AttributeType">The type of the attribute to find.</param>
        /// <returns>A <see cref="Attribute"/> object of type <paramref name="AttributeType"/>.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="AttributeType"/> is not derived from <see cref="Attribute"/>,
        /// it does not exist on the property, it could not be cast as the provided type, it could
        /// not be loaded, or it was  an ambiguous type.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="Property"/> or <paramref name="AttributeType"/> is null.
        /// </exception>
        public static Attribute GetAttribute(PropertyInfo Property, Type AttributeType)
        {
            if (Property == null)
                throw new ArgumentNullException("Property", "The provided property was null.");
            if (AttributeType == null)
                throw new ArgumentNullException("AttributeType", "The provided attribute type was null.");
            if (!AttributeType.IsSubclassOf(typeof(Attribute)))
                throw new ArgumentException("The provided attribute type is not derived from class Attribute.", "AttributeType");
            try
            {
                Attribute attribute = Property.GetCustomAttribute(AttributeType) as Attribute;
                if (attribute == null)
                    throw new ArgumentException("The provided attribute type could not be found on the provided property.", "AttributeType");
                return attribute;
            }
            catch (AmbiguousMatchException amex)
            {
                throw new ArgumentException("Multiple attributes were found for provided property and attribute type.", amex);
            }
            catch (TypeLoadException tlex)
            {
                throw new ArgumentException("The custom attribute type could not be loaded.", "AttributeType", tlex);
            }
        }

        /// <summary>
        /// Gets the attribute of type <typeparamref name="Attribute"/> applied to the specified
        /// property with the name <paramref name="PropertyName"/> in the class of the supplied <paramref name="Instance"/>.
        /// </summary>
        /// <typeparam name="Attribute">The Attribute to find and return.</typeparam>
        /// <param name="Instance">An object with the specified property that has the attributes.</param>
        /// <param name="PropertyName">The name of the property that contains the attribute.</param>
        /// <returns>A <see cref="Attribute"/> object of type <typeparamref name="Attribute"/>.</returns>
        /// <exception cref="ArgumentException">Thrown by <see cref="GetAttribute(object, string, Type)"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown by <see cref="GetAttribute(object, string, Type)"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown by <see cref="GetAttribute(object, string, Type)"/>.</exception>
        public static System.Attribute GetAttribute<Attribute>(object Instance, string PropertyName)
        {
            return GetAttribute(Instance, PropertyName, typeof(Attribute));
        }

        /// <summary>
        /// Gets the attribute of type <typeparamref name="Attribute"/> applied to the specified
        /// property <paramref name="Property"/>.
        /// </summary>
        /// <typeparam name="Attribute">The Attribute to find and return.</typeparam>
        /// <param name="Property">The property that contains the attribute.</param>
        /// <returns>A <see cref="Attribute"/> object of type <typeparamref name="Attribute"/>.</returns>
        /// <exception cref="ArgumentException">Thrown by <see cref="GetAttribute(PropertyInfo, Type)"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown by <see cref="GetAttribute(PropertyInfo, Type)"/>.</exception>
        public static System.Attribute GetAttribute<Attribute>(PropertyInfo Property)
        {
            return GetAttribute(Property, typeof(Attribute));
        }

        /// <summary>
        /// Gets the attribute of type <typeparamref name="Attribute"/> applied to the specified
        /// property with the name <paramref name="PropertyName"/> in the class <typeparamref name="Class"/>.
        /// </summary>
        /// <typeparam name="Class">The name of the class to search.</typeparam>
        /// <typeparam name="Attribute">The Attribute to find and return.</typeparam>
        /// <param name="PropertyName">The name of the property that contains the attribute.</param>
        /// <returns>An Attribute of type Attribute.</returns>
        /// <exception cref="ArgumentException">Thrown by <see cref="GetAttribute(object, string, Type)"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown by <see cref="GetAttribute(object, string, Type)"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown by <see cref="GetAttribute(object, string, Type)"/>.</exception>
        public static System.Attribute GetAttribute<Class, Attribute>(string PropertyName)
        {
            return GetAttribute(typeof(Class), PropertyName, typeof(Attribute));
        }

        /// <summary>
        /// Tries to get the attribute of type <paramref name="AttributeType"/> applied to the specified
        /// property with the name <paramref name="PropertyName"/> in the class of the supplied <paramref name="Instance"/>.
        /// </summary>
        /// <param name="Instance">An object with the specified property that has the attributes.</param>
        /// <param name="PropertyName">The name of the property that contains the attribute.</param>
        /// <param name="AttributeType">The type of the attribute to find.</param>
        /// <param name="CustomAttribute">The attribute to be populated.</param>
        /// <returns>True if a <see cref="Attribute"/> object of type <paramref name="AttributeType"/> is found, false otherwise.</returns>
        public static bool TryGetAttribute(object Instance, string PropertyName, Type AttributeType, out Attribute CustomAttribute)
        {
            CustomAttribute = default(Attribute);
            PropertyInfo Property;
            if (!TryGetProperty(Instance, PropertyName, out Property))
                return false;
            return TryGetAttribute(Property, AttributeType, out CustomAttribute);
        }

        /// <summary>
        /// Tries to get the attribute of type <paramref name="AttributeType"/> applied to the specified
        /// property <paramref name="Property"/>.
        /// </summary>
        /// <param name="Property">The property that contains the attribute.</param>
        /// <param name="AttributeType">The type of the attribute to find.</param>
        /// <param name="CustomAttribute">The attribute to be populated.</param>
        /// <returns>True if a <see cref="Attribute"/> object of type <paramref name="AttributeType"/> is found, false otherwise.</returns>
        public static bool TryGetAttribute(PropertyInfo Property, Type AttributeType, out Attribute CustomAttribute)
        {
            CustomAttribute = default(Attribute);
            if (Property == null || AttributeType == null)
                return false;
            if (!AttributeType.IsSubclassOf(typeof(Attribute)))
                return false;
            try
            {
                CustomAttribute = Property.GetCustomAttribute(AttributeType) as Attribute;
                if (CustomAttribute == null)
                    return false;
                return true;
            }
            catch (AmbiguousMatchException)
            {
                return false;
            }
            catch (TypeLoadException)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to get the attribute of type <typeparamref name="Attribute"/> applied to the specified
        /// property with the name <paramref name="PropertyName"/> in the class of the supplied <paramref name="Instance"/>.
        /// </summary>
        /// <typeparam name="Attribute">The Attribute to find and return.</typeparam>
        /// <param name="Instance">An object with the specified property that has the attributes.</param>
        /// <param name="PropertyName">The name of the property that contains the attribute.</param>
        /// <param name="CustomAttribute">The attribute to be populated.</param>
        /// <returns>True if a <see cref="Attribute"/> object of type <typeparamref name="Attribute"/> is found, false otherwise.</returns>
        public static bool TryGetAttribute<Attribute>(object Instance, string PropertyName, out System.Attribute CustomAttribute)
        {
            return TryGetAttribute(Instance, PropertyName, typeof(Attribute), out CustomAttribute);
        }

        /// <summary>
        /// Tries to get the attribute of type <typeparamref name="Attribute"/> applied to the specified
        /// property <paramref name="Property"/>.
        /// </summary>
        /// <typeparam name="Attribute">The Attribute to find and return.</typeparam>
        /// <param name="Property">The property that contains the attribute.</param>
        /// <param name="CustomAttribute">The attribute to be populated.</param>
        /// <returns>True if a <see cref="Attribute"/> object of type <typeparamref name="Attribute"/> is found, false otherwise.</returns>
        public static bool TryGetAttribute<Attribute>(PropertyInfo Property, out System.Attribute CustomAttribute)
        {
            return TryGetAttribute(Property, typeof(Attribute), out CustomAttribute);
        }

        /// <summary>
        /// Tries to get the attribute of type <typeparamref name="Attribute"/> applied to the specified
        /// property with the name <paramref name="PropertyName"/> in the class <typeparamref name="Class"/>.
        /// </summary>
        /// <typeparam name="Class">The name of the class to search.</typeparam>
        /// <typeparam name="Attribute">The Attribute to find and return.</typeparam>
        /// <param name="PropertyName">The name of the property that contains the attribute.</param>
        /// <param name="CustomAttribute">The attribute to be populated.</param>
        /// <returns>True if a <see cref="Attribute"/> object of type <typeparamref name="Attribute"/> is found, false otherwise.</returns>
        public static bool TryGetAttribute<Class, Attribute>(string PropertyName, out System.Attribute CustomAttribute)
        {
            return TryGetAttribute(typeof(Class), PropertyName, typeof(Attribute), out CustomAttribute);
        }

        #endregion

        #region GetAttributes

        /// <summary>
        /// Gets all of the custom attributes applied to a property
        /// of the provided Instance.
        /// </summary>
        /// <param name="Instance">An object with the specified property that has the attributes.</param>
        /// <param name="PropertyName">The name of the property that contains the attribute.</param>
        /// <returns>An array of all custom attributes applied to the property.</returns>
        /// <exception cref="ArgumentException">Thrown by <see cref="GetAttributes(PropertyInfo)"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown by <see cref="GetAttributes(PropertyInfo)"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown by <see cref="GetProperty(object, string)"/>.</exception>
        public static object[] GetAttributes(object Instance, string PropertyName)
        {
            return GetAttributes(GetProperty(Instance, PropertyName));
        }

        /// <summary>
        /// Gets all of the custom attributes applied to the property
        /// <paramref name="Property"/>.
        /// </summary>
        /// <param name="Property">The property that contains the attribute.</param>
        /// <returns>An array of all custom attributes applied to the property.</returns>
        /// <exception cref="ArgumentException">Thrown if the provided <paramref name="Property"/> could not be loaded.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the provided <paramref name="Property"/> is null.</exception>
        public static object[] GetAttributes(PropertyInfo Property)
        {
            if (Property == null)
                throw new ArgumentNullException("Property", "The provided property was null.");
            try
            {
                return Property.GetCustomAttributes(true);
            }
            catch (InvalidOperationException ioex)
            {
                throw new ArgumentException("The provided property has an attribute that could not be loaded.", "Property", ioex);
            }
            catch (TypeLoadException tlex)
            {
                throw new ArgumentException("The provided property has an attribute that could not be loaded.", "Property", tlex);
            }
        }

        /// <summary>
        /// Gets all of the custom attributes applied to a property
        /// of the provided Instance that are of type Attribute.
        /// </summary>
        /// <typeparam name="Attribute">The Attribute to find and return.</typeparam>
        /// <param name="Instance">An object with the specified property that has the attributes.</param>
        /// <param name="PropertyName">The name of the property that contains the attribute.</param>
        /// <returns>An array of all custom attributes applied to the property of type Attribute.</returns>
        /// <exception cref="ArgumentException">Thrown by <see cref="GetAttributes{Attribute}(PropertyInfo)"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown by <see cref="GetAttributes{Attribute}(PropertyInfo)"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown by <see cref="GetProperty(object, string)"/>.</exception>
        public static List<Attribute> GetAttributes<Attribute>(object Instance, string PropertyName) where Attribute : System.Attribute
        {
            return GetAttributes<Attribute>(GetProperty(Instance, PropertyName));
        }

        /// <summary>
        /// Gets all of the custom attributes applied to a property
        /// that are of type Attribute.
        /// </summary>
        /// <typeparam name="Attribute">The Attribute to find and return.</typeparam>
        /// <param name="Property">The property that contains the attribute.</param>
        /// <returns>An array of all custom attributes applied to the property of type Attribute.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the provided <paramref name="Property"/> could not be loaded, or method
        /// <see cref="CustomAttributeExtensions.GetCustomAttributes(MemberInfo, bool)"/> returned null.
        /// </exception>
        /// <exception cref="ArgumentNullException">Thrown if the provided <paramref name="Property"/> is null.</exception>
        public static List<Attribute> GetAttributes<Attribute>(PropertyInfo Property) where Attribute : System.Attribute
        {
            if (Property == null)
                throw new ArgumentNullException("Property", "The provided property was null.");
            try
            {
                IEnumerable<Attribute> attributes = Property.GetCustomAttributes<Attribute>(true);
                if (attributes == null)
                    throw new ArgumentException("The provided property does not have any custom attributes.", "Property");
                return new List<Attribute>(attributes);
            }
            catch (NotSupportedException nsex)
            {
                //Property is not a constructor, method, property, event, type, or field. 
                throw new ArgumentException("The provided property was not the correct type. It must be a constructor, method, property, event, type, or field.", "Property", nsex);
            }
            catch (TypeLoadException tlex)
            {
                throw new ArgumentException("The provided property has an attribute that could not be loaded.", "Property", tlex);
            }
        }

        /// <summary>
        /// Gets all of the custom attributes applied to a property
        /// of the provided Class that are of type Attribute.
        /// </summary>
        /// <typeparam name="Class">The name of the class to search.</typeparam>
        /// <typeparam name="Attribute">The Attribute to find and return.</typeparam>
        /// <param name="PropertyName">The name of the property that contains the attribute.</param>
        /// <returns>An array of all custom attributes applied to the property of type Attribute.</returns>
        /// <exception cref="ArgumentException">Thrown by <see cref="GetAttributes{Attribute}(PropertyInfo)"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown by <see cref="GetAttributes{Attribute}(PropertyInfo)"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown by <see cref="GetProperty{Class}(string)"/>.</exception>
        public static List<Attribute> GetAttributes<Class, Attribute>(string PropertyName) where Attribute : System.Attribute
        {
            return GetAttributes<Attribute>(GetProperty<Class>(PropertyName));
        }

        /// <summary>
        /// Tries to get all of the custom attributes applied to a property
        /// of the provided Instance.
        /// </summary>
        /// <param name="Instance">An object with the specified property that has the attributes.</param>
        /// <param name="PropertyName">The name of the property that contains the attribute.</param>
        /// <param name="Attributes">The array of attributes to populate.</param>
        /// <returns>True if an array of all custom attributes applied to the property is found, false otherwise.</returns>
        public static bool TryGetAttributes(object Instance, string PropertyName, out object[] Attributes)
        {
            Attributes = new object[0];
            PropertyInfo Property;
            if (!TryGetProperty(Instance, PropertyName, out Property))
                return false;
            return TryGetAttributes(Property, out Attributes);
        }

        /// <summary>
        /// Tries to get all of the custom attributes applied to the property
        /// <paramref name="Property"/>.
        /// </summary>
        /// <param name="Property">The property that contains the attribute.</param>
        /// <param name="Attributes">The array of attributes to populate.</param>
        /// <returns>True if an array of all custom attributes applied to the property is found, false otherwise.</returns>
        public static bool TryGetAttributes(PropertyInfo Property, out object[] Attributes)
        {
            Attributes = new object[0];
            if (Property == null)
                return false;
            try
            {
                Attributes = Property.GetCustomAttributes(true);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (TypeLoadException)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to get all of the custom attributes applied to a property
        /// of the provided Instance that are of type Attribute.
        /// </summary>
        /// <typeparam name="Attribute">The Attribute to find and return.</typeparam>
        /// <param name="Instance">An object with the specified property that has the attributes.</param>
        /// <param name="PropertyName">The name of the property that contains the attribute.</param>
        /// <param name="Attributes">The array of attributes to populate.</param>
        /// <returns>True if an array of all custom attributes applied to the property of type Attribute is found, false otherwise.</returns>
        public static bool TryGetAttributes<Attribute>(object Instance, string PropertyName, out List<Attribute> Attributes) where Attribute : System.Attribute
        {
            Attributes = new List<Attribute>(0);
            PropertyInfo Property;
            if (!TryGetProperty(Instance, PropertyName, out Property))
                return false;
            return TryGetAttributes(Property, out Attributes);
        }

        /// <summary>
        /// Tries to get all of the custom attributes applied to a property
        /// that are of type Attribute.
        /// </summary>
        /// <typeparam name="Attribute">The Attribute to find and return.</typeparam>
        /// <param name="Property">The property that contains the attribute.</param>
        /// <param name="Attributes">The array of attributes to populate.</param>
        /// <returns>True if an array of all custom attributes applied to the property of type Attribute is found, false otherwise.</returns>
        public static bool TryGetAttributes<Attribute>(PropertyInfo Property, out List<Attribute> Attributes) where Attribute : System.Attribute
        {
            Attributes = new List<Attribute>(0);
            if (Property == null)
                return false;
            try
            {
                IEnumerable<Attribute> attributes = Property.GetCustomAttributes<Attribute>(true);
                if (attributes == null)
                    return false;
                Attributes = new List<Attribute>(attributes);
                return true;
            }
            catch (NotSupportedException)
            {
                //Property is not a constructor, method, property, event, type, or field. 
                return false;
            }
            catch (TypeLoadException)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to get all of the custom attributes applied to a property
        /// of the provided Class that are of type Attribute.
        /// </summary>
        /// <typeparam name="Class">The name of the class to search.</typeparam>
        /// <typeparam name="Attribute">The Attribute to find and return.</typeparam>
        /// <param name="PropertyName">The name of the property that contains the attribute.</param>
        /// <param name="Attributes">The array of attributes to populate.</param>
        /// <returns>True if an array of all custom attributes applied to the property of type Attribute is found, false otherwise.</returns>
        public static bool TryGetAttributes<Class, Attribute>(string PropertyName, out List<Attribute> Attributes) where Attribute : System.Attribute
        {
            Attributes = new List<Attribute>(0);
            PropertyInfo Property;
            if (!TryGetProperty<Class>(PropertyName, out Property))
                return false;
            return TryGetAttributes<Attribute>(Property, out Attributes);
        }

        #endregion

        #endregion
    }
}