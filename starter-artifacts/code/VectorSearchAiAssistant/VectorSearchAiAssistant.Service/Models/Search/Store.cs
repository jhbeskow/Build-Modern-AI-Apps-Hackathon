using Azure.Search.Documents.Indexes;
using VectorSearchAiAssistant.SemanticKernel.Models;
using VectorSearchAiAssistant.SemanticKernel.TextEmbedding;

namespace VectorSearchAiAssistant.Service.Models.Search
{

    public class Store : EmbeddedEntity
    {
        [SearchableField(IsFilterable = true, IsFacetable = true)]
        [EmbeddingField(Label = "Store name")]
        public string storeName { get; set; }

        [SearchableField]
        [EmbeddingField(Label = "Store address")]
        public StoreAddress address { get; set; }

        [SearchableField]
        [EmbeddingField(Label = "Store email address")]
        public string emailAddress { get; set; }
        [SearchableField]
        [EmbeddingField(Label = "Store phone number")]
        public string phoneNumber { get; set; }

        public Store(string id, string storeName, StoreAddress address, string emailAddress, string phoneNumber)
        {
            this.id = id;
            this.storeName = storeName;
            this.address = address;
            this.emailAddress = emailAddress;
            this.phoneNumber = phoneNumber;
        }

        public Store()
        {
        }
    }

    public class StoreAddress
    {
        [SearchableField]
        [EmbeddingField(Label = "Store address line 1")]
        public string addressLine1 { get; set; }
        [SearchableField]
        [EmbeddingField(Label = "Customer address line 2")]
        public string addressLine2 { get; set; }
        [SearchableField]
        [EmbeddingField(Label = "Customer address city")]
        public string city { get; set; }
        [SearchableField]
        [EmbeddingField(Label = "Customer address state")]
        public string state { get; set; }
        [SearchableField]
        [EmbeddingField(Label = "Customer address country")]
        public string country { get; set; }
        [SearchableField]
        [EmbeddingField(Label = "Customer address zip code")]
        public string zipCode { get; set; }
        [SimpleField]
        public Location location { get; set; }

        public StoreAddress(string addressLine1, string addressLine2, string city, string state, string country, string zipCode, Location location)
        {
            this.addressLine1 = addressLine1;
            this.addressLine2 = addressLine2;
            this.city = city;
            this.state = state;
            this.country = country;
            this.zipCode = zipCode;
            this.location = location;
        }

        public class Location
        {
            [SimpleField]
            public string type { get; set; }
            [FieldBuilderIgnore]
            public List<float> coordinates { get; set; }

            public Location(string type, List<float> coordinates)
            {
                this.type = type;
                this.coordinates = coordinates;
            }
        }
    }
}
