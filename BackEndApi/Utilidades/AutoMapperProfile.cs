
using AutoMapper;
using BackEndApi.DTOs;
using BackEndApi.Models;
using System.Globalization; 

namespace BackEndApi.Utilidades;

public class AutoMapperProfile: Profile
{

    public AutoMapperProfile() {
        #region Mascota

        CreateMap<Mascota, MascotaDTO>().ReverseMap();
        #endregion

    }
}
