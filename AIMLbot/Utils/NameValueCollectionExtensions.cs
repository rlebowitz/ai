﻿using System.Collections.Specialized;
using System.ComponentModel;

namespace AIMLbot.Utils
{
    //http://stackoverflow.com/questions/7189509/configurationmanager-static-class/34053423#34053423
    public static class NameValueCollectionExtensions
    {
        public static T Get<T>(this NameValueCollection collection, string key, T defaultValue)
        {
            var value = collection[key];
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (string.IsNullOrWhiteSpace(value) || !converter.IsValid(value))
            {
                return defaultValue;
            }

            return (T)(converter.ConvertFromInvariantString(value));
        }
    }
}