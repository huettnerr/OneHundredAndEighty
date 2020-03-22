﻿#region Usings

using System;
using System.Globalization;
using NLog;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public class ConfigService
    {
        private readonly object locker;
        private readonly Logger logger;
        private readonly DBService dbService;

        public ConfigService(Logger logger, DBService dbService)
        {
            this.logger = logger;
            this.dbService = dbService;
            locker = new object();
        }

        public void Write(SettingsType key, object value)
        {
            lock (locker)
            {
                dbService.SettingsSetValue(key, Convert.ToString(value, CultureInfo.InvariantCulture));
            }
        }

        public T Read<T>(SettingsType key)
        {
            lock (locker)
            {
                object value;

                if (typeof(T) == typeof(double))
                {
                    value = Convert.ToDouble(dbService.SettingsGetValue(key), CultureInfo.InvariantCulture);
                }
                else if (typeof(T) == typeof(decimal))
                {
                    value = Convert.ToDecimal(dbService.SettingsGetValue(key), CultureInfo.InvariantCulture);
                }
                else if (typeof(T) == typeof(float))
                {
                    value = (float) Convert.ToDecimal(dbService.SettingsGetValue(key), CultureInfo.InvariantCulture);
                }
                else if (typeof(T) == typeof(int))
                {
                    value = Convert.ToInt32(dbService.SettingsGetValue(key));
                }
                else if (typeof(T) == typeof(bool))
                {
                    value = Convert.ToBoolean(dbService.SettingsGetValue(key));
                }
                else if (typeof(T) == typeof(string))
                {
                    value = dbService.SettingsGetValue(key);
                }
                else
                {
                    throw new Exception($"Not supported type for {nameof(Read)} method");
                }

                return (T) value;
            }
        }
    }
}