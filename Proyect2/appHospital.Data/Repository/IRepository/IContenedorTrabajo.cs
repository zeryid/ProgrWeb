using appHospital.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appHospital.Data.Repository.IRepository
{
    public interface IContenedorTrabajo : IDisposable
    {
        //IRolRepository Rol { get; }
        //IUsuarioRepository Usuario { get; }
        IUsuarioRepository Usuario { get; }
        void Save();
    }
}
