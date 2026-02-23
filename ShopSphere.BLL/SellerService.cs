using ShopSphere.Domain.Models;

public class SellerService : ISellerService
{
    private readonly ISellerRepository _repository;

    public SellerService(ISellerRepository repository)
    {
        _repository = repository;
    }


    public async Task<Seller?> GetSellerByUserIdAsync(string userId)
    {
        return await _repository.GetSellerByUserIdAsync(userId);
    }

    public async Task RegisterSellerAsync(
     string userId,
     string businessName,
     string phone,
     string address,
     string gst)
    {
        var existing = await _repository.GetSellerByUserIdAsync(userId);

        // First time applying
        if (existing == null)
        {
            var seller = new Seller
            {
                UserId = userId,
                BusinessName = businessName,
                PhoneNumber = phone,
                Address = address,
                GSTNumber = gst
            };

            await _repository.AddSellerAsync(seller);
            return;
        }

        //  Already Approved
        if (existing.Status == "Approved")
        {
            throw new Exception("You are already an approved seller.");
        }

        // Still Pending
        if (existing.Status == "Pending")
        {
            throw new Exception("Your application is still under review.");
        }

        //  Rejected (Check 30-day cooldown)
        if (existing.Status == "Rejected")
        {
            if (existing.RejectedDate.HasValue)
            {
                var nextAllowedDate = existing.RejectedDate.Value.AddDays(30);

                if (nextAllowedDate > DateTime.Now)
                {
                    throw new Exception(
                        $"You can reapply after {nextAllowedDate:dd MMM yyyy}.");
                }
            }

            // Cooldown completed → allow reapply
            existing.BusinessName = businessName;
            existing.PhoneNumber = phone;
            existing.Address = address;
            existing.GSTNumber = gst;

            await _repository.ReapplySellerAsync(existing);
            return;
        }

        throw new Exception("Invalid seller state.");
    }


    public async Task RejectSellerAsync(int sellerId, string reason)
    {
        await _repository.RejectSellerAsync(sellerId, reason);
    }

    public async Task<Seller?> GetSellerByIdAsync(int sellerId)
    {
        return await _repository.GetSellerByIdAsync(sellerId);
    }


    public async Task<IEnumerable<Seller>> GetPendingSellersAsync()
    {
        return await _repository.GetPendingSellersAsync();
    }

    public async Task ApproveSellerAsync(int sellerId)
    {
        await _repository.ApproveSellerAsync(sellerId);
    }

    public async Task<bool> SellerExistsAsync(string userId)
    {
        return await _repository.SellerExistsAsync(userId);
    }
}
