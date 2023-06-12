using Microsoft.EntityFrameworkCore;
using BackEndApi.Models;
using BackEndApi.Services.Contrato;

namespace BackEndApi.Services.Implementacion;

public class MascotaService:IMascotaService
{
    private DbMascotasContext _dbContext;

    public MascotaService(DbMascotasContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task<List<Mascota>> GetMascotaListAsync()
    {
        List<Mascota> lst = new List<Mascota>();
        try { 
            
            lst = await _dbContext.Mascota.ToListAsync();
        }
        catch (Exception ex)
        {
            throw ex; 
        }
        return lst; 
    }

    public async Task<Mascota> GetMascotaById(int id)
    {
        Mascota obj = new Mascota();
        try
        { 
            obj = await _dbContext.Mascota.Where(e => e.IdMascota == id ).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return obj;
    } 
    public async Task<Mascota> Add(Mascota model)
    {
        try { 
            _dbContext.Mascota.Add(model);
            await _dbContext.SaveChangesAsync();
            return model;  
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<bool> Delete(Mascota model )
    {
        try
        {
            _dbContext.Mascota.Remove(model);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    } 

    public async Task<bool> Update(Mascota model)
    {
        try
        {
            _dbContext.Mascota.Update(model);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
