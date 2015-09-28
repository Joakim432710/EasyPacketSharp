using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ChatClient
{
    /// <summary>
    ///     Implementation of <see cref="INotifyPropertyChanged" /> to simplify models.
    /// </summary>
    public abstract class ExtendedNotificationBase : INotifyPropertyChanged
    {
        /// <summary>
        ///     Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Maps a property name to its internal value holder (non-accessible through other means)
        /// </summary>
        private readonly Dictionary<string, object> _propertyValueMap;

        /// <summary>
        ///     Initializes a bindable base
        /// </summary>
        protected ExtendedNotificationBase()
        {
            _propertyValueMap = new Dictionary<string, object>();
        }

        /// <summary>
        ///     Internally takes care of event firing.
        /// </summary>
        /// <param name="propertyName"></param>
        private void InternalPropertyUpdate(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Notifies listeners that a property value has changed.
        /// </summary>
        /// <typeparam name="T">The type of the property that has changed.</typeparam>
        /// <param name="path">
        ///     An expression that evaluates to the property value.
        ///     Examples with lambda could be () => PropertyName
        /// </param>
        protected void OnPropertyChanged<T>(Expression<Func<T>> path)
        {
            var propertyName = GetPropertyName(path);
            InternalPropertyUpdate(propertyName);
        }

        /// <summary>
        ///     Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">
        ///     Name of the property used to notify listeners.  This
        ///     value is optional and can be provided automatically when invoked from compilers
        ///     that support <see cref="CallerMemberNameAttribute" />.
        /// </param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            InternalPropertyUpdate(propertyName);
        }

        /// <summary>
        ///     Uses an expression path to evaluate the property value.
        ///     Values are stored in the internal propertyValueMap of type Dictionary<string, object>.
        /// </summary>
        /// <typeparam name="T">The type of the property that has changed.</typeparam>
        /// <param name="path">
        ///     An expression that evaluates to the property value.
        ///     Examples with lambda could be () => PropertyName
        /// </param>
        /// <returns>The value of the property.</returns>
        protected T Get<T>(Expression<Func<T>> path)
        {
            return Get(path, default(T));
        }

        /// <summary>
        ///     Uses an expression path to evaluate the property value.
        ///     Values are stored in the internal propertyValueMap of type Dictionary<string, object>.
        /// </summary>
        /// <typeparam name="T">The type of the property that has changed.</typeparam>
        /// <param name="path">
        ///     An expression that evaluates to the property value.
        ///     Examples with lambda could be () => PropertyName
        /// </param>
        /// <param name="defaultValue">The default value to assign should the variable not be initialized.</param>
        /// <returns>The value of the property.</returns>
        protected virtual T Get<T>(Expression<Func<T>> path, T defaultValue)
        {
            var propertyName = GetPropertyName(path);
            if (_propertyValueMap.ContainsKey(propertyName))
                return (T)_propertyValueMap[propertyName];

            _propertyValueMap.Add(propertyName, defaultValue);
            return defaultValue;
        }

        /// <summary>
        ///     Uses an expression path to set the property value.
        ///     Implements correct INotifyPropertyChange logic only calling OnpropertyChanged if the value has changed.
        ///     Values are stored in the internal propertyValueMap of type Dictionary<string, object>.
        /// </summary>
        /// <typeparam name="T">The type of the property that has changed.</typeparam>
        /// <param name="path">
        ///     An expression that evaluates to the property value.
        ///     Examples with lambda could be () => PropertyName
        /// </param>
        /// <param name="value">The value to assign the property.</param>
        protected void Set<T>(Expression<Func<T>> path, T value)
        {
            Set(path, value, false);
        }

        /// <summary>
        ///     Uses an expression path to set the property value.
        ///     Implements correct INotifyPropertyChange logic only calling OnpropertyChanged if the value has changed but can be overloaded with forceUpdate = true.
        ///     Values are stored in the internal propertyValueMap of type Dictionary<string, object>.
        /// </summary>
        /// <typeparam name="T">The type of the property that has changed.</typeparam>
        /// <param name="path">
        ///     An expression that evaluates to the property value.
        ///     Examples with lambda could be () => PropertyName
        /// </param>
        /// <param name="value">The value to assign the property.</param>
        /// <param name="forceUpdate">Whether to ignore proper logic and just force a notification to appear in the subscriber event</param>
        protected virtual void Set<T>(Expression<Func<T>> path, T value, bool forceUpdate)
        {
            var oldValue = Get(path);
            var propertyName = GetPropertyName(path);

            if (Equals(value, oldValue) && !forceUpdate) return;
            _propertyValueMap[propertyName] = value;
            OnPropertyChanged(path);
        }

        /// <summary>
        ///     Retrieves the property name based on an expression.
        ///     Example: "() => PropertyName" returns PropertyName
        ///     Other ExpressionTypes are permitted
        /// </summary>
        /// <typeparam name="T">The returntype of the expression</typeparam>
        /// <param name="expression">The expression to retrieve the name from</param>
        /// <returns>The PropertyName</returns>
        protected static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var body = expression.Body;
            var memberExpression = body as MemberExpression ?? (MemberExpression)((UnaryExpression)body).Operand;
            return memberExpression.Member.Name;
        }

    }
}
