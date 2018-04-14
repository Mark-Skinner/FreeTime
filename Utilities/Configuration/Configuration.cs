using System;
using System.Configuration;

namespace Utilities.Configuration
{
    /// <summary>
    /// Supplies methods to retrieve configuration values from
    /// web configuration for a web application.
    /// </summary>
    public static class Configuration
    {
        #region Application Settings

        /// <summary>
        /// Gets the value of the application setting in the
        /// web configuration that has the provided <paramref name="Key"/>.
        /// </summary>
        /// <param name="Key">The key to look for in the web configuration.</param>
        /// <returns>
        /// The value of the application setting in the web
        /// configuration that has the provided <paramref name="Key"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="Key"/> is an empty string, or the value obtained with the key is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="Key"/> is null.
        /// </exception>
        /// <exception cref="ConfigurationErrorsException">
        /// Thrown if the application settings section in web configuration is configured incorrectly.
        /// </exception>
        public static object GetApplicationSetting(string Key)
        {
            if (Key == null)
                throw new ArgumentNullException("The provided key cannot be null.");
            if (Key.Equals(string.Empty))
                throw new ArgumentException("The provided key cannot be an empty string.");
            try
            {
                object Value = ConfigurationManager.AppSettings[Key];
                if (Value == null)
                    throw new ArgumentException(
                        string.Format("The key you are trying to get does not exist or has no value in the web configuration application settings. Key={0}",
                            Key));
                return Value;
            }
            catch (ConfigurationErrorsException ceex)
            {
                throw new ConfigurationErrorsException("The application settings section is not formatted correctly.", ceex);
            }
        }

        /// <summary>
        /// Tries to get the value of the application setting in the
        /// web configuration that has the provided <paramref name="Key"/>.
        /// </summary>
        /// <typeparam name="T">The type of the application setting value.</typeparam>
        /// <param name="Key">The key to look for in the web configuration.</param>
        /// <param name="Value">The value of the key if it exists.</param>
        /// <returns>
        /// True if the value of the application setting in the web
        /// configuration that has the provided <paramref name="Key"/> is found, false otherwise.
        /// </returns>
        public static bool TryGetApplicationSetting<T>(string Key, out T Value) where T : class
        {
            Value = default(T);
            if (string.IsNullOrEmpty(Key))
                return false;
            try
            {
                Value = ConfigurationManager.AppSettings[Key] as T;
                if (Value == null)
                    return false;
                return true;
            }
            catch (ConfigurationErrorsException)
            {
                return false;
            }
        }

        #endregion

        #region Configuration Sections

        /// <summary>
        /// Gets a configuration section with the provided <paramref name="SectionName"/>
        /// from the web configuration for the current web application.
        /// </summary>
        /// <param name="SectionName">The name of the configuration section.</param>
        /// <returns>
        /// A configuration section with the provided <paramref name="SectionName"/>
        /// from the web configuration for the current web application if it exists.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="SectionName"/> is an empty string.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="SectionName"/> is null.
        /// </exception>
        /// <exception cref="ConfigurationErrorsException">
        /// Thrown if the configuration section with the name <paramref name="SectionName"/>
        /// in the web configuration is configured incorrectly, or is not defined in the
        /// configSections section.
        /// </exception>
        public static ConfigurationSection GetConfigurationSection(string SectionName)
        {
            if (SectionName == null)
                throw new ArgumentNullException("The provided section name cannot be null.");
            if (SectionName.Equals(string.Empty))
                throw new ArgumentException("The provided section name cannot be an empty string.");

            ConfigurationSection Section = null;
            try
            {
                Section = ConfigurationManager.GetSection(SectionName) as ConfigurationSection;
            }
            catch (ConfigurationErrorsException ceex)
            {
                throw new ConfigurationErrorsException(
                    string.Format("The section you are trying to get is not formatted correctly. SectionName={0}.",
                        SectionName), ceex);
            }

            if (Section == null)
                throw new ConfigurationErrorsException(
                    string.Format("The section you are trying to get is not defined in the web configuration. If it is a custom configuration section, please make sure to list it in the <configSections> section. SectionName={0}",
                        SectionName));
            return Section;
        }

