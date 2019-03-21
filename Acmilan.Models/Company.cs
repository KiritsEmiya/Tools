namespace Acmilan.Models
{
    public class Company
    {
        public int Company_id { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string Telephone { get; set; }
        public string Facsimile { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Services { get; set; }
        public int Company_category_id { get; set; }
    }
}
