using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Our.Umbraco.AuthU.Models
{
    /// <summary>
    /// POCO for [umbracoKeyValue] table
    /// </summary>
    class UmbracoKeyValue
    {
        [Column("[key]")]
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime dateTime { get; set; }
    }
}
