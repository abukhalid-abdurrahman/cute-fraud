﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Fraud.Concerns.Extensions
{
    public static class AttributeExtensions
    {
        public static string GetDescription(this Enum value)
        {
            return value?.GetType()
                .GetMember(value.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description;
        }
    }
}