        /// <summary>
        /// Tries to get a configuration section with the provided <paramref name="SectionName"/>
        /// from the web configuration for the current web application.
        /// </summary>
        /// <typeparam name="T">The type of the configuration section.</typeparam>
        /// <param name="SectionName">The name of the configuration section.</param>
        /// <param name="Section">The section with the name <paramref name="SectionName"/> if it exists.</param>
        /// <returns>
        /// True if a configuration section with the provided <paramref name="SectionName"/>
        /// exists in the web configuration and may be cast as type <typeparamref name="T"/>,
        /// false otherwise.
        /// </returns>
        public static bool TryGetConfigurationSection<T>(string SectionName, out T Section) where T : ConfigurationSection
        {
            Section = default(T);
            if (string.IsNullOrEmpty(SectionName))
                return false;
            
            try
            {
                Section = ConfigurationManager.GetSection(SectionName) as T;
                if (Section == null)
                    return false;
                return true;
            }
            catch (ConfigurationErrorsException)
            {
                return false;
            }
        }

        #endregion

        #region Configuration Element Collections

        /// <summary>
        /// Gets the configuration element collection with the name
        /// <paramref name="CollectionName"/> in the web configuration
        /// section with the name <paramref name="SectionName"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ConfigurationHeaderElement"/> in the <seealso cref="ConfigurationElementCollection{T}"/>.</typeparam>
        /// <param name="SectionName">The name of the section that contains the <see cref="ConfigurationElementCollection{T}"/>.</param>
        /// <param name="CollectionName">The name of the <see cref="ConfigurationElementCollection{T}"/> to find.</param>
        /// <returns>
        /// The configuration element collection with the name
        /// <paramref name="CollectionName"/> in the web configuration
        /// section with the name <paramref name="SectionName"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="SectionName"/> or <paramref name="CollectionName"/> is an empty string.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="SectionName"/> or <paramref name="CollectionName"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by <see cref="GetConfigurationElementCollection{T}(ConfigurationSection, string)"/>.
        /// </exception>
        /// <exception cref="ConfigurationErrorsException">
        /// Thrown by <see cref="GetConfigurationSection(string)"/>.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// Thrown by <see cref="GetConfigurationElementCollection{T}(ConfigurationSection, string)"/>.
        /// </exception>
        public static ConfigurationElementCollection<T> GetConfigurationElementCollection<T>(string SectionName, string CollectionName) where T : ConfigurationHeaderElement, new()
        {
            if (CollectionName == null)
                throw new ArgumentNullException("The provided collection name cannot be null.");
            if (CollectionName.Equals(string.Empty))
                throw new ArgumentException("The provided collection name cannot be an empty string.");
            return GetConfigurationElementCollection<T>(GetConfigurationSection(SectionName), CollectionName);
        }

