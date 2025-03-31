using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[Route("api/[controller]")]
[ApiController]
public class LoansController : ControllerBase
{
    private readonly AppDbContext _context;
    public LoansController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Loan>>> GetLoans()
    {
        return await _context.Loans.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Loan>> GetLoan(int id)
    {
        var user = await _context.Loans.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Loan>>> GetLoansByUser(int userId)
    {
        var loans = await _context.Loans.Where(l => l.UserId == userId).ToListAsync();

        if (loans == null || loans.Count == 0)
        {
            return NotFound("No loans found for this user.");
        }

        return Ok(loans);
    }

    [HttpPost]
    public async Task<ActionResult<Loan>> PostLoan(Loan user)
    {
        _context.Loans.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLoan), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutLoan(int id, Loan user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LoanExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLoan(int id)
    {
        var user = await _context.Loans.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        _context.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool LoanExists(int id)
    {
        return _context.Loans.Any(e => e.Id == id);
    }
}