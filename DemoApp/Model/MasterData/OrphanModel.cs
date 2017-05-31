
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MasterData
{
    public class OrphanModel 
    {
        public OrphanModel()
        {
           // Uploads = new List<Upload>();
        }
        public string FullName { get; set; }
        public string FamilyName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DOB { get; set; }
        public int Age { get; set; }
        public string BirthCertificateNo { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        public string ProfileImage { get; set; }
        public string FatherName { get; set; }
        public DateTime? FatherDeathDate { get; set; }
      //  public Documents FatherCertificates { get; set; }
        public string FatherDeathReason { get; set; }
        public string FatherDeathCertificateNo { get; set; }
        public string FatherProfession { get; set; }
       // public Documents OrphanDocuments { get; set; }
        public string MotherName { get; set; }
        public DateTime? MotherDeathDate { get; set; }
      //  public Documents MotherCertificates { get; set; }
        public string MotherDeathCertificateNo { get; set; }
        public string MotherDeathReason { get; set; }
        public string MotherProfession { get; set; }
        public string GuardianName { get; set; }
        public string GuardianProfession { get; set; }
        public string GuardianNIC { get; set; }
        public string GuardianRelationship { get; set; }
      //  public Documents GuardianDocuments { get; set; }
        public string GuardianAddress { get; set; }
        public string Class { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public string AdmissionNumber { get; set; }
        public string EducationalStatus { get; set; }
        public string Sports { get; set; }
        public string ExtActivities { get; set; }
        public string Achievemnts { get; set; }
     //   public List<Upload> Uploads { get; set; }
        public string ACNO { get; set; }
        public string ACCode { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public float Amount { get; set; }
        public string SponserID { get; set; }
        public string SponserName { get; set; }
     //   public List<School> SchoolList { get; set; }
        public string Status { get; set; }
        public string OrphanCode { get; set; }
        public string District { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string MotherNIC { get; set; }
        public string FatherNIC { get; set; }
        public ResposiblePerson ResposiblePerson { get; set; }
        public string OrphanNo { get; set; }
        public string AcType { get; set; }
        public string Currency { get; set; }
        public string SupervisorID { get; set; }

        public string SupervisorName { get; set; }
    }
    public enum Gender
    {
        Male,
        Female
    }
    public enum ResposiblePerson
    {
        Father,
        Mother,
        Guardian
    }

}
