namespace TrueMoney.Models.Basic
{
    using System;

    public class PassportModel
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public DateTime? DateOfIssuing { get; set; }

        public string GiveOrganization { get; set; }
    }
}