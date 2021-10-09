namespace CalifornianHealth.WebBlazor.Models
{
    public class ConsultantModel
    {
        public int Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Specialty { get; set; }

        public override string ToString()
        {
            return $"{GivenName} {FamilyName}";
        }
    }
}