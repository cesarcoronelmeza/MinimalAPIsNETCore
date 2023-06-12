using BackEndApi.Models; 

namespace BackEndApi.Services.Contrato;

public interface IMascotaService
{
    Task<List<Mascota>> GetMascotaListAsync();
    Task<Mascota> GetMascotaById(int id);
    Task<Mascota> Add(Mascota model);
    Task<bool> Update(Mascota model);
    Task<bool> Delete(Mascota model);
}