        /// <summary>
        /// Gets the configuration element collection with the name
        /// <paramref name="CollectionName"/> in the web configuration
        /// section <paramref name="Section"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ConfigurationHeaderElement"/> in the <seealso cref="ConfigurationElementCollection{T}"/>.</typeparam>
        /// <param name="Section">The section that contains the <see cref="ConfigurationElementCollection{T}"/>.</param>
        /// <param name="CollectionName">The name of the <see cref="ConfigurationElementCollection{T}"/> to find.</param>
        /// <returns>
        /// The configuration element collection with the name
        /// <paramref name="CollectionName"/> in the web configuration
        /// section <paramref name="Section"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="CollectionName"/> is an empty string.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="Section"/> or <paramref name="CollectionName"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the provided <paramref name="CollectionName"/> does not exist in the provided <paramref name="Section"/>.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// Thrown if the property with the name <paramref name="CollectionName"/> in section <paramref name="Section"/>
        /// could not be cast as a <seealso cref="ConfigurationElementCollection{T}"/>.
        /// </exception>
        public static ConfigurationElementCollection<T> GetConfigurationElementCollection<T>(ConfigurationSection Section, string CollectionName) where T : ConfigurationHeaderElement, new()
        {
            if (Section == null)
                throw new ArgumentNullException("The provided section cannot be null.");
            if (CollectionName == null)
                throw new ArgumentNullException("The provided collection name cannot be null.");
            if (CollectionName.Equals(string.Empty))
                throw new ArgumentException("The provided collection name cannot be an empty string.");
            
            foreach (PropertyInformation Property in Section.ElementInformation.Properties)
            {
                if (Property.Name.Equals(CollectionName))
                {
                    ConfigurationElementCollection<T> Collection = Property.Value as ConfigurationElementCollection<T>;
                    if (Collection == null)
                        throw new InvalidCastException(
                            string.Format("The property that has the same name as the provided collection name could not be cast as type {0}.",
                                typeof(ConfigurationElementCollection<T>)));
                    return Collection;
                }
            }

            throw new ArgumentOutOfRangeException("CollectionName", CollectionName, "The collection could not be found in the provided section. See ParamName and ActualValue for more details.");
        }

        /// <summary>
        /// Tries to get the configuration element collection with the name
        /// <paramref name="CollectionName"/> in the web configuration
        /// section with the name <paramref name="SectionName"/>.
        /// </summary>
        /// <typeparam name="CollectionType">The type of the <seealso cref="ConfigurationElementCollection{T}"/>.</typeparam>
        /// <param name="SectionName">The name of the section that contains the <see cref="ConfigurationElementCollection{T}"/>.</param>
        /// <param name="CollectionName">The name of the <see cref="ConfigurationElementCollection{T}"/> to find.</param>
        /// <param name="Collection">The <seealso cref="ConfigurationElementCollection{T}"/> with name <paramref name="CollectionName"/>.</param>
        /// <returns>
        /// True if the configuration element collection with the name
        /// <paramref name="CollectionName"/> in the web configuration
        /// section with the name <paramref name="SectionName"/> is found,
        /// false otherwise.
        /// </returns>
        public static bool TryGetConfigurationElementCollection<CollectionType>(string SectionName, string CollectionName, out CollectionType Collection) where CollectionType : ConfigurationElementCollection
        {
            Collection = default(CollectionType);
            if (string.IsNullOrEmpty(CollectionName))
                return false;
            ConfigurationSection Section;
            if (!TryGetConfigurationSection(SectionName, out Section))
                return false;
            return TryGetConfigurationElementCollection(Section, CollectionName, out Collection);
        }

