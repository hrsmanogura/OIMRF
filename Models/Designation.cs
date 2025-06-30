using OIMRF.Models;



namespace OIMRF.Models
{
    public class Designation
    {
        public Guid DesignationID { get; set; }
        public string Title { get; set; }

        public ICollection<User> Users { get; set; }
    }
}