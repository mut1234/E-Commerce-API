using E_Commerce_API.Dto.Invoice;
using E_Commerce_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoicesController : ControllerBase
    {
        private readonly ECommerceDbContext _context;

        public InvoicesController(ECommerceDbContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        public async Task<ActionResult<InvoiceResponseDto>> CreateInvoice(List<CreateInvoiceDto> listCreateInvoiceDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Invalid user");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null )
            {
                return NotFound("User not found");
            }

            if (listCreateInvoiceDto == null || listCreateInvoiceDto.Count == 0)
            {
                return BadRequest("Invoice must contain at least one item");
            }

            var invoice = new Invoice
            {
                Date = DateTime.UtcNow,
                UserId = userId,
                TotalAmount = 0,
                Details = new List<InvoiceDetail>()
            };

            decimal totalAmount = 0;
            var listIds = listCreateInvoiceDto.Select(p => p.ProductId).ToList();
            var ListProducts = await _context.Products.Where(p=> listIds.Contains(p.Id)).ToListAsync();

            foreach (var item in listCreateInvoiceDto)
            {
                var product = ListProducts
                    .Where(p => p.Id == item.ProductId)
                    .FirstOrDefault();

                if (product == null)
                {
                    return BadRequest($"Product with ID {item.ProductId} not found");
                }

                if (item.Quantity <= 0)
                {
                    return BadRequest("Quantity must be greater than zero");
                }

                var detail = new InvoiceDetail
                {
                    ProductId = product.Id,
                    Price = product.Price,
                    Quantity = item.Quantity
                };

                invoice.Details.Add(detail);
                totalAmount += product.Price * item.Quantity;
            }

            invoice.TotalAmount = totalAmount;

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            var response =  MapInvoiceResponseDto(invoice , user , ListProducts);
            return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, response);
        }

        [HttpGet]
        public async Task<ActionResult<List<InvoiceSummaryDto>>> GetMyInvoices()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Invalid user");
            }

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            IQueryable<Invoice> query = _context.Invoices.Include(i => i.User);

            if (userRole != "Admin")
            {
                query = query.Where(i => i.UserId == userId);
            }

            var invoices = await query
                .OrderByDescending(i => i.Date)
                .Select(i => new InvoiceSummaryDto
                {
                    Id = i.Id,
                    Date = i.Date,
                    UserName = i.User.FullName,
                    TotalAmount = i.TotalAmount,
                    ItemsCount = i.Details.Count
                })
                .ToListAsync();

            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceResponseDto>> GetInvoice(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Invalid user");
            }

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var invoice = await GetInvoiceById(id);

            if (invoice == null)
            {
                return NotFound("Invoice not found");
            }

            if (userRole != "Admin" && invoice.UserId != userId)
            {
                return Forbid();//403
            }

            return Ok(invoice);
        }
        private async Task<InvoiceResponseDto> MapInvoiceResponseDto(Invoice inv, User user,List<Product> products)
        {
            var dto = new InvoiceResponseDto
            {
                Id = inv.Id,
                Date = inv.Date,
                TotalAmount = inv.TotalAmount,
                UserEmail = user.Email,
                UserName = user.Username,
                UserId = user.Id,
                Items = products.Select(p => new InvoiceItemDto
                {
                    ProductNameAr = p.NameAr,
                    ProductNameEn = p.NameEn,
                    ProductId = p.Id,
                    Price = p.Price,
                    Quantity = inv.Details.First(o => o.ProductId == p.Id).Quantity,
                    Subtotal = p.Price * (inv.Details.First(o => o.ProductId == p.Id).Quantity)


                }).ToList()
            };
            return dto;
        }
        private async Task<InvoiceResponseDto> GetInvoiceById(int id)
        {
            return await _context.Invoices
                .Where(i => i.Id == id)
                .Include(i => i.User)
                .Include(i => i.Details)
                .ThenInclude(d => d.Product)
                .Select(i => new InvoiceResponseDto
                {
                    Id = i.Id,
                    Date = i.Date,
                    UserId = i.UserId,
                    UserName = i.User.FullName,
                    UserEmail = i.User.Email,
                    TotalAmount = i.TotalAmount,
                    Items = i.Details.Select(d => new InvoiceItemDto
                    {
                        ProductId = d.ProductId,
                        ProductNameAr = d.Product.NameAr,
                        ProductNameEn = d.Product.NameEn,
                        Price = d.Price,
                        Quantity = d.Quantity,
                        Subtotal = d.Price * d.Quantity
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}