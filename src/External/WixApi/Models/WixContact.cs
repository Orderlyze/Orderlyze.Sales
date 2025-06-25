using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WixApi.Models
{
    public class Address
    {
        public string country { get; set; }
        public string subdivision { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
        public string addressLine { get; set; }
        public string addressLine2 { get; set; }
        public StreetAddress streetAddress { get; set; }
    }

    public class Addresses
    {
        public List<Item> items { get; set; }
    }

    public class Emails
    {
        public List<Item> items { get; set; }
    }

    public class ExtendedFieldItems
    {
        [JsonPropertyName("contacts.displayByLastName")]
        public string Name { get; set; }
        [JsonPropertyName("custom.branche")]
        public string Branche { get; set; }
    }

    public class ExtendedFields
    {
        public ExtendedFieldItems items { get; set; }
    }

    public class Info
    {
        public Name name { get; set; }
        public Emails emails { get; set; }
        public Phones phones { get; set; }
        public Addresses addresses { get; set; }
        public string company { get; set; }
        public string jobTitle { get; set; }
        public string birthdate { get; set; }
        public string locale { get; set; }
        public LabelKeys labelKeys { get; set; }
        public ExtendedFields extendedFields { get; set; }
        public Locations locations { get; set; }
    }

    public class Item
    {
        public string id { get; set; }
        public string tag { get; set; }
        public string email { get; set; }
        public bool primary { get; set; }
        public string countryCode { get; set; }
        public string phone { get; set; }
        public string e164Phone { get; set; }
        public Address address { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("custom.color")]
        public string customcolor { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("custom.number")]
        public int customnumber { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("custom.date")]
        public string customdate { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("custom.url")]
        public string customurl { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("members.membershipStatus")]
        public string membersmembershipStatus { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("emailSubscriptions.subscriptionStatus")]
        public string emailSubscriptionssubscriptionStatus { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("emailSubscriptions.deliverabilityStatus")]
        public string emailSubscriptionsdeliverabilityStatus { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("emailSubscriptions.effectiveEmail")]
        public string emailSubscriptionseffectiveEmail { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("ecom.numOfPurchases")]
        public int ecomnumOfPurchases { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("ecom.totalSpentAmount")]
        public int ecomtotalSpentAmount { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("ecom.totalSpentCurrency")]
        public string ecomtotalSpentCurrency { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("ecom.lastPurchaseDate")]
        public DateTime ecomlastPurchaseDate { get; set; }
    }

    public class LabelKeys
    {
        public List<string> items { get; set; }
    }

    public class LastActivity
    {
        public DateTime activityDate { get; set; }
        public string activityType { get; set; }
    }

    public class Locations
    {
        public List<string> items { get; set; }
    }

    public class Name
    {
        public string first { get; set; }
        public string last { get; set; }
    }

    public class Phones
    {
        public List<Item> items { get; set; }
    }

    public class Picture
    {
        public string id { get; set; }
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }

    public class PrimaryInfo
    {
        public string email { get; set; }
        public string phone { get; set; }
    }

    public class WixContact
    {
        public string id { get; set; }
        public int revision { get; set; }
        public Source source { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime updatedDate { get; set; }
        public LastActivity lastActivity { get; set; }
        public PrimaryInfo primaryInfo { get; set; }
        public Picture picture { get; set; }
        public Info info { get; set; }
    }

    public class Source
    {
        public string sourceType { get; set; }
    }

    public class StreetAddress
    {
        public string number { get; set; }
        public string name { get; set; }
    }

}
