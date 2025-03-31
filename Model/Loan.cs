using System.ComponentModel.DataAnnotations;
public enum LoanStatus
{
    Pending = 1,
    Approved = 2,
    Denied = 3
}
public class Loan
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public double Amount { get; set; }
    public int Months { get; set; }
    public LoanStatus Status { get; set; } = LoanStatus.Pending;
    public int ReviewedBy { get; set; }
}