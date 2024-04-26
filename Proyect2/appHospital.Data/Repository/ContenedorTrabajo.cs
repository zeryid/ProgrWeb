using appHospital.Data.Repository.IRepository;
using appHospital.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using appHospital.Data.Repository;
using appHospital.Data.Repository.IRepository;
namespace appHospital.Data.Repository
{
    public class ContenedorTrabajo : IContenedorTrabajo
    {
        private readonly ApplicationDbContext _context;

        public ContenedorTrabajo(ApplicationDbContext context)
        {
            _context = context;
            //se agregan cada uno de los repositorios para que queden encapsulados
            //Rol = new RolRepository(_context);
            Usuario = new UsuarioRepository(_context);
        }


        //public IRolRepository Rol { get; private set; }
        public IUsuarioRepository Usuario { get; private set; }
        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
