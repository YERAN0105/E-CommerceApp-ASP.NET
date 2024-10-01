namespace ecomC.DTOs
{
    public class RankingDTO
    {
        public string VendorId { get; set; }  // Reference to the vendor
        public int Rating { get; set; }        // Rating given by the customer
        public string Comment { get; set; }    // Comment provided by the customer
        public string CustomerId { get; set; } // Reference to the customer
    }
}