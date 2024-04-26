using appHospital.Data.Repository.IRepository;
using appHospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace appHospital.Data.Repository
{
    public class UsuarioRepository: Repository<Usuario>,IUsuarioRepository
    {
        private readonly ApplicationDbContext _db;
        public UsuarioRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public void BloquearUsuario(string IdUsuario)
        {
            var usuarioDesdedb = _db.Usuario.FirstOrDefault( u => u.Id == IdUsuario);
            usuarioDesdedb.LockoutEnd = DateTime.Now.AddYears(1000);
            _db.SaveChanges();

        }
        public void DesbloquearUsuario(string IdUsuario)
        {
            var usuarioDesdedb = _db.Usuario.FirstOrDefault(u => u.Id == IdUsuario);
            usuarioDesdedb.LockoutEnd = DateTime.Now;
            _db.SaveChanges();
        }
    }
}
