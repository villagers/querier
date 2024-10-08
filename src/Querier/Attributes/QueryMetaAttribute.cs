﻿namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class QueryMetaAttribute : BaseAttribute, IMetaAttribute
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public QueryMetaAttribute(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
