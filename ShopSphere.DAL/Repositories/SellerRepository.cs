using Dapper;
using System.Data;
using ShopSphere.Domain.Models;
using ShopSphere.DAL.Context;

public class SellerRepository : ISellerRepository
{
    private readonly DapperContext _context;

    public SellerRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<bool> SellerExistsAsync(string userId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Flag", "CheckSellerExists");
        parameters.Add("@UserId", userId);

        using var connection = _context.CreateConnection();
        var count = await connection.ExecuteScalarAsync<int>(
            "sp_Seller_CRUD",
            parameters,
            commandType: CommandType.StoredProcedure);

        return count > 0;
    }


    public async Task AddSellerAsync(Seller seller)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Flag", "InsertSeller");
        parameters.Add("@UserId", seller.UserId);
        parameters.Add("@BusinessName", seller.BusinessName);
        parameters.Add("@PhoneNumber", seller.PhoneNumber);
        parameters.Add("@Address", seller.Address);
        parameters.Add("@GSTNumber", seller.GSTNumber);

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync("sp_Seller_CRUD",
            parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<Seller?> GetSellerByUserIdAsync(string userId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Flag", "GetSellerByUserId");
        parameters.Add("@UserId", userId);

        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Seller>(
            "sp_Seller_CRUD", parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Seller?> GetSellerByIdAsync(int sellerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Flag", "GetSellerById");
        parameters.Add("@SellerId", sellerId);

        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Seller>(
            "sp_Seller_CRUD",
            parameters,
            commandType: CommandType.StoredProcedure);
    }


    public async Task<IEnumerable<Seller>> GetPendingSellersAsync()
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Flag", "GetPendingSellers");

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Seller>(
            "sp_Seller_CRUD", parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task ApproveSellerAsync(int sellerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Flag", "ApproveSeller");
        parameters.Add("@SellerId", sellerId);

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync("sp_Seller_CRUD",
            parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task RejectSellerAsync(int sellerId, string reason)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Flag", "RejectSeller");
        parameters.Add("@SellerId", sellerId);
        parameters.Add("@RejectionReason", reason);

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync("sp_Seller_CRUD",
            parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task ReapplySellerAsync(Seller seller)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Flag", "ReapplySeller");
        parameters.Add("@UserId", seller.UserId);
        parameters.Add("@BusinessName", seller.BusinessName);
        parameters.Add("@PhoneNumber", seller.PhoneNumber);
        parameters.Add("@Address", seller.Address);
        parameters.Add("@GSTNumber", seller.GSTNumber);

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync("sp_Seller_CRUD",
            parameters, commandType: CommandType.StoredProcedure);
    }


}