        /// <summary>
        /// Tries to get the configuration element collection with the name
        /// <paramref name="CollectionName"/> in the web configuration
        /// section <paramref name="Section"/>.
        /// </summary>
        /// <typeparam name="CollectionType">The type of the <see cref="ConfigurationElementCollection{T}"/>.</typeparam>
        /// <param name="Section">The section that contains the <see cref="ConfigurationElementCollection{T}"/>.</param>
        /// <param name="CollectionName">The name of the <see cref="ConfigurationElementCollection{T}"/> to find.</param>
        /// <param name="Collection">The <seealso cref="ConfigurationElementCollection{T}"/> with name <paramref name="CollectionName"/>.</param>
        /// <returns>
        /// True if the configuration element collection with the name
        /// <paramref name="CollectionName"/> in the web configuration
        /// section <paramref name="Section"/> is found, false otherwise.
        /// </returns>
        public static bool TryGetConfigurationElementCollection<CollectionType>(ConfigurationSection Section, string CollectionName, out CollectionType Collection) where CollectionType : ConfigurationElementCollection
        {
            Collection = default(CollectionType);
            if (Section == null || string.IsNullOrEmpty(CollectionName))
                return false;

            foreach (PropertyInformation Property in Section.ElementInformation.Properties)
            {
                if (Property.Name.Equals(CollectionName))
                {
                    Collection = Property.Value as CollectionType;
                    if (Collection == null)
                        return false;
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Configuration Elements

        /// <summary>
        /// Gets a <see cref="ConfigurationHeaderElement"/> with name <paramref name="ElementName"/>
        /// from a <seealso cref="ConfigurationElementCollection{T}"/> with name <paramref name="CollectionName"/>
        /// that is contained in a configuration section with name <paramref name="SectionName"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ConfigurationHeaderElement"/> in the <seealso cref="ConfigurationElementCollection{T}"/>.</typeparam>
        /// <param name="SectionName">The name of the section that contains the <see cref="ConfigurationElementCollection{T}"/>.</param>
        /// <param name="CollectionName">The name of the <see cref="ConfigurationElementCollection{T}"/> that contains the <see cref="ConfigurationHeaderElement"/>.</param>
        /// <param name="ElementName">The name of the <see cref="ConfigurationHeaderElement"/> in the <seealso cref="ConfigurationElementCollection{T}"/>.</param>
        /// <returns>
        /// A <see cref="ConfigurationHeaderElement"/> with name <paramref name="ElementName"/>
        /// from a <seealso cref="ConfigurationElementCollection{T}"/> with name <paramref name="CollectionName"/>
        /// that is contained in a configuration section with name <paramref name="SectionName"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="SectionName"/>, <paramref name="CollectionName"/>, or <paramref name="ElementName"/> is an empty string.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="SectionName"/>, <paramref name="CollectionName"/>, or <paramref name="ElementName"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by <see cref="GetConfigurationElementCollection{T}(string, string)"/>.
        /// </exception>
        /// <exception cref="ConfigurationErrorsException">
        /// Thrown by <see cref="GetConfigurationElementCollection{T}(string, string)"/>.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// Thrown by <see cref="GetConfigurationElementCollection{T}(string, string)"/>.
        /// </exception>
        public static T GetConfigurationHeaderElement<T>(string SectionName, string CollectionName, string ElementName) where T : ConfigurationHeaderElement, new()
        {
            if (ElementName == null)
                throw new ArgumentNullException("The provided element name cannot be null.");
            if (ElementName.Equals(string.Empty))
                throw new ArgumentException("The provided element name cannot be an empty string.");
            return GetConfigurationHeaderElement(GetConfigurationElementCollection<T>(SectionName, CollectionName), ElementName);
        }

        /// <summary>
        /// Gets a <see cref="ConfigurationHeaderElement"/> with name <paramref name="ElementName"/>
        /// from a <seealso cref="ConfigurationElementCollection{T}"/> with name <paramref name="CollectionName"/>
        /// that is contained in a configuration section <paramref name="Section"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ConfigurationHeaderElement"/> in the <seealso cref="ConfigurationElementCollection{T}"/>.</typeparam>
        /// <param name="Section">The section that contains the <see cref="ConfigurationElementCollection{T}"/>.</param>
        /// <param name="CollectionName">The name of the <see cref="ConfigurationElementCollection{T}"/> that contains the <see cref="ConfigurationHeaderElement"/>.</param>
        /// <param name="ElementName">The name of the <see cref="ConfigurationHeaderElement"/> in the <seealso cref="ConfigurationElementCollection{T}"/>.</param>
        /// <returns>
        /// A <see cref="ConfigurationHeaderElement"/> with name <paramref name="ElementName"/>
        /// from a <seealso cref="ConfigurationElementCollection{T}"/> with name <paramref name="CollectionName"/>
        /// that is contained in a configuration section <paramref name="Section"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="CollectionName"/> or <paramref name="ElementName"/> is an empty string.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="Section"/>, <paramref name="CollectionName"/>, or <paramref name="ElementName"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by <see cref="GetConfigurationElementCollection{T}(ConfigurationSection, string)"/>.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// Thrown by <see cref="GetConfigurationElementCollection{T}(ConfigurationSection, string)"/>.
        /// </exception>
        public static T GetConfigurationHeaderElement<T>(ConfigurationSection Section, string CollectionName, string ElementName) where T : ConfigurationHeaderElement, new()
        {
            if (ElementName == null)
                throw new ArgumentNullException("The provided element name cannot be null.");
            if (ElementName.Equals(string.Empty))
                throw new ArgumentException("The provided element name cannot be an empty string.");
            return GetConfigurationHeaderElement(GetConfigurationElementCollection<T>(Section, CollectionName), ElementName);
        }

        /// <summary>
        /// Gets a <see cref="ConfigurationHeaderElement"/> with name <paramref name="ElementName"/>
        /// from the collection <paramref name="Collection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ConfigurationHeaderElement"/> in the <seealso cref="ConfigurationElementCollection{T}"/>.</typeparam>
        /// <param name="Collection">The <see cref="ConfigurationElementCollection{T}"/> that contains the <see cref="ConfigurationHeaderElement"/>.</param>
        /// <param name="ElementName">The name of the <see cref="ConfigurationHeaderElement"/> in the <seealso cref="ConfigurationElementCollection{T}"/>.</param>
        /// <returns>
        /// A <see cref="ConfigurationHeaderElement"/> with name <paramref name="ElementName"/>
        /// from the collection <paramref name="Collection"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="ElementName"/> is an empty string.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="Collection"/> or <paramref name="ElementName"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the provided <paramref name="ElementName"/> does not exist in the provided <paramref name="Collection"/>.
        /// </exception>
        public static T GetConfigurationHeaderElement<T>(ConfigurationElementCollection<T> Collection, string ElementName) where T : ConfigurationHeaderElement, new()
        {
            if (Collection == null)
                throw new ArgumentNullException("The provided collection cannot be null.");
            if (ElementName == null)
                throw new ArgumentNullException("The provided element name cannot be null.");
            if (ElementName.Equals(string.Empty))
                throw new ArgumentException("The provided element name cannot be an empty string.");
            foreach (T Element in Collection)
                if (Element.Name.Equals(ElementName))
                    return Element;

            throw new ArgumentOutOfRangeException("ElementName", ElementName, "The element could not be found in the provided collection. See the ParamName and ActualValue properties for more details.");
        }

        /// <summary>
        /// Tries to get a <see cref="ConfigurationHeaderElement"/> with name <paramref name="ElementName"/>
        /// from a <seealso cref="ConfigurationElementCollection{T}"/> with name <paramref name="CollectionName"/>
        /// that is contained in a configuration section with name <paramref name="SectionName"/>.
        /// </summary>
        /// <typeparam name="ElementType">The type of the <see cref="ConfigurationHeaderElement"/> in the <seealso cref="ConfigurationElementCollection{T}"/>.</typeparam>
        /// <param name="SectionName">The name of the section that contains the <see cref="ConfigurationElementCollection{T}"/>.</param>
        /// <param name="CollectionName">The name of the <see cref="ConfigurationElementCollection{T}"/> that contains the <see cref="ConfigurationHeaderElement"/>.</param>
        /// <param name="ElementName">The name of the <see cref="ConfigurationHeaderElement"/> in the <seealso cref="ConfigurationElementCollection{T}"/>.</param>
        /// <param name="Element">The <see cref="ConfigurationHeaderElement"/> with name <paramref name="ElementName"/>.</param>
        /// <returns>
        /// True if a <see cref="ConfigurationHeaderElement"/> with name <paramref name="ElementName"/>
        /// from a <seealso cref="ConfigurationElementCollection{T}"/> with name <paramref name="CollectionName"/>
        /// that is contained in a configuration section with name <paramref name="SectionName"/> is
        /// found, false otherwise.
        /// </returns>
        public static bool TryGetConfigurationHeaderElement<ElementType>(string SectionName, string CollectionName, string ElementName, out ElementType Element) where ElementType : ConfigurationHeaderElement, new()
        {
            Element = default(ElementType);
            if (string.IsNullOrEmpty(ElementName))
                return false;

            ConfigurationElementCollection<ElementType> Collection;
            if (!TryGetConfigurationElementCollection(SectionName, CollectionName, out Collection))
                return false;

            return TryGetConfigurationHeaderElement(Collection, ElementName, out Element);
        }

        /// <summary>
        /// Tries to get a <see cref="ConfigurationHeaderElement"/> with name <paramref name="ElementName"/>
        /// from a <seealso cref="ConfigurationElementCollection{T}"/> with name <paramref name="CollectionName"/>
        /// that is contained in a configuration section <paramref name="Section"/>.
        /// </summary>
        /// <typeparam name="ElementType">The type of the <see cref="ConfigurationHeaderElement"/> in the <seealso cref="ConfigurationElementCollection{T}"/>.</typeparam>
        /// <param name="Section">The section that contains the <see cref="ConfigurationElementCollection{T}"/>.</param>
        /// <param name="CollectionName">The name of the <see cref="ConfigurationElementCollection{T}"/> that contains the <see cref="ConfigurationHeaderElement"/>.</param>
        /// <param name="ElementName">The name of the <see cref="ConfigurationHeaderElement"/> in the <seealso cref="ConfigurationElementCollection{T}"/>.</param>
        /// <param name="Element">The <see cref="ConfigurationHeaderElement"/> with name <paramref name="ElementName"/>.</param>
        /// <returns>
        /// True if a <see cref="ConfigurationHeaderElement"/> with name <paramref name="ElementName"/>
        /// from a <seealso cref="ConfigurationElementCollection{T}"/> with name <paramref name="CollectionName"/>
        /// that is contained in a configuration section <paramref name="Section"/> is found, false otherwise.
        /// </returns>
        public static bool TryGetConfigurationHeaderElement<ElementType>(ConfigurationSection Section, string CollectionName, string ElementName, out ElementType Element) where ElementType : ConfigurationHeaderElement, new()
        {
            Element = default(ElementType);
            if (string.IsNullOrEmpty(ElementName))
                return false;

            ConfigurationElementCollection<ElementType> Collection;
            if (!TryGetConfigurationElementCollection(Section, CollectionName, out Collection))
                return false;

            return TryGetConfigurationHeaderElement(Collection, ElementName, out Element);
        }

        /// <summary>
        /// Tries to get a <see cref="ConfigurationHeaderElement"/> with name <paramref name="ElementName"/>
        /// from the collection <paramref name="Collection"/>.
        /// </summary>
        /// <typeparam name="ElementType">The type of the <see cref="ConfigurationHeaderElement"/> in the <seealso cref="ConfigurationElementCollection{T}"/>.</typeparam>
        /// <param name="Collection">The <see cref="ConfigurationElementCollection{T}"/> that contains the <see cref="ConfigurationHeaderElement"/>.</param>
        /// <param name="ElementName">The name of the <see cref="ConfigurationHeaderElement"/> in the <seealso cref="ConfigurationElementCollection{T}"/>.</param>
        /// <param name="Element">The <see cref="ConfigurationHeaderElement"/> with name <paramref name="ElementName"/>.</param>
        /// <returns>
        /// True if a <see cref="ConfigurationHeaderElement"/> with name <paramref name="ElementName"/>
        /// from the collection <paramref name="Collection"/> is found, false otherwise.
        /// </returns>
        public static bool TryGetConfigurationHeaderElement<ElementType>(ConfigurationElementCollection<ElementType> Collection, string ElementName, out ElementType Element) where ElementType : ConfigurationHeaderElement, new()
        {
            Element = default(ElementType);
            if (Collection == null || string.IsNullOrEmpty(ElementName))
                return false;

            foreach (ElementType E in Collection)
            {
                if (E.Name.Equals(ElementName))
                {
                    Element = E;
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
